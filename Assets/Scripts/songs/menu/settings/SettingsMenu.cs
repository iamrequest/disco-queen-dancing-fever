using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsMenu : MonoBehaviour {
    [Tooltip("Is the game played with foot trackers, or controllers?")]
    public GazeMenuButton buttonInputMethodTrackers, buttonInputMethodControllers;

    [Tooltip("Init floor height calibration")]
    public GazeMenuButton buttonCallibrateStart;

    private void Awake() {
        buttonInputMethodControllers.onStateChanged.AddListener(SetInputMethodControllers);
        buttonInputMethodTrackers.onStateChanged.AddListener(SetInputMethodTrackers);
        buttonCallibrateStart.onStateChanged.AddListener(StartCallibration);
    }


    private void StartCallibration(bool state) {
    }

    private void SetInputMethodTrackers(bool state) {
    }
    private void SetInputMethodControllers(bool state) {
    }
}
