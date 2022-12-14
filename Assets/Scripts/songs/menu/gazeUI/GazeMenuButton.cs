using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;
using System;

// These are simple boolean buttons, which is fine for my use-case. Some of these inputs should be toggles, or one-shot checkboxes.
public class GazeMenuButton : MonoBehaviour {
    private GazableMenu gazableMenu;

    public GameStateEventChannel gameStateEventChannel;
    private bool isHoveredThisFrame;
    public bool isSelected;
    public bool canDeselect;
    [Tooltip("Silently forces the value to false")]
    public bool deselectOnUnhover;
    [Tooltip("Plays on change of state, after gazing at the button for long enough")]
    public AudioClip onGazeSFX;


    [Tooltip("How long it takes to select this item, when hovered")]
    public float hoverSelectionDuration;
    private float elapsedHoverSelectionDuration;
    private bool hoverDisabled; // Disable hover after selection, re-enable after off-hover
    public AnimationCurve imgFillAlpha;

    [Header("References")]
    public Image imgIcon;
    public Image imgFill;
    public Sprite selectedIcon, deselectedIcon;
    public TextMeshProUGUI label;
    public Transform centerTransform; // Used for snapping the UI icon to the center of this button

    public UnityEvent<bool> onStateChanged;

    public GAME_STATE deselectOnGameState;

    private void Awake() {
        gazableMenu = GetComponentInParent<GazableMenu>();
        if (!gazableMenu) {
            Debug.LogWarning("Could not find a gazable menu in the parent of this gameobject!");
        }

        SetSelectedSilent(isSelected, true);
    }

    private void OnEnable() {
        gameStateEventChannel.onGameStateChange += OnGameStateChange;
    }
    private void OnDisable() {
        gameStateEventChannel.onGameStateChange -= OnGameStateChange;
    }

    private void Update() {
        if (!isHoveredThisFrame) {
            elapsedHoverSelectionDuration = Mathf.Max(0f, elapsedHoverSelectionDuration - Time.deltaTime);
            if (hoverDisabled && deselectOnUnhover) {
                SetSelectedSilent(false, true);
            }
            hoverDisabled = false;
        } 

        isHoveredThisFrame = false;

        UpdateFillAlpha();
    }

    private void OnGameStateChange(GAME_STATE oldGameState, GAME_STATE newGameState) {
        if ((newGameState & deselectOnGameState) > 0) {
            SetSelectedSilent(false, true);
        }
    }

    Color colorTmpFill;
    private void UpdateFillAlpha() {
        colorTmpFill = imgFill.color;

        // If we've already selected the button, and we're still hovering, keep holding onto full opacity
        colorTmpFill.a = imgFillAlpha.Evaluate(elapsedHoverSelectionDuration / hoverSelectionDuration);

        imgFill.color = colorTmpFill;
    }

    /// <summary>
    /// Change the state of this button silently (without sending state changed events).
    /// Useful for initializing the state at the beginning of the game, without getting into stack overflows
    /// </summary>
    /// <param name="isSelected"></param>
    /// <param name="force">If true, this ignores "canDeselect"</param>
    [Button]
    public void SetSelectedSilent(bool isSelected, bool force = false) {
        if (!force) {
            // Ignore setting the value if no change is made (avoid event spam)
            if (isSelected == this.isSelected) return;

            // Disallow de-selection, if that option is enabled
            if (!isSelected && !canDeselect) return;
        }

        //Debug.Log($"Changing state of button: from {this.isSelected} to {isSelected} ");
        this.isSelected = isSelected;

        if (isSelected) {
            imgIcon.sprite = selectedIcon;
        } else {
            imgIcon.sprite = deselectedIcon;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="isSelected"></param>
    /// <param name="force">If true, this ignores "canDeselect"</param>
    [Button]
    public void SetSelected(bool isSelected, bool force = false) {
        SetSelectedSilent(isSelected, force);
        SFXManager.Instance.PlaySFX(onGazeSFX, transform.position);
        onStateChanged.Invoke(isSelected);
    }

    public void OnHover() {
        isHoveredThisFrame = true;
        gazableMenu.OnGaze(centerTransform.position);

        // If we just changed the value of the button, disable hover
        if (hoverDisabled) return;

        // Disallow de-selection, if that option is enabled
        if (isSelected && !canDeselect) return;

        elapsedHoverSelectionDuration += Time.deltaTime;

        if (elapsedHoverSelectionDuration >= hoverSelectionDuration) {
            hoverDisabled = true;
            SetSelected(!isSelected);
        }
    }
}
