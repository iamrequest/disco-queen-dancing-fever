using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Being lazy and just making a single sfx player
public class SFXFloorInput : MonoBehaviour {
    public InputEventChannel inputEventChannel;
    public INPUT_DIRS inputDir;

    public List<AudioClip> sfxOnStep;

    [Tooltip("Only play onStep SFX during these game states")]
    public GAME_STATE gameStateOnStep;

    private void OnEnable() {
        inputEventChannel.onInput += OnInput;
    }

    private void OnDisable() {
        inputEventChannel.onInput -= OnInput;
    }

    private void OnInput(INPUT_DIRS inputDir, int playerIndex) {
        if (this.inputDir != inputDir) return;
        if ((GameManager.Instance.gameState & gameStateOnStep) == 0) return;

        SFXManager.Instance.PlayRandomSFX(sfxOnStep, transform.position);
    }
}
