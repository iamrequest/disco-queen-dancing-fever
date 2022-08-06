using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameOverUI : MonoBehaviour {
    public GameStateEventChannel gameStateEventChannel;
    public TextMeshProUGUI uiText;

    private void OnEnable() {
        gameStateEventChannel.onGameStateChange += OnGameStateChanged;
    }

    private void OnDisable() {
        gameStateEventChannel.onGameStateChange += OnGameStateChanged;
    }

    private void OnGameStateChanged(GAME_STATE oldGameState, GAME_STATE newGameState) {
        if (newGameState == GAME_STATE.GAME_WON) {
            uiText.text = "That's a wrap!";
        } else if (newGameState == GAME_STATE.GAME_LOST) {
            uiText.text = "You died!";
        }
    }
}
