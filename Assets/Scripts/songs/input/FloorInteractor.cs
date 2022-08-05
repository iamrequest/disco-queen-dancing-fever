using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum INPUT_DIRS { LEFT=0, UP = 1, DOWN=2, RIGHT=3}
public class FloorInteractor : MonoBehaviour {
    // Future proofing player input in the event that multiple floor interactors are a thing. Remove const when necessary
    [HideInInspector]
    public const int playerIndex = 0;

    public SettingsEventChannel settingsEventChannel;
    public InputEventChannel inputEventChannel;

    // These settings filter out multiple presses on the floor. Might be necessary to remove/reduce the cooldown during gameplay, but it's necessary for menu input
    public LayerMask buttonLayerMask;
    public float inputCooldown;

    private void OnEnable() {
        settingsEventChannel.onCalibrateFloorHeight += UpdateFloorHeight;
    }
    private void OnDisable() {
        settingsEventChannel.onCalibrateFloorHeight -= UpdateFloorHeight;
    }

    private void UpdateFloorHeight(float newFloorHeight) {
        transform.position = Vector3.zero + Vector3.up * newFloorHeight;
    }


    [Button]
    public void SendInput(INPUT_DIRS input) {
        inputEventChannel.SendPlayerInput(input, playerIndex);
    }


    // --------------------------------------------------------------------------------
    // Editor Debug
    // --------------------------------------------------------------------------------
    [ButtonGroup("Debug Input")] [Button("<")]
    private void SendInputLeft() { SendInput(INPUT_DIRS.LEFT); }
    [ButtonGroup("Debug Input")] [Button("^")]
    private void SendInputUp() { SendInput(INPUT_DIRS.UP); }
    [ButtonGroup("Debug Input")] [Button("v")]
    private void SendInputDown() { SendInput(INPUT_DIRS.DOWN); }
    [ButtonGroup("Debug Input")] [Button(">")]
    private void SendInputRight() { SendInput(INPUT_DIRS.RIGHT); }
}
