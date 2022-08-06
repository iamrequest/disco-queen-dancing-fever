using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerInputMethodManager))]
public class PlayerJumpInputManager : MonoBehaviour {
    private PlayerInputMethodManager playerInputMethodManager;
    private float floorBaseHeight;

    public SettingsEventChannel settingsEventChannel;
    public InputEventChannel inputEventChannel;

    private bool isCurrentlyJumping;

    private void Awake() {
        playerInputMethodManager = GetComponent<PlayerInputMethodManager>();
    }

    private void OnEnable() {
        settingsEventChannel.onCalibrateFloorHeight += UpdateCachedFloorHeight;
    }
    private void OnDisable() {
        settingsEventChannel.onCalibrateFloorHeight -= UpdateCachedFloorHeight;
    }

    private void Update() {
        if (GameManager.Instance.gameState == GAME_STATE.GAME_ACTIVE) {
            TestJumping();
        }
    }

    private void UpdateCachedFloorHeight(float newFloorHeight) {
        floorBaseHeight = newFloorHeight;
    }

    public bool TestJumping() {
        bool isJumping = playerInputMethodManager.footLeft.position.y >= floorBaseHeight + settingsEventChannel.jumpMinHeight
            && playerInputMethodManager.footRight.position.y >= floorBaseHeight + settingsEventChannel.jumpMinHeight;

        if (isJumping && !isCurrentlyJumping) {
            // playerInputMethodManager.playerIndex = 0;
            inputEventChannel.SendOnJumpStarted(0);
        }

        isCurrentlyJumping = isJumping;
        return isJumping;
    }
}
