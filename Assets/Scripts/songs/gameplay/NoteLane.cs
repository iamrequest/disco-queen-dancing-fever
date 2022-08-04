using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteLane : MonoBehaviour {
    public NoteBoard noteBoard;
    public NOTE_BOARD_LANES lane;

    // This stores a sliding window of notes, fron oldestNoteIndex to (oldestNoteIndex + newestNoteIndex).
    // These indicies wrap around from maxNumNotes back to 0, and will be in the range of [0, maxNumNotes-1]
    [ShowInInspector][HideInEditorMode]
    protected Note[] notes;

    [ReadOnly] public int oldestNoteIndex;
    [ShowInInspector] [ReadOnly] private int count;


    private void Awake() {
        notes = new Note[noteBoard.maxNumNotesPerLane];
    }

    // --------------------------------------------------------------------------------
    // Sliding Window Queue, for notes
    // --------------------------------------------------------------------------------
    public void EnqueueNote(Note n) {
        if (count >= noteBoard.maxNumNotesPerLane - 1) {
            Debug.LogWarning($"Attempted to exceed the capacity of this note lane ({lane})");
            return;
        }

        notes[(oldestNoteIndex + count) % noteBoard.maxNumNotesPerLane] = n;
        count++;
        if(count == 0) oldestNoteIndex = IncrementIndex(oldestNoteIndex);
    }

    public Note DequeueNote() {
        Note n = notes[oldestNoteIndex];
        oldestNoteIndex = IncrementIndex(oldestNoteIndex);
        count--;
        return n;
    }

    /// <summary>
    /// Clears the list of notes, and destroys the notes in it
    /// </summary>
    [Button]
    public void Clear() {
        foreach (Note n in notes) {
            if (n != null) {
                Destroy(n.gameObject);
            }
            oldestNoteIndex = 0;
            count = 0;
        }
    }

    // Fetches the n'th note, where the 0'th note is the oldest note (but not necessarially at index 0)
    public Note GetNoteAtOrder(int i) {
        int index = (oldestNoteIndex + i) % noteBoard.maxNumNotesPerLane;
        return notes[index];
    }

    // Fetches the n'th note, where the 0'th note is at index 0 (but not necessarially at the oldest note)
    public Note GetNoteAtIndex(int i) {
        return notes[i];
    }

    private int IncrementIndex(int index) {
        return (index + 1) % noteBoard.maxNumNotesPerLane;
    }

    // --------------------------------------------------------------------------------
    // Gameplay
    // --------------------------------------------------------------------------------

    [Button]
    public void SpawnNote() {
        GameObject newNote = Instantiate(noteBoard.notePrefab, transform);
        Note n = newNote.GetComponent<Note>();

        n.noteBoard = noteBoard;
        n.lane = lane;
        n.spawnTime = SongPlayer.Instance.elapsedMidiTime;

        // Move the note into position
        n.transform.position = noteBoard.GetNotePosition(0f, lane);

        EnqueueNote(n);
    }

    public void MoveNotes() {
        foreach (Note n in notes) {
            if (n != null) {
                n.Move();
            }
        }
    }

    // --------------------------------------------------------------------------------
    // Editor 
    // --------------------------------------------------------------------------------
    // Used to align the icon buttons in editor
    [Button]
    private void Align() {
        Vector3 newPos = noteBoard.GetNotePosition(1f, lane);
        transform.position = new Vector3(newPos.x, newPos.y, transform.position.z);
    }
}
