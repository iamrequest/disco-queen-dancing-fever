using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheatMenu : GazableMenu {
    public static CheatMenu Instance { get; private set; }

    public GazeMenuButton btnNoFailMode;

    private void Awake() {
        if (Instance == null) {
            Instance = this;
        } else {
            Debug.LogError($"Multiple {GetType()} components detected. This is probably a bug.");
            Destroy(this);
        }
    }

    public bool noFailMode;

    protected override void OnEnable() {
        base.OnEnable();

        btnNoFailMode.onStateChanged.AddListener(SetNoFailMode);
    }

    protected override void OnDisable() {
        base.OnDisable();

        btnNoFailMode.onStateChanged.RemoveListener(SetNoFailMode);
    }


    // --------------------------------------------------------------------------------
    // Gaze Button Functionality
    // --------------------------------------------------------------------------------
    private void SetNoFailMode(bool state) {
        noFailMode = state;
    }
}
