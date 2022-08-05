using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[System.Serializable]
public class SongMetadata {
    [Header("User-Supplied")]
    public string songName;
    public string artistName;
    public float bpm;
    public string audioFilename;

    // Currently unused, mostly for future proofing.
    public string gamemode; // Currently always "classic"
    public string version; // Currently always "1.0"

    public List<SongDifficulty> songDifficulties;


    [Header("Calculated")]
    [Tooltip("Path to the folder containing this file, without a trailing slash")]
    public string fullDirectoryPath;

    public AudioType GetAudioType() {
        string fileExtension = Path.GetExtension(audioFilename);
        switch (fileExtension.ToLower()) {
            case ".wav":
                return AudioType.WAV;
            case ".ogg":
                return AudioType.OGGVORBIS;
            case ".mp3":
                return AudioType.MPEG;
            default:
                return AudioType.UNKNOWN;
        }
    }
}

[System.Serializable]
public class SongDifficulty {
    [Header("User-Supplied")]
    [Tooltip("Custom string to represent this difficulty")]
    public string difficultyName;

    [Tooltip("Filename of the difficulty for this track, relative to this song's directory")]
    public string midiFilename;

    // For some reason, Reaper creates 1-indexed MIDI tracks. So, this is just a nice way of letting the player choose the track, since it seems that different DAWs do it differently.
    [Tooltip("The MIDI track that is responsible for spawning notes for the player to hit.")]
    public int playerInputMidiTrack;
}
