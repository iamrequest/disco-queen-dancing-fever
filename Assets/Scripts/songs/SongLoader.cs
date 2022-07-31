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

            if (IsMetadataFileValid(metadata)) {
                return metadata;
            } else {
                return null;
            }
        } catch (System.Exception e) {
            Debug.LogError($"Failed to parse metadata file: {metadataFullFilePath}: {e.Message}");
            return null;
        }
    }

    /// <summary>
    /// Confirms that the contents of the metadata file are as expected.
    /// Checks that the midi files listed in each difficulty setting exist in the current dir
    /// </summary>
    /// <param name="metadata"></param>
    /// <returns></returns>    
    private static bool IsMetadataFileValid(SongMetadata metadata) {
        List<string> failureReasons = new List<string>();

        // Validate that there are difficulty tracks for this song.
        if (metadata.songDifficulties == null) {
            failureReasons.Add("No difficulties exist for this song.\n");
        } else {
            if (metadata.songDifficulties.Count == 0) {
                failureReasons.Add("No difficulties exist for this song.\n");
            }

            // Validate that each difficulty for this song has a MIDI file that exists in the current dir
            foreach (SongDifficulty difficulty in metadata.songDifficulties) {
                if (!File.Exists(metadata.fullDirectoryPath + "/" + difficulty.midiFilename)) {
                    failureReasons.Add($"Difficulty [{difficulty.difficultyName}] is missing a midi track (File [{metadata.fullDirectoryPath + "/" + difficulty.midiFilename}] doesn't exist)\n");
                }
            }
        }

        if (failureReasons.Count > 0) {
            string failureStr = string.Concat(failureReasons);
            Debug.LogWarning($"Invalid metadata file [{metadata.songName}] at [{metadata.fullDirectoryPath}]:\n{failureStr}");
        }
        return failureReasons.Count == 0;
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
