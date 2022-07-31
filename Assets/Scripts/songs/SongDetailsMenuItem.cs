using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SongDetailsMenuItem : SongMenuItem {
    public TextMeshProUGUI songName;
    public TextMeshProUGUI artistName;

    public void Render(SongMetadata metadata, bool isSelected) {
        Render(metadata);

        if (isSelected) Hover();
        else UnHover();
    }

    public void Render(SongMetadata metadata) {
        songName.text = metadata.songName;
        artistName.text = metadata.artistName;

    }
}
