using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This is a menu that lists all of the difficulties, for a given song.
/// TODO: Should refactor stuff from this and songDetailsMenu into some parent class
/// </summary>
public class SongDifficultyMenu : MonoBehaviour {
    // Items to render on the UI
    public List<SongDifficultyMenuItem> songDifficultyMenuItems;

    // List of songs
    public SongMetadata songMetadata;

    public GameObject songMenuItemPrefab;


    [Tooltip("Parent transform for song menu items")]
    public Transform songMenuItemParentTransform;
    public float itemHeight;

    [Range(0, 10)]
    public int itemsToDisplay;
    public int highlightedItemIndex;
    public int currentDifficultyIndex;

    // Inspector usage, mainly
    [ButtonGroup("MenuGen")]
    private void CreateSongMenuItems() {
        if (songDifficultyMenuItems.Count > 0) {
            DeleteSongMenuItems();
        }

        highlightedItemIndex = 0;
        currentDifficultyIndex = 0;

        for (int i = 0; i < Mathf.Min(itemsToDisplay, songMetadata.songDifficulties.Count); i++) {
            GameObject songMenuItemInstance = Instantiate(songMenuItemPrefab, songMenuItemParentTransform);
            SongDifficultyMenuItem menuItem = songMenuItemInstance.GetComponent<SongDifficultyMenuItem>();
            songDifficultyMenuItems.Add(menuItem);

            menuItem.transform.localPosition = new Vector3(0f, i * itemHeight, 0f);
        }
    }

    // Inspector usage, mainly
    [ButtonGroup("MenuGen")]
    private void DeleteSongMenuItems() {
        for (int i = 0; i < songDifficultyMenuItems.Count;) {
            if (Application.isPlaying) {
                Destroy(songDifficultyMenuItems[songDifficultyMenuItems.Count - 1].gameObject);
            } else {
                DestroyImmediate(songDifficultyMenuItems[songDifficultyMenuItems.Count - 1].gameObject);
            }

            songDifficultyMenuItems.RemoveAt(songDifficultyMenuItems.Count - 1);
        }
    }


    [Button]
    public void Render() {
        if (songDifficultyMenuItems.Count == 0) {
            CreateSongMenuItems();
        }

        // Render each individual item
        for (int i = 0; i < Mathf.Min(itemsToDisplay, songMetadata.songDifficulties.Count); i++) {
            songDifficultyMenuItems[i].Render(songMetadata.songDifficulties[currentDifficultyIndex + i], 
                i == highlightedItemIndex);
        }
    }
    public void Render(SongMetadata songMetadata) {
        this.songMetadata = songMetadata;
        DeleteSongMenuItems();
        Render();
    }


    // Menu interaction
    [ButtonGroup("SongManagement")]
    public void PrevSong() {
        // If we're at the end of the list of items, also advance the current song index
        if (highlightedItemIndex == 0) {
            currentDifficultyIndex = Mathf.Max(0, currentDifficultyIndex - 1);
        }

        highlightedItemIndex = Mathf.Max(0, highlightedItemIndex - 1);

        Render();
    }

    // Menu interaction
    [ButtonGroup("SongManagement")]
    public void NextSong() {
        // If we're at the end of the list of items, also advance the current song index
        if (highlightedItemIndex == (itemsToDisplay - 1)) {
            currentDifficultyIndex = Mathf.Min(currentDifficultyIndex + 1, songMetadata.songDifficulties.Count - songDifficultyMenuItems.Count);
        }

        highlightedItemIndex = Mathf.Min(highlightedItemIndex + 1, songDifficultyMenuItems.Count - 1);
        Render();
    }

    [Button]
    public SongDifficulty GetSelectedSongDifficulty() {
        //Debug.Log($"Selected song difficulty index: {highlightedItemIndex + currentDifficultyIndex}");
        return songMetadata.songDifficulties[highlightedItemIndex + currentDifficultyIndex];
    }
}
