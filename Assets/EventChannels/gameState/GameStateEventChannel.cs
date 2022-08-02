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
    public UnityAction<GAME_STATE> onGameStateChange;

    [Button]
    public void SendOnRequestGameStateChange(GAME_STATE newGameState) {
        if (onRequestGameStateChange != null) {
            onRequestGameStateChange.Invoke(newGameState);
        }
    }

    public void SendOnGameStateChange(GAME_STATE newGameState) {
        if (onGameStateChange != null) {
            onGameStateChange.Invoke(newGameState);
        }
    }
}
