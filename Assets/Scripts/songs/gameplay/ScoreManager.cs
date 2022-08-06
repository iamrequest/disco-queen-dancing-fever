using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Maybe todo later: This shouldn't be a singleton, if it's managing only 1 score
public class ScoreManager : MonoBehaviour {
    public static ScoreManager Instance { get; private set; }
    private void Awake() {
        if (Instance == null) {
            Instance = this;
        } else {
            Debug.LogError($"Multiple {GetType()} components detected. This is probably a bug.");
            Destroy(this);
        }
    }

    public ScoreEventChannel scoreEventChannel;
    public GameStateEventChannel gameStateEventChannel;
    public int score;

    private void OnEnable() {
        scoreEventChannel.onRequestScoreChange += OnScoreChangeRequest;
        gameStateEventChannel.onGameStateChange += OnGameStateChange;
    }


    private void OnDisable() {
        scoreEventChannel.onRequestScoreChange -= OnScoreChangeRequest;
        gameStateEventChannel.onGameStateChange -= OnGameStateChange;
    }

    private void OnScoreChangeRequest(int scoreChange) {
        if (FeverModeManager.Instance.isFeverModeActive) {
            score += (2 * scoreChange);
            scoreEventChannel.SendOnScoreChanged(score - (2 * scoreChange), score);
        } else {
            score += scoreChange;
            scoreEventChannel.SendOnScoreChanged(score - scoreChange, score);
        }
    }

    private void OnGameStateChange(GAME_STATE oldGameState, GAME_STATE newGameState) {
        switch (newGameState) {
            case GAME_STATE.MAIN_MENU:
                ResetScore();
                break;
            case GAME_STATE.GAME_ACTIVE:
                if (oldGameState == GAME_STATE.GAME_LOST || oldGameState == GAME_STATE.GAME_WON) {
                    ResetScore();
                }
                break;
        }
    }


    private void ResetScore() {
        score = 0;
        scoreEventChannel.SendOnScoreReset();
    }
}
