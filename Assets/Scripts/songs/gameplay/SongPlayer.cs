using SmfLite;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Sirenix.OdinInspector;

public class SongPlayer : MonoBehaviour {
    public static SongPlayer Instance { get; private set; }
    private void Awake() {
        if (Instance == null) {
            Instance = this;
        } else {
            Debug.LogError($"Multiple {GetType()} components detected. This is probably a bug.");
            Destroy(this);
        }
    }

    public NoteBoard noteBoard;
    public AudioSource audioSource;

    private Coroutine startSongCoroutine;

    private MidiTrackSequencer sequencer;

    // Delay before trying to play the midi track, to prevent stuttering
    private const float preSongDelay = 1f;

    private void Update() {
        AdvanceMidi();
    }

    [Button]
    public void StartSong(SongMetadata songMetadata, SongDifficulty difficulty) {
        startSongCoroutine = StartCoroutine(StartSongAfterDelay(songMetadata, difficulty));
    }

    [Button]
    public void StopSong() {
        if (startSongCoroutine != null) {
            StopCoroutine(startSongCoroutine);
            startSongCoroutine = null;
        }

        audioSource.Stop();
        sequencer = null;
    }

    private IEnumerator StartSongAfterDelay(SongMetadata songMetadata, SongDifficulty difficulty) {
        // Load midi song
        MidiFileContainer song = MidiFileLoader.Load(MidiFileToBytes(songMetadata, difficulty));
        sequencer = new MidiTrackSequencer(song.tracks[difficulty.playerInputMidiTrack], song.division, songMetadata.bpm);

        // Fetch the audio clip from streaming assets
        yield return StartCoroutine(SongLoader.LoadAudioClip(audioSource, songMetadata));
        if (audioSource.clip == null) {
            Debug.LogError("Failed to load audio.");
            yield break;
        }

        // Wait a second before starting audio, to prevent stuttering
        yield return new WaitForSeconds(preSongDelay);

        audioSource.PlayDelayed(noteBoard.noteSpeed);
        sequencer.Start();
    }

    private byte[] MidiFileToBytes(SongMetadata songMetadata, SongDifficulty difficulty) {
        if (!File.Exists(songMetadata.fullDirectoryPath + "/" + difficulty.midiFilename)) {
            Debug.LogError($"MIDI file does not exist: {songMetadata.fullDirectoryPath + "/" + difficulty.midiFilename}");
        }
        return File.ReadAllBytes(songMetadata.fullDirectoryPath + "/" + difficulty.midiFilename);
    }

    private void AdvanceMidi() {
        if (sequencer == null) return;
        if (sequencer.Playing) {
            // Check events over the past time-slice
            List<MidiEvent> events = sequencer.Advance(Time.deltaTime);

            if (events != null) {
                foreach (MidiEvent e in events) {
                    // Next steps: Sustained notes
                    if (IsNoteOn(e)) {
                        SpawnNote(e);
                    }
                }
            }
        }
    }

    private bool IsNoteOn(MidiEvent e) {
        // Note on: 0x9n, where n can be anything
        return (e.status & 0xf0) == 0x90;
    }
    private bool IsNoteOff(MidiEvent e) {
        // Note off: 0x8n, where n can be anything
        return (e.status & 0xf0) == 0x80;
    }

    private void SpawnNote(MidiEvent e) {
        switch ((int) e.data1) {
            case 0:
                noteBoard.SpawnNote(NOTE_BOARD_LANES.LEFT);
                break;
            case 1:
                noteBoard.SpawnNote(NOTE_BOARD_LANES.UP);
                break;
            case 2:
                noteBoard.SpawnNote(NOTE_BOARD_LANES.DOWN);
                break;
            case 3:
                noteBoard.SpawnNote(NOTE_BOARD_LANES.RIGHT);
                break;
        }
    }
}
