using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class GameManager : MonoBehaviour {
    public static GameManager Instance { get; private set; }
    public GameStateEventChannel gameStateEventChannel;
    public GAME_STATE gameState { get; private set; }

    private void Awake() {
        if (Instance == null) {
            Instance = this;
        } else {
            Debug.LogError($"Multiple {GetType()} components detected. This is probably a bug.");
            Destroy(this);
        }
    }

    private void OnEnable() {
        gameStateEventChannel.onRequestGameStateChange += RequestGameStateChange;
    }
    private void OnDisable() {
        gameStateEventChannel.onRequestGameStateChange -= RequestGameStateChange;
    }

    private void RequestGameStateChange(GAME_STATE newGameState) {
        // Can't transition to a "None" state.
        // This is only an option, because I made the enum a flag type (system.flags)
        if (newGameState == 0) {
            Debug.LogWarning($"Attempted to switch to a 'None' game type. This shouldn't happen - did you forget to init a parameter?");
            return;
        }

        if (IsMultipleGameStatesSelected(newGameState)) {
            Debug.LogWarning($"Attempted to switch to a multiple game types at the same time ({newGameState}). This shouldn't happen.");
            return;
        }

        gameStateEventChannel.SendOnGameStateChange(gameState, newGameState);
        gameState = newGameState; // TODO: Confirm things don't break by setting this after the event call
    }

    // More System.Flags stuff. Test if multiple flags are selected.
    private bool IsMultipleGameStatesSelected(GAME_STATE gameState) {
        // Test if the enum is a power of 2. If so, it's only 1 enum value was selected (good)
        // Source: https://forum.unity.com/threads/how-to-check-how-many-flags-set-in-a-bitmask-enum.520772/#post-3416510
        return !( (gameState != 0) && ((gameState & (gameState - 1)) == 0));
    }
}
