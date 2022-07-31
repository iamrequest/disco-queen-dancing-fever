using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SongDifficultyMenuItem : SongMenuItem {
    public TextMeshProUGUI difficulty;
    //public TextMeshProUGUI highscore;

    public void Render(SongDifficulty songDifficulty, bool isSelected) {
        Render(songDifficulty);

        if (isSelected) Hover();
        else UnHover();
    }

    public void Render(SongDifficulty songDifficulty) {
        difficulty.text = "Difficulty: " + songDifficulty.difficultyName;
        //highscore.text = "High Score: " + songDifficulty.highscore;
    }
}
