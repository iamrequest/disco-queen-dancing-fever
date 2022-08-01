using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Sirenix.OdinInspector;

public class SongMenuItem : MonoBehaviour {
    public Image background;
    public Color bgColor, bgHoveredColor;

    [ButtonGroup("Selection")]
    public void Hover() {
        background.color = bgHoveredColor;
    }

    [ButtonGroup("Selection")]
    public void UnHover() {
        background.color = bgColor;
    }
}
