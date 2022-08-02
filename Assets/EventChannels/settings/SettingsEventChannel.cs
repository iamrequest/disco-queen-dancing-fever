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

    public void SendInputTypeChanged(INPUT_TYPES inputType, int playerIndex) {
        if (onInputTypeChanged != null) {
            onInputTypeChanged.Invoke(inputType, playerIndex);
        }
    }
}
