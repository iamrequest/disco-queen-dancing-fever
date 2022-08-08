using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Being lazy and just making a single sfx player
public class SFXFeverMode : MonoBehaviour {
    [Title("Event Channels")]
    public FeverEventChannel feverEventChannel;

    [Title("SFX")]
    public AudioClip sfxFeverReady;
    [Tooltip("Play all of these")]
    public List<AudioClip> sfxFeverActivated; // This should probably be a separate var for each sfx I wanna play at the same time

    private void OnEnable() {
        feverEventChannel.onFeverReady += OnFeverReady;
        feverEventChannel.onFeverActivated += OnFeverActivated;
    }

    private void OnDisable() {
        feverEventChannel.onFeverReady -= OnFeverReady;
        feverEventChannel.onFeverActivated -= OnFeverActivated;
    }

    private void OnFeverReady(int playerIndex) {
        if (sfxFeverReady) SFXManager.Instance.PlaySFX(sfxFeverReady, Vector3.zero);
    }

    private void OnFeverActivated(int playerIndex) {
        if (sfxFeverActivated.Count != 0) {
            foreach (AudioClip ac in sfxFeverActivated) {
                SFXManager.Instance.PlaySFX(ac, Vector3.zero);
            }
        }
    }
}
