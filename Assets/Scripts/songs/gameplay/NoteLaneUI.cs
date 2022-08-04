using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using Sirenix.OdinInspector;
using System;

[RequireComponent(typeof(NoteLane))]
public class NoteLaneUI : MonoBehaviour {
    public InputEventChannel inputEventChannel;
    public NoteHitEventChannel noteHitEventChannel;

    private NoteLane noteLane;
    public Image noteIconFill;
    public TextMeshProUGUI noteHitText;

    public Ease easeIn, easeOut;
    public float durationIn, durationOut;

    private void Awake() {
        noteLane = GetComponent<NoteLane>();
    }

    private void OnEnable() {
        inputEventChannel.onInput += OnInput;
        noteHitEventChannel.onNoteHit += OnNoteHit;

        ResetVFX();
    }

    private void OnDisable() {
        inputEventChannel.onInput -= OnInput;
        noteHitEventChannel.onNoteHit -= OnNoteHit;
    }

    private void Start() {
        DOTween.Init();
    }

    private void ResetVFX() {
        Color c = noteIconFill.color;
        c.a = 0f;
        noteIconFill.color = c;

        c = noteHitText.color;
        c.a = 0f;
        noteHitText.color = c;
    }


    private void OnInput (INPUT_DIRS inputDir, int playerIndex) {
        // if(playerIndex == noteLane.noteBoard.playerIndex) { }

        if ((int)inputDir == (int)noteLane.lane) {
            FlashNoteIcon();
        }
    }

    [Button]
    public void FlashNoteIcon() {
        Sequence seq = DOTween.Sequence();
        seq.Append(noteIconFill.DOFade(1f, durationIn).SetEase(easeIn));
        seq.Append(noteIconFill.DOFade(0f, durationOut).SetEase(easeOut));
    }

    private void OnNoteHit(NOTE_BOARD_LANES lane, NOTE_HIT_TYPES noteHitType, int playerIndex) {
        // if(playerIndex == noteLane.noteBoard.playerIndex) { }
        if (lane != noteLane.lane) return;

        switch (noteHitType) {
            case NOTE_HIT_TYPES.PERFECT:
                noteHitText.text = "PERFECT";
                break;
            case NOTE_HIT_TYPES.GREAT:
                noteHitText.text = "GREAT";
                break;
            case NOTE_HIT_TYPES.GOOD:
                noteHitText.text = "GOOD";
                break;
        }

        Sequence seq = DOTween.Sequence();
        seq.Append(noteHitText.DOFade(1f, durationIn).SetEase(easeIn));
        seq.Append(noteHitText.DOFade(0f, durationOut).SetEase(easeOut));
    }

}
