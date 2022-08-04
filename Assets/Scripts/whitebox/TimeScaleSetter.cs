using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Whitebox method to speed up time for testing
public class TimeScaleSetter : MonoBehaviour {
    [Range(0f, 5f)]
    public float t;

    [ReadOnly]
    public float currentTimeScale;


    [ButtonGroup("TimeScale")]
    [Button("Set to T")]
    public void SetTimeScaleToT() {
        SetTimeScale(t);
    }

    [ButtonGroup("TimeScale")]
    [Button("Reset")]
    public void ResetTimeScale() {
        SetTimeScale(1f);
    }

    [ButtonGroup("TimeScale1")]
    [Button("2x")]
    public void SetTimeScale2x() {
        Time.timeScale = 2f;
        SetTimeScale(2f);
    }
    [ButtonGroup("TimeScale1")]
    [Button("3x")]
    public void SetTimeScale3x() {
        SetTimeScale(3f);
    }

    [ButtonGroup("TimeScale1")]
    [Button("5x")]
    public void SetTimeScale5x() {
        SetTimeScale(5f);
    }
    [ButtonGroup("TimeScale1")]
    [Button("10x")]
    public void SetTimeScale10x() {
        SetTimeScale(10f);
    }

    private void SetTimeScale(float time) {
        currentTimeScale = time;
        SongPlayer.Instance.audioSource.pitch = time;
        Time.timeScale = time;
    }
}
