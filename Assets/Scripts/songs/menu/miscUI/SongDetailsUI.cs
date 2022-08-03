using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SongDetailsUI : MonoBehaviour {
    public GameStateEventChannel gameStateEventChannel;
    public TextMeshProUGUI songNameValueText;
    public TextMeshProUGUI artistNameValueText;

    private void OnEnable() {
        gameStateEventChannel.onGameStateChange += OnGameStateChanged;

    }

    private void OnDisable() {
        gameStateEventChannel.onGameStateChange -= OnGameStateChanged;
    }

    private void OnGameStateChanged(GAME_STATE oldState, GAME_STATE newState) {
        switch (newState) {
            case GAME_STATE.GAME_ACTIVE:
            case GAME_STATE.GAME_OVER:
                Render();
                break;
        }
    }

    private void Render() {
        if (SongPlayer.Instance != null) {
            songNameValueText.text = SongPlayer.Instance.currentSong.songName;
            artistNameValueText.text = SongPlayer.Instance.currentSong.artistName;
        } else {
            songNameValueText.text = "";
            artistNameValueText.text = "";
        }
    }
}
