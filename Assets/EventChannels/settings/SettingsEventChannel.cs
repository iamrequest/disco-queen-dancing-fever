using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// https://www.youtube.com/watch?v=WLDgtRNK2VE
/// </summary>
[CreateAssetMenu(menuName = "Events/Settings Event Channel")]
public class SettingsEventChannel : ScriptableObject {
    public UnityAction<INPUT_TYPES, int> onInputTypeChanged;

    public UnityAction startCalibrateFloorHeight; // Request a calibration
    public UnityAction<float> onCalibrateFloorHeight; // On calibration complete

    // This should probably be in a separate Settings scriptable object
    [Tooltip("Minimum height from the floor required to initiate a jump input")]
    public float jumpMinHeight;

    public void SendInputTypeChanged(INPUT_TYPES inputType, int playerIndex) {
        if (onInputTypeChanged != null) {
            onInputTypeChanged.Invoke(inputType, playerIndex);
        }
    }

    public void SendStartCalibrateFloorHeight() {
        if (startCalibrateFloorHeight != null) {
            startCalibrateFloorHeight.Invoke();
        }
    }
    public void SendOnCalibrateFloorHeight(float newFloorHeight) {
        if (onCalibrateFloorHeight != null) {
            onCalibrateFloorHeight.Invoke(newFloorHeight);
        }
    }
}
