using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class VFXNoteLane : MonoBehaviour {
    public NoteHitEventChannel noteHitEventChannel;

    public VisualEffect vfxPerfect, vfxGreat, vfxGood;
    public NOTE_BOARD_LANES lane;

    private void OnEnable() {
        noteHitEventChannel.onNoteHit += OnNoteHit;
    }


    private void OnDisable() {
        noteHitEventChannel.onNoteHit -= OnNoteHit;
    }

    private void OnNoteHit(NOTE_BOARD_LANES lane, NOTE_HIT_TYPES noteHitType, int playerIndex) {
        // if(playerIndex == ...) {}
        if (this.lane != lane) return;

        switch (noteHitType) {
            case NOTE_HIT_TYPES.PERFECT:
                vfxPerfect.Play();
                break;
            case NOTE_HIT_TYPES.GREAT:
                vfxGreat.Play();
                break;
            case NOTE_HIT_TYPES.GOOD:
                vfxGood.Play();
                break;
        }
    }
}
