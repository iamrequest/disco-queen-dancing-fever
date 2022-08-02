using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Simple interface to update game state, via unityevents. Specificially, unity events from GameMenuButtons.
/// It seems like Unity doesn't let you pick a function from the UnityEvent dropdown, if it has a custom enum as a parameter? 
/// Either that, or it's because this specific enum has a System.Flag property attribute
/// </summary>
public class GameStateChanger : MonoBehaviour {
    public GameStateEventChannel gameStateEventChannel;
    public GAME_STATE targetGameState;

    public void RequestGameStateChange() {
        gameStateEventChannel.SendOnRequestGameStateChange(targetGameState);
    }
}
