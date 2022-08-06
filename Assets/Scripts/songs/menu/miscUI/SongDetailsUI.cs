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
            case GAME_STATE.GAME_WON:
            case GAME_STATE.GAME_LOST:
                Render();
                break;
        }
    }

    private void Render() {
        if (SongPlayer.Instance != null) {
            if (SongPlayer.Instance.currentSong != null) {
                songNameValueText.text = SongPlayer.Instance.currentSong.songName;
                artistNameValueText.text = SongPlayer.Instance.currentSong.artistName;
                return;
            }
        }

        // No song loaded, this is probably an in-editor test
        songNameValueText.text = "";
        artistNameValueText.text = "";
    }
}
