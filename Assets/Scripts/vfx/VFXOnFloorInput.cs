using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class VFXOnFloorInput: MonoBehaviour {
    public InputEventChannel inputEventChannel;

    public VisualEffect vfx;
    public INPUT_DIRS inputDir;
    public GAME_STATE onlyPlayOnState;

    private void OnEnable() {
        inputEventChannel.onInput += OnInput;
    }


    private void OnDisable() {
        inputEventChannel.onInput -= OnInput;
    }

    private void OnInput(INPUT_DIRS inputDir, int playerIndex) {
        // if(playerIndex != ...) { }
        if (this.inputDir != inputDir) return;
        if ((GameManager.Instance.gameState & onlyPlayOnState) > 0) {
            vfx.Play();
        }
    }
}
