using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class ScoreUI : MonoBehaviour {
    public ScoreEventChannel scoreEventChannel;
    public TextMeshProUGUI scoreValueText;

    private void OnEnable() {
        scoreEventChannel.onScoreChanged += OnScoreChanged;
        scoreEventChannel.onScoreReset += OnScoreReset;

        if (ScoreManager.Instance != null) {
            Render(ScoreManager.Instance.score);
        } else {
            Render(0);
        }
    }

    private void OnDisable() {
        scoreEventChannel.onScoreChanged -= OnScoreChanged;
        scoreEventChannel.onScoreReset -= OnScoreReset;
    }

    private void OnScoreReset() {
        Render(0);
    }

    private void OnScoreChanged(int scoreChange, int newScore) {
        Render(newScore);
    }

    private void Render(int value) {
        scoreValueText.text = value.ToString("000 000 000 000");
    }
}
