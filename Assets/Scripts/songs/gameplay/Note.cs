using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Note : MonoBehaviour {
    [HideInInspector]
    public NoteBoard noteBoard;
    [HideInInspector]
    public NOTE_BOARD_LANES lane;

    public float t;

    public void Move() {
        if (t >= 1f) {
            // TODO. Maybe need to keep this alive a bit more than t=1, for when the player hits the note late
        } else {
            t += Time.deltaTime / noteBoard.noteSpeed;
            transform.position = noteBoard.GetNotePosition(t, lane);
        }
    }
}
