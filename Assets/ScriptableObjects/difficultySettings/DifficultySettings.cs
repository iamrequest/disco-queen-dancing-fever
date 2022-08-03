using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Difficulty Settings")]
public class DifficultySettings : ScriptableObject {
    [Tooltip("Duration it takes a note to traverse the board")]
    public float noteLifetime;

    [Header("Note timings (in seconds)")]
    public float perfectThreshold;
    public float greatThreshold;
    public float goodThreshold;
}
