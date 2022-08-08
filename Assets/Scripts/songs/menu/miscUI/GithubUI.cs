using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GithubUI : GazableMenu {
    public GazeMenuButton btn;

    protected override void OnEnable() {
        base.OnEnable();

        btn.onStateChanged.AddListener(OpenGithubURL);
    }

    protected override void OnDisable() {
        base.OnDisable();

        btn.onStateChanged.RemoveListener(OpenGithubURL);
    }


    // --------------------------------------------------------------------------------
    // Gaze Button Functionality
    // --------------------------------------------------------------------------------
    private void OpenGithubURL(bool state) {
        Application.OpenURL("https://github.com/iamrequest/disco-queen-dancing-fever");
        btn.label.text = "Opened! \nCheck your browser!";
    }
}
