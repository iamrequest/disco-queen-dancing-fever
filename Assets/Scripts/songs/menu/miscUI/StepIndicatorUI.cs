using Freya;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StepIndicatorUI : MonoBehaviour {
    public Image uiLeftFoot, uiRightFoot;
    public PlayerInputMethodManager playerInputMethodManager;
    public float maxBoundsPos, maxBoundsAlpha;
    public AnimationCurve alphaLerp;

    private void Update() {
        Render();
    }

    public void Render() {
        CalculateUILocalPosition(playerInputMethodManager.footLeft, uiLeftFoot.transform);
        CalculateUILocalPosition(playerInputMethodManager.footRight, uiRightFoot.transform);
    }

    private void CalculateUILocalPosition(Transform src, Transform dst) {
        dst.localPosition = new Vector3(ClampValue(src.localPosition.x), ClampValue(src.localPosition.z), 0f);

        SetAlpha(src, uiLeftFoot);
        SetAlpha(src, uiRightFoot);
    }

    private float ClampValue(float value) {
        return Mathf.Clamp(value, -maxBoundsPos, maxBoundsPos);
    }

    private void SetAlpha(Transform src, Image img) {
        float furthestDimensionValue = Mathf.Abs(Mathf.Max(src.localPosition.x, src.localPosition.z));
        float alpha = alphaLerp.Evaluate(Mathfs.Remap(0f, maxBoundsAlpha, 0f, 1f, furthestDimensionValue));

        img.color = new Color(img.color.r, img.color.g, img.color.b, alpha);
    }
}
