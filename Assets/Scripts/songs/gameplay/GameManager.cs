using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GAME_STATE { TITLE, MAIN_MENU, GAME_ACTIVE, GAME_PAUSED, GAME_OVER } 
public class GameManager : MonoBehaviour {
    public static GameManager Instance { get; private set; }
    private void Awake() {
        if (Instance == null) {
            Instance = this;
        } else {
            Debug.LogError($"Multiple {GetType()} components detected. This is probably a bug.");
            Destroy(this);
        }
    }

    public GAME_STATE gameState;
}
