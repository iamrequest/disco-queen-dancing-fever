using DG.Tweening;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpIndicatorUI : MonoBehaviour {
    public SettingsEventChannel settingsEventChannel;
    public FeverEventChannel feverEventChannel;
    public Material uiMaterial;
    public float showAlpha;
    public float fadeDuration;

    private void Awake() {
        UpdatePosition();
        HideUI(0);
        DOTween.Init();
    }

    private void OnEnable() {
        settingsEventChannel.onCalibrateFloorHeight += OnCalibrateFloorHeight;
        feverEventChannel.onFeverReady += ShowUI;
        feverEventChannel.onFeverActivated += HideUI;
        feverEventChannel.onFeverFinished += HideUI;
    }

    private void OnDisable() {
        settingsEventChannel.onCalibrateFloorHeight -= OnCalibrateFloorHeight;
        feverEventChannel.onFeverReady -= ShowUI;
        feverEventChannel.onFeverActivated -= HideUI;
        feverEventChannel.onFeverFinished -= HideUI;
    }

    [ButtonGroup]
    private void HideUI(int playerIndex) {
        uiMaterial.DOFade(0f, fadeDuration);
    }

    [ButtonGroup]
    private void ShowUI(int playerIndex) {
        uiMaterial.DOFade(showAlpha, fadeDuration);
    }

    private void OnCalibrateFloorHeight(float newFloorHeight) {
        UpdatePosition();
    }

    [ButtonGroup]
    private void UpdatePosition() {
        transform.localPosition = new Vector3(0f, settingsEventChannel.jumpMinHeight, 0f);
    }
}
