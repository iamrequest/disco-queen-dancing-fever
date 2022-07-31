using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class StreamingAssetsReadTest : MonoBehaviour {
    private string songPath = Application.streamingAssetsPath + "/songs";

    [Button]
    public void ListStreamingAssetsContents() {
        DirectoryInfo songDirParent = new DirectoryInfo(songPath);
        DirectoryInfo[] songDirs = songDirParent.GetDirectories();

        foreach (DirectoryInfo songDir in songDirs) {
            Debug.Log($"Found song dir. Name: {songDir.Name}");
            Debug.Log($"Found song dir. Fullname: {songDir.FullName}");
            FileInfo metadataFile = new FileInfo(songDir.FullName + "/metadata.json");
            if (metadataFile.Exists) {
                // TODO: Read json
                Debug.Log($"File exists");
            } else {
                Debug.LogWarning("Unable to read metadata file: File doesn't exist.");
            }
        }
    }
}
