using SmfLite;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Sirenix.OdinInspector;
using System;

public class SongPlayer : MonoBehaviour {
    public static SongPlayer Instance { get; private set; }
    public GameStateEventChannel gameStateEventChannel;

    public NoteBoard noteBoard;
    public AudioSource audioSource;
    public DifficultySettings difficultySettings; // For now, assume a single difficulty setting

    private Coroutine startSongCoroutine;
    private Coroutine stopSongAfterAudioCompletionCoroutine;

    private MidiTrackSequencer sequencer;

    // Delay before trying to play the midi track, to prevent stuttering
    private const float preSongDelay = 1f;
    public float elapsedMidiTime; // To get elapsed song time, subtract preSongDelay


    private void Awake() {
        if (Instance == null) {
            Instance = this;
        } else {
            Debug.LogError($"Multiple {GetType()} components detected. This is probably a bug.");
            Destroy(this);
        }
    }

    private void OnEnable() {
        gameStateEventChannel.onGameStateChange += OnGameStateChanged;
    }
    private void OnDisable() {
        gameStateEventChannel.onGameStateChange -= OnGameStateChanged;
    }

    private void Update() {
        if(GameManager.Instance.gameState == GAME_STATE.GAME_ACTIVE) {
            AdvanceMidi();
        }
    }

    private void OnGameStateChanged(GAME_STATE oldGameState, GAME_STATE newGameState) {
        switch (newGameState) {
            case GAME_STATE.GAME_PAUSED:
                PauseSong();
                break;
            case GAME_STATE.GAME_ACTIVE:
                if (oldGameState == GAME_STATE.GAME_PAUSED) {
                    UnpauseSong();
                }
                break;
            case GAME_STATE.GAME_OVER:
                StopSong();
                break;
        }
    }


    [ButtonGroup("Song Management")]
    public void StartSong(SongMetadata songMetadata, SongDifficulty difficulty) {
        startSongCoroutine = StartCoroutine(StartSongAfterDelay(songMetadata, difficulty));
    }

    [ButtonGroup("Song Management")]
    public void StopSong() {
        if (startSongCoroutine != null) {
            StopCoroutine(startSongCoroutine);
            startSongCoroutine = null;
        }

        if (stopSongAfterAudioCompletionCoroutine != null) {
            StopCoroutine(stopSongAfterAudioCompletionCoroutine);
            stopSongAfterAudioCompletionCoroutine = null;
        }

        audioSource.Stop();
        sequencer = null;

        // TODO: Despawn notes after a delay
    }

    [ButtonGroup("Song Management")]
    public void PauseSong() {
        audioSource.Pause();
    }

    [ButtonGroup("Song Management")]
    private void UnpauseSong() {
        audioSource.UnPause();
    }

    private IEnumerator StartSongAfterDelay(SongMetadata songMetadata, SongDifficulty difficulty) {
        // Load midi song
        MidiFileContainer song = MidiFileLoader.Load(MidiFileToBytes(songMetadata, difficulty));
        sequencer = new MidiTrackSequencer(song.tracks[difficulty.playerInputMidiTrack], song.division, songMetadata.bpm);

        // Fetch the audio clip from streaming assets
        yield return StartCoroutine(SongLoader.LoadAudioClip(audioSource, songMetadata));
        if (audioSource.clip == null) {
            Debug.LogError("Failed to load audio.");

            sequencer = null;
            yield break;
        }

        // Let the game manager know that we're starting the game
        gameStateEventChannel.SendOnRequestGameStateChange(GAME_STATE.GAME_ACTIVE);

        // Wait a second before starting audio, to prevent stuttering
        yield return new WaitForSeconds(preSongDelay);

        // Quit the game after the song is complete
        stopSongAfterAudioCompletionCoroutine = StartCoroutine(StopGameAfterAudioCompletion());

        // TODO: If the player pauses before the audio has started playing, then audio&midi will probably get out of sync. Likely just restart the song on unpause in this case
        audioSource.PlayDelayed(difficultySettings.noteLifetime);
        sequencer.Start();
    }

    private IEnumerator StopGameAfterAudioCompletion() {
        elapsedMidiTime = 0f;

        while (elapsedMidiTime < audioSource.clip.length) {
            if (GameManager.Instance.gameState == GAME_STATE.GAME_ACTIVE) {
                elapsedMidiTime += Time.deltaTime;
            }
            yield return null;
        }

        gameStateEventChannel.SendOnRequestGameStateChange(GAME_STATE.GAME_OVER);
        //StopSong(); // This gets called as a result of the game state change
    }


    // --------------------------------------------------------------------------------
    // MIDI file reading
    // --------------------------------------------------------------------------------
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
