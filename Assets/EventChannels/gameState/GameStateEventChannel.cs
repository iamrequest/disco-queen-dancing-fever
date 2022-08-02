using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// https://www.youtube.com/watch?v=WLDgtRNK2VE
/// </summary>
[CreateAssetMenu(menuName = "Events/Game State Event Channel")]
public class GameStateEventChannel : ScriptableObject { 
    [Tooltip("Ask the game manager to change the game state")]
    public UnityAction<GAME_STATE> onRequestGameStateChange;

    [Tooltip("The game manager has just changed the game state")]
    public UnityAction<GAME_STATE, GAME_STATE> onGameStateChange;

    [Button]
    public void SendOnRequestGameStateChange(GAME_STATE newGameState) {
        if (onRequestGameStateChange != null) {
            onRequestGameStateChange.Invoke(newGameState);
        }
    }

    public void SendOnGameStateChange(GAME_STATE oldGameState, GAME_STATE newGameState) {
        if (onGameStateChange != null) {
            onGameStateChange.Invoke(oldGameState, newGameState);
        }
    }

    // --------------------------------------------------------------------------------
    // Editor
    // --------------------------------------------------------------------------------
    [Title("Set Game State")]
    [Button("Title")]
    private void RequestGameStateTitle() { SendOnRequestGameStateChange(GAME_STATE.TITLE); }

    [Button("Main Menu")]
    private void RequestGameStateMenu() { SendOnRequestGameStateChange(GAME_STATE.MAIN_MENU); }

    [Button("Game Active")]
    [ButtonGroup("StateChange Game")]
    private void RequestGameStateGameActive() { SendOnRequestGameStateChange(GAME_STATE.GAME_ACTIVE); }

    [Button("Game Paused")]
    [ButtonGroup("StateChange Game")]
    private void RequestGameStatePaused() { SendOnRequestGameStateChange(GAME_STATE.GAME_PAUSED); }

    [Button("Game Over")]
    [ButtonGroup("StateChange Game")]
    private void RequestGameStateGameOver() { SendOnRequestGameStateChange(GAME_STATE.GAME_OVER); }
}
