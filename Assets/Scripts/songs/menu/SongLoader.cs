using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.Networking;

[ShowInInspector]
public static class SongLoader {
    private static string songPath = Application.streamingAssetsPath + "/songs";

    // --------------------------------------------------------------------------------
    // Metadata file
    // --------------------------------------------------------------------------------

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

        // Validate that the audio file exists
        if (!File.Exists(metadata.fullDirectoryPath + "/" + metadata.audioFilename)) {
            failureReasons.Add($"No audio file exist for this song at [{metadata.fullDirectoryPath + "/" + metadata.audioFilename}].\n");
        }

        // Validate that the filetype for the audio file is valid
        // Next steps: Validate this better, it's likely possible to rename the extension to get past validation, breaking something in the process
        string fileExtension = Path.GetExtension(metadata.audioFilename);
        switch (fileExtension.ToLower()) {
            case ".wav":
            case ".ogg":
            case ".mp3":
                break;
            default:
                failureReasons.Add($"Audio type [{fileExtension}] is not valid (Only wav, ogg, and mp3 are allowed).\n");
                break;
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


    // --------------------------------------------------------------------------------
    // Audio Clip
    // --------------------------------------------------------------------------------
    public static IEnumerator LoadAudioClip(AudioSource targetAudioSource, SongMetadata metadata) {
        yield return LoadAudioClip(targetAudioSource, 
            metadata.fullDirectoryPath + "/" + metadata.audioFilename, 
            metadata.GetAudioType());
    }

    public static IEnumerator LoadAudioClip(AudioSource targetAudioSource, string filePath, AudioType audioType) {
        // Validate that we're using the right call
        switch (audioType) {
            case AudioType.WAV:
            case AudioType.OGGVORBIS:
            case AudioType.MPEG:
                break;
            default:
                Debug.LogWarning($"Unable to get audio clip contents: Audio type is not valid (Only wav, ogg, and mp3 are allowed. Got [{audioType}])");
                yield break;
        }

        UnityWebRequest request = UnityWebRequestMultimedia.GetAudioClip(filePath, AudioType.OGGVORBIS);

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.ConnectionError) {
            Debug.LogWarning($"Unable to get audio clip contents: {request.result}");
            yield break;
        } else {
            AudioClip clip = DownloadHandlerAudioClip.GetContent(request);
            clip.name = filePath;
            targetAudioSource.clip = clip;
        }
    }
}
