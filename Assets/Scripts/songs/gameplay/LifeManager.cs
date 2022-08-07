using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LifeManager : MonoBehaviour
{
    public GameStateEventChannel gameStateEventChannel;
    public NoteHitEventChannel noteHitEventChannel;
    public DifficultySettings difficultySettings;

    public Slider lifeSlider;
    public TextMeshProUGUI uiText;

    public int currentHealth;

    private void OnEnable() {
        gameStateEventChannel.onGameStateChange += OnGameStateChange;
        noteHitEventChannel.onNoteHit += OnNoteHit;
        noteHitEventChannel.onNoteMiss += OnNoteMiss;
    }
    private void OnDisable() {
        gameStateEventChannel.onGameStateChange -= OnGameStateChange;
        noteHitEventChannel.onNoteHit -= OnNoteHit;
        noteHitEventChannel.onNoteMiss -= OnNoteMiss;
    }

    // --------------------------------------------------------------------------------
    // Events
    // --------------------------------------------------------------------------------
    private void OnGameStateChange(GAME_STATE oldGameState, GAME_STATE newGameState) {
        if (newGameState == GAME_STATE.GAME_ACTIVE && oldGameState != GAME_STATE.GAME_PAUSED) {
            ResetHealth();
        }
    }

    private void OnNoteHit(NOTE_BOARD_LANES lane, NOTE_HIT_TYPES noteHitType, int playerIndex) {
        if (GameManager.Instance.gameState != GAME_STATE.GAME_ACTIVE) return;

        switch (noteHitType) {
            case NOTE_HIT_TYPES.PERFECT:
                AdjustHealth(difficultySettings.healthRegenPerfect);
                Render();
                break;
            case NOTE_HIT_TYPES.GREAT:
                AdjustHealth(difficultySettings.healthRegenGreat);
                Render();
                break;
            case NOTE_HIT_TYPES.GOOD:
                AdjustHealth(difficultySettings.healthRegenGood);
                Render();
                break;
        }

    }

    private void OnNoteMiss(NOTE_BOARD_LANES lane, int playerIndex) {
        if (GameManager.Instance.gameState != GAME_STATE.GAME_ACTIVE) return;

        AdjustHealth(-difficultySettings.damageNoteMiss);
        Render();

        if (!CheatMenu.Instance.noFailMode) {
            if (currentHealth <= 0) {
                OnDeath();
            }
        }
    }

    [ButtonGroup]
    private void ResetHealth() {
        currentHealth = difficultySettings.healthMax;
        Render();
    }

    private void AdjustHealth(int delta) {
        currentHealth = Mathf.Clamp(currentHealth + delta, 0 , difficultySettings.healthMax);
    }

    private void OnDeath() {
        gameStateEventChannel.SendOnRequestGameStateChange(GAME_STATE.GAME_LOST);
    }

    [ButtonGroup]
    private void Render() {
        lifeSlider.value = (float)currentHealth / (float)difficultySettings.healthMax;

        uiText.lineSpacing = 100;
        uiText.text = "LIFE";

        if (!CheatMenu.Instance.noFailMode && currentHealth <= 0) {
            uiText.lineSpacing = 50;
            uiText.text = "DEAD!";
        }
    }
}
