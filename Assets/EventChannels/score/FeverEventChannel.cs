using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// https://www.youtube.com/watch?v=WLDgtRNK2VE
/// </summary>
[CreateAssetMenu(menuName = "Events/Fever Event Channel")]
public class FeverEventChannel : ScriptableObject {
    public UnityAction<int> onFeverReady;
    public UnityAction<int> onFeverActivated;
    public UnityAction<int> onFeverFinished;


    [ButtonGroup]
    public void SendOnFeverReady(int playerIndex) {
        if (onFeverReady != null) {
            onFeverReady.Invoke(playerIndex);
        }
    }

    [ButtonGroup]
    public void SendOnFeverActivated(int playerIndex) {
        if (onFeverActivated != null) {
            onFeverActivated.Invoke(playerIndex);
        }
    }

    [ButtonGroup]
    public void SendOnFeverFinished(int playerIndex) {
        if (onFeverFinished != null) {
            onFeverFinished.Invoke(playerIndex);
        }
    }
}
