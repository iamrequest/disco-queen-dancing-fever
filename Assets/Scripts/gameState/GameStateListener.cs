using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class GameStateListener : MonoBehaviour {
    public GameStateEventChannel gameStateEventChannel;
    public Animator animator;
    private const string isHiddenParam = "isHidden";
    private int isHiddenParamHash;

    [Tooltip("Play the show animation on transition to these game states")]
    [EnumToggleButtons]
    public GAME_STATE showOnState;

    [Tooltip("Play the hide animation on transition to these game states")]
    [EnumToggleButtons]
    public GAME_STATE hideOnState;

    private void Awake() {
        isHiddenParamHash = Animator.StringToHash(isHiddenParam);
    }

    private void OnEnable() {
        gameStateEventChannel.onGameStateChange += OnGameStateChange;
    }
    private void OnDisable() {
        gameStateEventChannel.onGameStateChange -= OnGameStateChange;
    }

    private void OnGameStateChange(GAME_STATE newGameState) {
        Debug.Log($"Game state change: {newGameState}");

        if ((newGameState & showOnState) > 0) {
            Debug.Log("Show");
            animator.SetBool(isHiddenParamHash, false);
        } else if ((newGameState & hideOnState) > 0) {
            Debug.Log("Hide");
            animator.SetBool(isHiddenParamHash, true);
        }
    }
}
