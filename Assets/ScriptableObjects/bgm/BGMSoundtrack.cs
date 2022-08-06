using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/BGM Soundtrack")]
public class BGMSoundtrack : ScriptableObject {
    [Header("BGM")]
    public AudioClip bgmTitle;
    public AudioClip bgmMenu;
    public AudioClip bgmGameOver;

    [Header("SFX")]
    public AudioClip sfxOnGameWon;

    [Header("Volume")]
    [Range(0f, 2f)] public float volumeTitle;
    [Range(0f, 2f)] public float volumeMenu;
    [Range(0f, 2f)] public float volumeGameOver;
}
