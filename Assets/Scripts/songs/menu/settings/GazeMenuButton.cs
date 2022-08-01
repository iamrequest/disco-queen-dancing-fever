using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;

// These are simple boolean buttons, which is fine for my use-case. Some of these inputs should be toggles, or one-shot checkboxes.
public class GazeMenuButton : MonoBehaviour {
    private bool isHoveredThisFrame;
    public bool isSelected;
    public bool canDeselect;

    [Tooltip("How long it takes to select this item, when hovered")]
    public float hoverSelectionDuration;
    private float elapsedHoverSelectionDuration;
    private bool hoverDisabled; // Disable hover after selection, re-enable after off-hover

    public Image icon;
    public Sprite selectedIcon, deselectedIcon;
    public TextMeshProUGUI label;

    public UnityEvent<bool> onStateChanged;

    private void Update() {
        if (!isHoveredThisFrame) {
            elapsedHoverSelectionDuration = Mathf.Max(0f, elapsedHoverSelectionDuration - Time.deltaTime);
            hoverDisabled = false;
        }
        isHoveredThisFrame = false;
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
            if (isSelected && this.isSelected) return;

            // Disallow de-selection, if that option is enabled
            if (this.isSelected && !canDeselect) return;
        }

        this.isSelected = isSelected;

        if (isSelected) {
            icon.sprite = selectedIcon;
        } else {
            icon.sprite = deselectedIcon;
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
        onStateChanged.Invoke(isSelected);
    }

    public void OnHover() {
        isHoveredThisFrame = true;

        // If we just changed the value of the button, disable hover
        if (hoverDisabled) return;

        // Disallow de-selection, if that option is enabled
        if (isSelected && canDeselect) return;

        elapsedHoverSelectionDuration += Time.deltaTime;

        if (elapsedHoverSelectionDuration >= hoverSelectionDuration) {
            elapsedHoverSelectionDuration = 0f;
            hoverDisabled = true;
            SetSelected(!isSelected);
        }
    }
}
