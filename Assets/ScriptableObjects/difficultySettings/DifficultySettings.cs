using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Difficulty Settings")]
public class DifficultySettings : ScriptableObject {
    [Tooltip("Duration it takes a note to traverse the board")]
    public float noteLifetime;

    [Tooltip("The percentage of a perfect score you'd have to hit to get fever mode.\neg: If a perfect score is 100, then you might need to get 20 points to be able to use fever mode")]
    [Range(0f, 1f)]
    public float perfectScorePercentageToFeverMode;

    // Next step: Maybe use the BPM and time signature to calculate the length of fever mode. Might be hard for songs that change bpm/time signature, but maybe it could be encoded in the midi file
    [Tooltip("Duration of fever mode, in seconds")]
    [Range(0f, 60f)]
    public float feverModeDuration;

    [Header("Note timings (in seconds)")]
    public float thresholdPerfect;
    public float thresholdGreat;
    public float thresholdGood;

    // This should probably not change on a per-difficulty basis, but it's easier to throw it in here. Can refactor later
    [Header("Score values for each note")]
    public int scoreValuePerfect;
    public int scoreValueGreat;
    public int scoreValueGood;

    [Header("Life Settings")]
    public int healthMax;
    public int damageNoteMiss;
    public int healthRegenPerfect, healthRegenGreat, healthRegenGood;
}
