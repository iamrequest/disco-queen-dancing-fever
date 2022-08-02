using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputMethodManager : MonoBehaviour {
    public SettingsEventChannel settingsEventChannel;
    public INPUT_TYPES inputType;

    //private const int playerIndex = 0;

    [Header("Feet models")]
    [Tooltip("Feet models, used for input")]
    public Transform footLeft;
    public Transform footRight;

    [Header("Source controller transforms")]
    public Transform controllerLeft;
    public Transform controllerRight;
    public Transform footTrackerLeft, footTrackerRight;

    // TODO: Add offset such that the feet mesh face the floor on calibrate
    //public Quaternion leftRotOffset, rightRotOffset;

    private void OnEnable() {
        settingsEventChannel.onInputTypeChanged += UpdateInputType;
    }

    private void OnDisable () {
        settingsEventChannel.onInputTypeChanged += UpdateInputType;
    }

    private void Update() {
        UpdateFeetTransform();
    }

    private void UpdateFeetTransform() {
        switch (inputType) {
            case INPUT_TYPES.FEET_TRACKER:
                UpdateFoot(footTrackerLeft, footLeft, true);
                UpdateFoot(footTrackerRight, footRight, false);
                break;
            case INPUT_TYPES.CONTROLLER:
                UpdateFoot(controllerLeft, footLeft, true);
                UpdateFoot(controllerRight, footRight, false);
                break;
            default:
                Debug.LogError($"Unexpected input type [{inputType}]. Cannot update foot transform.");
                break;
        }
    }

    private void UpdateFoot(Transform src, Transform dst, bool isLeftFoot) {
        dst.position = src.position;

        //if (isLeftFoot) {
        //    dst.rotation = src.rotation * leftRotOffset;
        //} else {
        //    dst.rotation = src.rotation * rightRotOffset;
        //}
        dst.rotation = src.rotation;
    }

    private void UpdateInputType(INPUT_TYPES inputType, int playerIndex) {
        // if(playerIndex = this.playerIndex
        this.inputType = inputType;
    }
}
