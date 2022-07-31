using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

[ShowInInspector]
public static class SongLoader {
    private static string songPath = Application.streamingAssetsPath + "/songs";

    /// <summary>
    /// Load the metadata for all songs in the StreamingAssets dir.
    /// Next steps: Consider running this on a coroutine over a handful of frames
    /// </summary>
    /// <returns></returns>    
    public static List<SongMetadata> LoadAllSongMetdata() {
        DirectoryInfo songDirParent = new DirectoryInfo(songPath);
        DirectoryInfo[] songDirs = songDirParent.GetDirectories();
        List<SongMetadata> songsMetadata = new List<SongMetadata>();

        foreach (DirectoryInfo songDir in songDirs) {
            string metadataFullFilePath = songDir.FullName + "/metadata.json";

            if (!File.Exists(metadataFullFilePath)) {
                Debug.LogWarning($"Unable to read metadata file in [StreamingAssets/{songDir.Name}]: File doesn't exist.");
            } else {
                SongMetadata metadata = ReadMetadataFile(metadataFullFilePath);
                if (metadata != null) {
                    songsMetadata.Add(metadata);
                }
            }
        }

        return songsMetadata;
    }

    /// <summary>
    /// Load the metadata file into a SongMetadata object 
    /// </summary>
    /// <param name="metadataFullFilePath"></param>
    /// <returns></returns>
    private static SongMetadata ReadMetadataFile(string metadataFullFilePath) {
        SongMetadata metadata = new SongMetadata();

        try {
            string jsonStr = GetTextFileContents(metadataFullFilePath);
            metadata = JsonUtility.FromJson<SongMetadata>(jsonStr);

            metadata.fullDirectoryPath = Path.GetDirectoryName(metadataFullFilePath);

            return metadata;
        } catch (System.Exception e) {
            Debug.LogError($"Failed to parse metadata file: {metadataFullFilePath}: {e.Message}");
            return null;
        }
    }

    private static string GetTextFileContents(string filePath) {
        // Double check for safety
        if (!File.Exists(filePath)) {
            Debug.LogWarning($"Tried to get text file contents, but the file doesn't exist: {filePath}");
            return null;
        }

        //return "{ \"songName\": \"Example Song\", \"artistName\": \"Some artist\" }";
        return File.ReadAllText(filePath);
    }
}
