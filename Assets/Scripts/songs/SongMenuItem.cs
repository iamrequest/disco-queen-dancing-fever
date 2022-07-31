using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Sirenix.OdinInspector;

public class SongMenuItem : MonoBehaviour {
    public Image background;
    public TextMeshProUGUI songName;
    public TextMeshProUGUI artistName;
    public Color bgColor, bgHoveredColor;

    public void Render(SongMetadata metadata, bool isSelected) {
        Render(metadata);

        if (isSelected) Hover();
        else UnHover();
    }

    public void Render(SongMetadata metadata) {
        songName.text = metadata.songName;
        artistName.text = metadata.artistName;
    }


    [ButtonGroup("Selection")]
    public void Hover() {
        background.color = bgHoveredColor;
    }

    [ButtonGroup("Selection")]
    public void UnHover() {
        background.color = bgColor;
    }
}
