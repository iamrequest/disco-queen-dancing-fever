using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum NOTE_BOARD_LANES { LEFT=0, UP = 1, DOWN=2, RIGHT=3}
public class NoteBoard : MonoBehaviour {
    public List<Note> activeNotes;

    public Transform noteOrigin, noteDestination;
    public GameObject notePrefab;

    public float boardWidth;
    private const int numPaths = 4;
    [Tooltip("The number of seconds it takes for a note to traverse the board")]
    public float noteSpeed;

    private Vector3 lrOffset;
    private Vector3 centerOffset;
    private void Start() {
        UpdateBoardSize();
    }

    private void Update() {
        foreach (Note n in activeNotes) {
            n.Move();
        }
    }

    public Vector3 GetNotePosition(float t, NOTE_BOARD_LANES laneNum) {
        float notePathWidth = boardWidth / numPaths;
        return Vector3.Lerp(noteOrigin.position - lrOffset + (Vector3.right * (int)laneNum * notePathWidth) + centerOffset, 
            noteDestination.position - lrOffset + (Vector3.right * (int)laneNum * notePathWidth) + centerOffset,
            t);
    }

    // Update calculated offsets, used when calculating note position movements
    private void UpdateBoardSize() {
        lrOffset = new Vector3(boardWidth / 2, 0f, 0f); // Offset to the right side 
        centerOffset = new Vector3(boardWidth / numPaths / 2, 0f, 0f);
    }

    private void OnDrawGizmosSelected() {
        UpdateBoardSize();

        // -- Draw the bounds of the board
        Gizmos.color = Color.white;
        Gizmos.DrawLine(noteOrigin.position - lrOffset, noteDestination.position - lrOffset);
        Gizmos.DrawLine(noteOrigin.position + lrOffset, noteDestination.position + lrOffset);

        // -- Draw the individual note paths
        Gizmos.color = Color.red;
        float notePathWidth = boardWidth / numPaths;

        for (int i = 0; i < numPaths; i++) {
            Vector3 pathOffset = Vector3.right * i * notePathWidth;
            pathOffset.x += notePathWidth / 2; // Center the note paths

            // TODO: This is broken
            Gizmos.DrawLine(noteOrigin.position - lrOffset + (Vector3.right * i * notePathWidth) + centerOffset,
                noteDestination.position - lrOffset + (Vector3.right * i * notePathWidth) + centerOffset);

            //Gizmos.DrawLine(noteOrigin.position - lrOffset + pathOffset,
            //    noteDestination.position - lrOffset + pathOffset);
        }

        Gizmos.color = Color.white;
    }


    /*
    // TODO: Optionally, make the distance to note origin user-configurable
    public float boardDistance;

    /// <summary>
    /// Save the distance from noteOrigin to noteDestination. Useful for inspector config
    /// </summary>
    [ButtonGroup("Board Distance")]
    private void SaveBoardDistance() {
        boardDistance = (noteDestination.position - noteOrigin.position).magnitude;
    }

    /// <summary>
    /// Move the note origin transform $boardDistance meters away from note destination 
    /// </summary>
    [ButtonGroup("Board Distance")]
    public void SetBoardDistance() {
        Vector3 destToOrigin = (noteOrigin.position - noteDestination.position).normalized;
        noteOrigin.position = destToOrigin * boardDistance;
    }
    */

    // --------------------------------------------------------------------------------
    // Editor Debug
    // --------------------------------------------------------------------------------
    // TODO: Object pool
    [Button]
    public void SpawnNote(NOTE_BOARD_LANES lane) {
        GameObject newNote = Instantiate(notePrefab, transform);
        Note n = newNote.GetComponent<Note>();

        n.noteBoard = this;
        n.lane = lane;

        activeNotes.Add(n);
    }

    [ButtonGroup("Spawn Notes")] [Button("<")]
    private void SpawnNoteLeft() { SpawnNote(NOTE_BOARD_LANES.LEFT); }
    [ButtonGroup("Spawn Notes")] [Button("^")]
    private void SpawnNoteUp() { SpawnNote(NOTE_BOARD_LANES.UP); }
    [ButtonGroup("Spawn Notes")] [Button("v")]
    private void SpawnNoteDown() { SpawnNote(NOTE_BOARD_LANES.DOWN); }
    [ButtonGroup("Spawn Notes")] [Button(">")]
    private void SpawnNoteRight() { SpawnNote(NOTE_BOARD_LANES.RIGHT); }
}
