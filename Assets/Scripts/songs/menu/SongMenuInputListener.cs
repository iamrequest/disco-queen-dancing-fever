using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SongMenuInputListener : MonoBehaviour {
    public InputEventChannel inputEventChannel;
    public bool isSongSelected;

    private void OnEnable() {
        inputEventChannel.onInput += OnInput;
    }
    private void OnDisable() {
        inputEventChannel.onInput -= OnInput;
    }

    private void OnInput(INPUT_DIRS inputDir, int playerIndex) {
        if (!isSongSelected) {
            // Song menu is active
            switch (inputDir) {
                case INPUT_DIRS.UP:
                    SongDetailsMenu.Instance.PrevSong();
                    break;
                case INPUT_DIRS.DOWN:
                    SongDetailsMenu.Instance.NextSong();
                    break;
                case INPUT_DIRS.RIGHT:
                    isSongSelected = true;
                    SongDifficultyMenu.Instance.SetHighlightSelectedMenuItem(true);
                    break;
            }
        } else {
            // Song difficulty menu is active
            switch (inputDir) {
                case INPUT_DIRS.UP:
                    SongDifficultyMenu.Instance.PrevSong();
                    break;
                case INPUT_DIRS.DOWN:
                    SongDifficultyMenu.Instance.NextSong();
                    break;
                case INPUT_DIRS.RIGHT:
                    isSongSelected = false;
                    SongDifficultyMenu.Instance.SetHighlightSelectedMenuItem(false);
                    SongDifficultyMenu.Instance.StartSelected();
                    break;
                case INPUT_DIRS.LEFT:
                    isSongSelected = false;
                    SongDifficultyMenu.Instance.SetHighlightSelectedMenuItem(false);
                    break;
            }
        }
    }
}