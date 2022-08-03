using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// https://www.youtube.com/watch?v=WLDgtRNK2VE
/// </summary>
[CreateAssetMenu(menuName = "Events/Score Event Channel")]
public class ScoreEventChannel : ScriptableObject {
    [Tooltip("Ask the score manager to increase the score")]
    public UnityAction<int> onRequestScoreChange;

    [Tooltip("The score manager has just increased the score")]
    public UnityAction<int, int> onScoreChanged;

    public UnityAction onScoreReset;
    public UnityAction onRequestScoreReset;

    public void SendOnRequestScoreChanged(int scoreChange) {
        if (onRequestScoreChange != null) {
            onRequestScoreChange.Invoke(scoreChange);
        }
    }

    public void SendOnScoreChanged(int scoreChange, int newScore) {
        if (onScoreChanged != null) {
            onScoreChanged.Invoke(scoreChange, newScore);
        }
    }


    public void SendOnRequestScoreReset() {
        if (onRequestScoreReset != null) {
            onRequestScoreReset.Invoke();
        }
    }
    public void SendOnScoreReset() {
        if (onScoreReset != null) {
            onScoreReset.Invoke();
        }
    }
}
