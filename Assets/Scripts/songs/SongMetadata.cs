using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SongMetadata {
    public string songName;
    public string artistName;

    [Tooltip("Path to the folder containing this file, without a trailing slash")]
    public string fullDirectoryPath;
}
