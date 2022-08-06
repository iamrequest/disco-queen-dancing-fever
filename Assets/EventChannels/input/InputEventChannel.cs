using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// https://www.youtube.com/watch?v=WLDgtRNK2VE
/// </summary>
[CreateAssetMenu(menuName = "Events/Input Event Channel")]
public class InputEventChannel : ScriptableObject {
    public UnityAction<INPUT_DIRS, int> onInput;

    [Tooltip("First frame of the player jumping")]
    public UnityAction<int> onJumpStarted;

    [Button]
    public void SendPlayerInput(INPUT_DIRS inputDir, int playerIndex) {
        if (onInput != null) {
            onInput.Invoke(inputDir, playerIndex);
        }
    }

    [Button]
    public void SendOnJumpStarted(int playerIndex) {
        if (onJumpStarted != null) {
            onJumpStarted.Invoke(playerIndex);
        }
    }
}
