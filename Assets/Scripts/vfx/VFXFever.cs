using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class VFXFever : MonoBehaviour {
    public FeverEventChannel feverEventChannel;
    public GameStateEventChannel gameStateEventChannel;

    public VisualEffect feverReadyVFX;
    public VisualEffect feverActivatedVFX;
    public bool stopFeverReadyVFXOnFeverActivated;

    private void OnEnable() {
        gameStateEventChannel.onGameStateChange += OnGameStateChange;
        feverEventChannel.onFeverReady += StartFeverReadyVFX;
        feverEventChannel.onFeverActivated += StartFeverActivatedVFX;
        feverEventChannel.onFeverFinished += StopVFX;
    }
    private void OnDisable() {
        gameStateEventChannel.onGameStateChange -= OnGameStateChange;
        feverEventChannel.onFeverReady -= StartFeverReadyVFX;
        feverEventChannel.onFeverActivated -= StartFeverActivatedVFX;
        feverEventChannel.onFeverFinished -= StopVFX;
    }

    private void OnGameStateChange(GAME_STATE oldGameState, GAME_STATE newGameState) {
        switch (newGameState) {
            case GAME_STATE.GAME_WON:
            case GAME_STATE.GAME_LOST:
            case GAME_STATE.MAIN_MENU:
                StopVFX(0);
                break;
        }
    }

    private void StopVFX(int playerIndex) {
        if (feverActivatedVFX) {
            feverActivatedVFX.Stop();
        }

        if (feverReadyVFX) {
            feverReadyVFX.Stop();
        }
    }

    public void StartFeverReadyVFX(int playerIndex) {
        // if(playerIndex == ...) {}
        if (feverActivatedVFX) {
            feverActivatedVFX.Stop();
        }
        if (feverReadyVFX) {
            feverReadyVFX.Play();
        }
    }

    public void StartFeverActivatedVFX(int playerIndex) {
        // if(playerIndex == ...) {}
        if (feverReadyVFX && stopFeverReadyVFXOnFeverActivated) {
            feverReadyVFX.Stop();
        }
        if (feverActivatedVFX) {
            feverActivatedVFX.Play();
        }
    }
}
