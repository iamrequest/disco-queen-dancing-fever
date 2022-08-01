using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SongMetadata {
    public string songName;
    public string artistName;
    public float bpm;

    [Tooltip("Path to the folder containing this file, without a trailing slash")]
    public string fullDirectoryPath;

    public List<SongDifficulty> songDifficulties;
}

[System.Serializable]
public class SongDifficulty {
    [Tooltip("Custom string to represent this difficulty")]
    public string difficultyName;

    [Tooltip("Filename of the difficulty for this track, relative to this song's directory")]
    public string midiFilename;

    // For some reason, Reaper creates 1-indexed MIDI tracks. So, this is just a nice way of letting the player choose the track, since it seems that different DAWs do it differently.
    [Tooltip("The MIDI track that is responsible for spawning notes for the player to hit.")]
    public int playerInputMidiTrack;
}
