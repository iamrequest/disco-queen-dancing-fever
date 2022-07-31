using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SmfLite;
using Sirenix.OdinInspector;

// See also: https://breno.sarmen.to/midi_documentation/list.html
// See also: https://github.com/keijiro/smflite/tree/test
//  Some observations: 
//      - The status can be looked up in the above documentation. 
//          - 0x9n is for note on, 0x8n is for note off.
//          - Haven't come across other statuses in the input stream yet
//      - Data1 represents pitch (0-255 I think, where 0 is C-1, 1 is C0, etc)
//      - Data2 represents velocity (0-127)
public class MidiReadTest : MonoBehaviour {
    private MidiTrackSequencer sequencer;
    public TextAsset midiFile;
    public float bpm;
    public int track;

    // Start is called before the first frame update
    void Start() {
    }

    // Update is called once per frame
    void Update() {
        if (sequencer == null) return;

        if (sequencer.Playing) {
            // Check events over the past time-slice
            List<MidiEvent> events = sequencer.Advance(Time.deltaTime);

            // Need to null check because this list can be null, instead of 0-size.
            if (events != null) {
                foreach (MidiEvent e in events) {
                    // Note: e.status.ToString("X") prints the input stream in bytecode format (eg: 0x90, but only the "90" part). Useful for comparing to the midi event chart linked above
                    //Debug.Log($"[{Time.time}]: {e.status.ToString("X")}, {e.data1}, {e.data2}");

                    if (IsNoteOn(e)) {
                        Debug.Log($"Note on: {e.data1}");
                    } else if (IsNoteOff(e)) {
                        Debug.Log($"Note off: {e.data1}");
                    }
                }
            }
        }
    }

    public bool IsNoteOn(MidiEvent e) {
        // Note on: 0x9n, where n can be anything
        return (e.status & 0xf0) == 0x90;
    }
    public bool IsNoteOff(MidiEvent e) {
        // Note off: 0x8n, where n can be anything
        return (e.status & 0xf0) == 0x80;
    }

    [Button]
    public void LoadSong() {
        MidiFileContainer song = MidiFileLoader.Load(midiFile.bytes);
        sequencer = new MidiTrackSequencer(song.tracks[track], song.division, bpm);
    }

    [Button]
    public void StartSong() {
        // Note: This should be done after a short delay, after loading the song. If done immediately after, there's apparently stuttering (see: smflite test branch link)
        sequencer.Start();
    }

    [Button]
    public void StopSong() {
        sequencer = null;
    }
}
