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
    public UnityAction<int> onJump;

    public void SendPlayerInput(INPUT_DIRS inputDir, int playerIndex) {
        if (onInput != null) {
            onInput.Invoke(inputDir, playerIndex);
        }
    }

    public void SendOnJumpStarted(int playerIndex) {
    }
}
