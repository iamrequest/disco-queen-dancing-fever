using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsMenu : GazableMenu {
    public static SettingsMenu Instance { get; private set; }

    public SettingsEventChannel settingsEventChannel;

    [Tooltip("Is the game played with foot trackers, or controllers?")]
    public GazeMenuButton buttonInputMethodTrackers, buttonInputMethodControllers;

    [Tooltip("Init floor height calibration")]
    public GazeMenuButton buttonCallibrateStart;

    private void Awake() {
        if (Instance == null) {
            Instance = this;
        } else {
            Debug.LogError($"Multiple {GetType()} components detected. This is probably a bug.");
            Destroy(this);
        }
    }

    protected override void OnEnable() {
        base.OnEnable();

        buttonInputMethodControllers.onStateChanged.AddListener(SetInputMethodControllers);
        buttonInputMethodTrackers.onStateChanged.AddListener(SetInputMethodTrackers);
        buttonCallibrateStart.onStateChanged.AddListener(StartCallibration);
    }

    protected override void OnDisable() {
        base.OnDisable();

        buttonInputMethodControllers.onStateChanged.RemoveListener(SetInputMethodControllers);
        buttonInputMethodTrackers.onStateChanged.RemoveListener(SetInputMethodTrackers);
        buttonCallibrateStart.onStateChanged.RemoveListener(StartCallibration);
    }

    // --------------------------------------------------------------------------------
    // Gaze Button Functionality
    // --------------------------------------------------------------------------------
    private void StartCallibration(bool state) {
        settingsEventChannel.SendStartCalibrateFloorHeight();
    }

    private void SetInputMethodTrackers(bool state) {
        buttonInputMethodControllers.SetSelectedSilent(false, true);
        settingsEventChannel.SendInputTypeChanged(INPUT_TYPES.FEET_TRACKER, 0);
    }

    private void SetInputMethodControllers(bool state) {
        buttonInputMethodTrackers.SetSelectedSilent(false, true);
        settingsEventChannel.SendInputTypeChanged(INPUT_TYPES.CONTROLLER, 0);
    }

}
