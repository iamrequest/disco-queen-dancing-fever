using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SongMenuInputListener : MonoBehaviour {
    public InputEventChannel inputEventChannel;
    public bool isSongSelected;
    public AudioClip sfxMenuMove, sfxMenuBack, sfxSongSelect, sfxDifficultySelect;

    private void OnEnable() {
        inputEventChannel.onInput += OnInput;
    }
    private void OnDisable() {
        inputEventChannel.onInput -= OnInput;
    }

    private void OnInput(INPUT_DIRS inputDir, int playerIndex) {
        // Only process menu inputs when we're at the main menu
        if (GameManager.Instance.gameState != GAME_STATE.MAIN_MENU) return;

        // Don't process floor input for the song list menu if there's no songs to choose from
        if (SongDetailsMenu.Instance.songsMetadata.Count == 0) {
            return;
        }

        if (!isSongSelected) {
            // Song menu is active
            switch (inputDir) {
                case INPUT_DIRS.UP:
                    SongDetailsMenu.Instance.PrevSong();
                    SFXManager.Instance.PlaySFX(sfxMenuMove, transform.position);
                    break;

                case INPUT_DIRS.DOWN:
                    SongDetailsMenu.Instance.NextSong();
                    SFXManager.Instance.PlaySFX(sfxMenuMove, transform.position);
                    break;

                case INPUT_DIRS.RIGHT:
                    isSongSelected = true;
                    SongDifficultyMenu.Instance.SetHighlightSelectedMenuItem(true);
                    SFXManager.Instance.PlaySFX(sfxSongSelect, transform.position);
                    break;
            }
        } else {
            // Song difficulty menu is active
            switch (inputDir) {
                case INPUT_DIRS.UP:
                    SongDifficultyMenu.Instance.PrevSong();
                    SFXManager.Instance.PlaySFX(sfxMenuMove, transform.position);
                    break;

                case INPUT_DIRS.DOWN:
                    SongDifficultyMenu.Instance.NextSong();
                    SFXManager.Instance.PlaySFX(sfxMenuMove, transform.position);
                    break;

                case INPUT_DIRS.RIGHT:
                    isSongSelected = false;
                    SongDifficultyMenu.Instance.SetHighlightSelectedMenuItem(false);
                    SongDifficultyMenu.Instance.StartSelected();
                    SFXManager.Instance.PlaySFX(sfxDifficultySelect, transform.position);
                    break;

                case INPUT_DIRS.LEFT:
                    isSongSelected = false;
                    SongDifficultyMenu.Instance.SetHighlightSelectedMenuItem(false);
                    SFXManager.Instance.PlaySFX(sfxMenuBack, transform.position);
                    break;
            }
        }
    }
}
