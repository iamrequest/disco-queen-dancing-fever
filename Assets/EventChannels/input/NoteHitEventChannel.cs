using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// https://www.youtube.com/watch?v=WLDgtRNK2VE
/// </summary>
[CreateAssetMenu(menuName = "Events/Note Hit Event Channel")]
public class NoteHitEventChannel : ScriptableObject {
    public UnityAction<NOTE_BOARD_LANES, NOTE_HIT_TYPES, int> onNoteHit;

    public void SendOnNoteHit(NOTE_BOARD_LANES lane, NOTE_HIT_TYPES noteHitType, int playerIndex) {
        if (onNoteHit != null) {
            onNoteHit.Invoke(lane, noteHitType, playerIndex);
        }
    }
}

