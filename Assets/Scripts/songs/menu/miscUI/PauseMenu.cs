using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Just a GameStateChanger, but it toggles the game state between Paused and Active, depending on the current game state
[RequireComponent(typeof(GazeMenuButton))]
public class PauseMenu : MonoBehaviour {
    public GameStateEventChannel gameStateEventChannel;
    private GazeMenuButton gazeMenuButton;

    private void Awake() {
        gazeMenuButton = GetComponent<GazeMenuButton>();
    }

    private void OnEnable() {
        gameStateEventChannel.onGameStateChange += OnGameStateChange;
        gazeMenuButton.onStateChanged.AddListener(SetPause);
    }

    private void OnDisable() {
        gameStateEventChannel.onGameStateChange -= OnGameStateChange;
        gazeMenuButton.onStateChanged.RemoveListener(SetPause);
    }

    private void OnGameStateChange(GAME_STATE oldGameState, GAME_STATE newGameState) {
        if (oldGameState == GAME_STATE.MAIN_MENU) {
            // Reset the button when the game starts (in the event that the player 
            gazeMenuButton.SetSelectedSilent(false, true);
        }
    }

    public void SetPause(bool buttonState) {
        if (buttonState) {
            gameStateEventChannel.SendOnRequestGameStateChange(GAME_STATE.GAME_PAUSED);
        } else {
            gameStateEventChannel.SendOnRequestGameStateChange(GAME_STATE.GAME_ACTIVE);
        }
    }
}
