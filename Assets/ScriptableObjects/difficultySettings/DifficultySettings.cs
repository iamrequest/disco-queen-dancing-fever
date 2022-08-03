using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Difficulty Settings")]
public class DifficultySettings : ScriptableObject {
    [Tooltip("Duration it takes a note to traverse the board")]
    public float noteLifetime;

    [Header("Note timings (in seconds)")]
    public float thresholdPerfect;
    public float thresholdGreat;
    public float thresholdGood;

    // This should probably not change on a per-difficulty basis, but it's easier to throw it in here. Can refactor later
    [Header("Score values for each note")]
    public int scoreValuePerfect;
    public int scoreValueGreat;
    public int scoreValueGood;
}
