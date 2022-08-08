using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FeverModeManager : MonoBehaviour {
    public static FeverModeManager Instance { get; private set; }

    [Title("Scriptable objects")]
    public GameStateEventChannel gameStateEventChannel;
    public ScoreEventChannel scoreEventChannel;
    public InputEventChannel inputEventChannel;
    public FeverEventChannel feverEventChannel;
    public DifficultySettings difficultySettings;

    [Title("References")]
    public Slider slider;
    public TextMeshProUGUI feverUIText;

    [Title("DEBUG")]
    [Range(0f, 1f)]
    public float feverGauge;
    public float elapsedFeverDuration;
    public bool isFeverModeActive, isFeverModeAvailable, isFeverModeUsed;

    private void Awake() {
        if (Instance == null) {
            Instance = this;
        } else {
            Debug.LogError($"Multiple {GetType()} components detected. This is probably a bug.");
            Destroy(this);
        }
    }

    private void OnEnable() {
        gameStateEventChannel.onGameStateChange += OnGameStateChange;
        scoreEventChannel.onScoreChanged += OnScoreChange;
        scoreEventChannel.onScoreReset += OnScoreReset;
        inputEventChannel.onJumpStarted += OnJump;
    }


    private void OnDisable() {
        gameStateEventChannel.onGameStateChange -= OnGameStateChange;
        scoreEventChannel.onScoreChanged -= OnScoreChange;
        scoreEventChannel.onScoreReset -= OnScoreReset;
        inputEventChannel.onJumpStarted -= OnJump;
    }


    // --------------------------------------------------------------------------------
    // Events
    // --------------------------------------------------------------------------------
    private void OnGameStateChange(GAME_STATE oldGameState, GAME_STATE newGameState) {
        switch (newGameState) {
            case GAME_STATE.GAME_ACTIVE:
                if (oldGameState != GAME_STATE.GAME_PAUSED) {
                    ResetFeverMode();
                    Render();
                }
                break;
            case GAME_STATE.MAIN_MENU:
            case GAME_STATE.GAME_LOST: 
            case GAME_STATE.GAME_WON: 
                ResetFeverMode();
                Render();
                break;
        }
    }

    private void OnScoreReset() {
        ResetFeverMode();
        Render();
    }

    private void OnScoreChange(int scoreChange, int newScore) {
        CalculateFeverGauge(newScore);
        Render();

        if (!isFeverModeUsed && !isFeverModeActive && !isFeverModeAvailable) {
            if (feverGauge >= 1f) {
                isFeverModeAvailable = true;
                feverEventChannel.SendOnFeverReady(0);
            }
        }
    }

    private void OnJump(int playerIndex) {
        // For multiple players: ActivateFeverMode(playerIndex);
        ActivateFeverMode();
    }

    // --------------------------------------------------------------------------------
    // Fever mode rendering
    // --------------------------------------------------------------------------------
    private void CalculateFeverGauge(int score) {
        if (SongPlayer.Instance.currentDifficulty != null) {
            float minScoreToGetFeverMode = SongPlayer.Instance.currentDifficulty.numNotes * difficultySettings.scoreValuePerfect * difficultySettings.perfectScorePercentageToFeverMode;
            feverGauge = score / minScoreToGetFeverMode;
        }
    }

    [ButtonGroup]
    private void Render() {
        feverUIText.text = "FEVER";
        feverUIText.lineSpacing = 70f;

        if (isFeverModeUsed) {
            slider.value = 0f;
        } else {
            slider.value = feverGauge;
        }

        if (isFeverModeAvailable && !isFeverModeUsed) {
            feverUIText.text = "JUMP!";
            feverUIText.lineSpacing = 70f;
        } else if (isFeverModeActive) {
            slider.value = (difficultySettings.feverModeDuration - elapsedFeverDuration) / difficultySettings.feverModeDuration;

            feverUIText.text = "2X\n\nSCORE";
            feverUIText.lineSpacing = 0f;
        }
    }

    // --------------------------------------------------------------------------------
    // Fever mode - General
    // --------------------------------------------------------------------------------
    private void ResetFeverMode() {
        isFeverModeActive = false;
        isFeverModeAvailable = false;
        isFeverModeUsed = false;

        feverGauge = 0f;
        Render();
    }

    private void FinishFeverMode() {
        isFeverModeActive = false;
        isFeverModeUsed = true;

        feverGauge = 0f;
        Render();

        feverEventChannel.SendOnFeverFinished(0);
    }


    [ButtonGroup]
    public void ActivateFeverMode() {
        // If fever mode is currently being used, or it already has been used, stop here
        if (isFeverModeActive || isFeverModeUsed) {
            return;
        }

        // If we haven't gotten fever mode yet, stop here
        if (!isFeverModeAvailable) return;

        isFeverModeActive = true;
        isFeverModeUsed = true;
        StartCoroutine(DisableFeverModeAfterDuration());

        feverEventChannel.SendOnFeverActivated(0);
    }

    private IEnumerator DisableFeverModeAfterDuration() {
        elapsedFeverDuration = 0f;
        while (elapsedFeverDuration < difficultySettings.feverModeDuration) {
            if (GameManager.Instance.gameState == GAME_STATE.GAME_WON || GameManager.Instance.gameState == GAME_STATE.GAME_LOST || GameManager.Instance.gameState == GAME_STATE.MAIN_MENU) {
                // Might be cleaner/faster to cache the coroutine, and destroy this on event
                break;
            }

            if (GameManager.Instance.gameState == GAME_STATE.GAME_ACTIVE) {
                elapsedFeverDuration += Time.deltaTime;
                Render();
                yield return null;
            }
        }

        FinishFeverMode();
    }
}
