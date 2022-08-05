using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Freya;

// Source: https://github.com/iamrequest/shattered-skies/blob/main/Assets/Scripts/BGMManager.cs
[RequireComponent(typeof(AudioSource))]
public class BGMManager : MonoBehaviour {
    private AudioSource audioSource;

    public GameStateEventChannel gameStateEventChannel;
    public BGMSoundtrack soundtrack;

    [ShowInInspector]
    [Range(0f, 2f)]
    private float baseVolume;

    [Range(0f, 3f)]
    public float fadeDuration;
    private Coroutine fadeCoroutine;

    private void Awake() {
        audioSource = GetComponent<AudioSource>();
        baseVolume = audioSource.volume;

        PlaySong(soundtrack.bgmTitle, soundtrack.volumeTitle);
    }

    private void OnEnable() {
        gameStateEventChannel.onGameStateChange += OnGameStateChange;
    }
    private void OnDisable() {
        gameStateEventChannel.onGameStateChange -= OnGameStateChange;
    }


    private void OnGameStateChange(GAME_STATE oldGameState, GAME_STATE newGameState) {
        switch (newGameState) {
            case GAME_STATE.TITLE:
                PlaySong(soundtrack.bgmTitle, soundtrack.volumeTitle);
                break;

            case GAME_STATE.MAIN_MENU:
                FadeToStopThenPlay(soundtrack.bgmMenu, soundtrack.volumeMenu);
                break;

            case GAME_STATE.GAME_ACTIVE:
                FadeToStop();
                break;

            case GAME_STATE.GAME_OVER:
                Stop();
                FadeToStopThenPlay(soundtrack.bgmGameOver, soundtrack.volumeGameOver);
                break;
        }
    }



    public void Stop() {
        if (fadeCoroutine != null) StopCoroutine(fadeCoroutine);
        audioSource.Stop();
    }
    public void FadeToStop() {
        if (fadeCoroutine != null) StopCoroutine(fadeCoroutine);
        StartCoroutine(DoFadeToStop());
    }
    public void FadeToStopThenPlay(AudioClip clip, float volume) {
        if (fadeCoroutine != null) StopCoroutine(fadeCoroutine);
        StartCoroutine(DoFadeToStopThenPlay(clip, volume));
    }

    private IEnumerator DoFadeToStop() {
        float t = 0f;
        float initialVolume = audioSource.volume;

        while (t < fadeDuration) {
            t += Time.deltaTime;
            audioSource.volume = Mathfs.LerpClamped(initialVolume, 0f, t / fadeDuration);
            yield return null;
        }

        audioSource.Stop();
    }

    private IEnumerator DoFadeToStopThenPlay(AudioClip clip, float volume) {
        float t = 0f;
        float initialVolume = audioSource.volume;

        while (t < fadeDuration) {
            t += Time.deltaTime;
            audioSource.volume = Mathfs.LerpClamped(initialVolume, 0f, t / fadeDuration);
            yield return null;
        }

        PlaySong(clip, volume);
    }


    public void PlaySong(AudioClip clip, float volume) {
        if (fadeCoroutine != null) StopCoroutine(fadeCoroutine);

        audioSource.clip = clip;
        audioSource.volume = baseVolume * volume;

        audioSource.Play();
    }
}
