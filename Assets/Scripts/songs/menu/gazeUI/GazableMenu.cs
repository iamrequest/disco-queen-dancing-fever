using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GazableMenu : MonoBehaviour {
    // Render an icon where the user is looking. Gradually make it fade after not looking for a while
    [Header("Gaze Icon")]
    public Image gazeIcon;
    private float gazeIconElapsedDuration; // Visibility duration
    public float gazeIconDuration;
    public float gazeIconLerpSpeed;
    private Vector3 gazeLastSeenPosition;
    public AnimationCurve gazeIconVisibilityCurve;

    protected virtual void OnEnable() {
        // Hide the gaze icon on menu menu spawn
        gazeIconElapsedDuration = gazeIconDuration;
    }

    protected virtual void OnDisable() {
        // Hide pointer on disable. This gets around an animator bug that freezes the gaze icon's alpha (since it's being set in an animator state)
        Color c = gazeIcon.color;
        c.a = 0f;
        gazeIcon.color = c;
    }

    // --------------------------------------------------------------------------------
    // Gaze UI Icon
    // --------------------------------------------------------------------------------
    public void OnGaze(Vector3 worldPosition) {
        gazeLastSeenPosition = worldPosition;

        // If the gaze icon is currently hidden, snap it to the current position
        if (gazeIconElapsedDuration >= gazeIconDuration) {
            gazeIcon.transform.position = gazeLastSeenPosition;
        } else {
            gazeIcon.transform.position = Vector3.Lerp(gazeIcon.transform.position, gazeLastSeenPosition, gazeIconLerpSpeed);
        }

        gazeIconElapsedDuration = 0f;
    }

    public void Update() {
        UpdateGazeIcon();
    }

    private void UpdateGazeIcon() {
        gazeIconElapsedDuration += Time.deltaTime;

        // Lerp icon alpha to 0
        Color c = gazeIcon.color;
        c.a = gazeIconVisibilityCurve.Evaluate(gazeIconElapsedDuration / gazeIconDuration);
        gazeIcon.color = c;
    }

}
