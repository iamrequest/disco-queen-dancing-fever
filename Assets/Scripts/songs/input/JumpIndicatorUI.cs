using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpIndicatorUI : MonoBehaviour {
    public SettingsEventChannel settingsEventChannel;

    private void Awake() {
        UpdatePosition();
    }

    private void OnEnable() {
        settingsEventChannel.onCalibrateFloorHeight += OnCalibrateFloorHeight;
    }
    private void OnDisable() {
        settingsEventChannel.onCalibrateFloorHeight -= OnCalibrateFloorHeight;
    }

    private void OnCalibrateFloorHeight(float newFloorHeight) {
        UpdatePosition();
    }

    private void UpdatePosition() {
        transform.localPosition = new Vector3(0f, settingsEventChannel.jumpMinHeight, 0f);
    }
}
