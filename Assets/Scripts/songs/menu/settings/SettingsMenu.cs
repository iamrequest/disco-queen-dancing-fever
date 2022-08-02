using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour {
    public static SettingsMenu Instance { get; private set; }

    public SettingsEventChannel settingsEventChannel;

    [Tooltip("Is the game played with foot trackers, or controllers?")]
    public GazeMenuButton buttonInputMethodTrackers, buttonInputMethodControllers;

    [Tooltip("Init floor height calibration")]
    public GazeMenuButton buttonCallibrateStart;

    // Render an icon where the user is looking. Gradually make it fade after not looking for a while
    [Header("Gaze Icon")]
    public Image gazeIcon;
    private float gazeIconElapsedDuration; // Visibility duration
    public float gazeIconDuration;
    public float gazeIconLerpSpeed;
    private Vector3 gazeLastSeenPosition;
    public AnimationCurve gazeIconVisibilityCurve;

    private void Awake() {
        if (Instance == null) {
            Instance = this;
        } else {
            Debug.LogError($"Multiple {GetType()} components detected. This is probably a bug.");
            Destroy(this);
        }
    }
    private void OnEnable() {
        // Hide the gaze icon on menu menu spawn
        gazeIconElapsedDuration = gazeIconDuration;

        buttonInputMethodControllers.onStateChanged.AddListener(SetInputMethodControllers);
        buttonInputMethodTrackers.onStateChanged.AddListener(SetInputMethodTrackers);
        buttonCallibrateStart.onStateChanged.AddListener(StartCallibration);
    }

    private void OnDisable() {
        buttonInputMethodControllers.onStateChanged.RemoveListener(SetInputMethodControllers);
        buttonInputMethodTrackers.onStateChanged.RemoveListener(SetInputMethodTrackers);
        buttonCallibrateStart.onStateChanged.RemoveListener(StartCallibration);
    }

    private void Update() {
        UpdateGazeIcon();
    }

    // --------------------------------------------------------------------------------
    // Gaze UI Icon
    // --------------------------------------------------------------------------------
    public void OnGaze(Vector3 worldPosition) {
        gazeLastSeenPosition = worldPosition;

        // If the gaze icon is currently hidden, snap it to the current position
        if (gazeIconElapsedDuration >= gazeIconDuration) {
            gazeIcon.transform.position = gazeLastSeenPosition;
        } else {
            gazeIcon.transform.position = Vector3.Lerp(gazeIcon.transform.position, gazeLastSeenPosition, gazeIconLerpSpeed);
        }

        gazeIconElapsedDuration = 0f;
    }

    private void UpdateGazeIcon() {
        gazeIconElapsedDuration += Time.deltaTime;

        // Lerp icon alpha to 0
        Color c = gazeIcon.color;
        c.a = gazeIconVisibilityCurve.Evaluate(gazeIconElapsedDuration / gazeIconDuration);
        gazeIcon.color = c;
    }

    // --------------------------------------------------------------------------------
    // Gaze Button Functionality
    // --------------------------------------------------------------------------------
    private void StartCallibration(bool state) {
        buttonCallibrateStart.SetSelectedSilent(false, true);
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
