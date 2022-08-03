using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// This is a menu that lists all of the songs.
/// </summary>
public class SongDetailsMenu : MonoBehaviour {
    public static SongDetailsMenu Instance { get; private set; }

    // Items to render on the UI
    public List<SongDetailsMenuItem> songMenuItems;

    // List of songs
    public List<SongMetadata> songsMetadata;

    public TextMeshProUGUI noSongsFoundText;

    public GameObject songMenuItemPrefab;


    [Tooltip("Parent transform for song menu items")]
    public Transform songMenuItemParentTransform;
    public float itemHeight;

    [Range(0, 10)]
    public int itemsToDisplay;
    public int highlightedItemIndex;
    public int currentSongIndex;

    private void Awake() {
        if (Instance == null) {
            Instance = this;
        } else {
            Debug.LogError($"Multiple {GetType()} components detected. This is probably a bug.");
            Destroy(this);
        }

        noSongsFoundText.gameObject.SetActive(false);
    }
    private void Start() {
        Render();
    }

    // Read song listing from StreamingAssets
    [Button]
    public void LoadSongsMetadata() {
        songsMetadata = SongLoader.LoadAllSongMetdata();
    }

    // Inspector usage, mainly
    [ButtonGroup("MenuGen")]
    private void CreateSongMenuItems() {
        if (songsMetadata.Count == 0) {
            LoadSongsMetadata();
        }

        if (songMenuItems.Count > 0) {
            DeleteSongMenuItems();
        }

        highlightedItemIndex = 0;
        currentSongIndex = 0;

        for (int i = 0; i < Mathf.Min(itemsToDisplay, songsMetadata.Count); i++) {
            GameObject songMenuItemInstance = Instantiate(songMenuItemPrefab, songMenuItemParentTransform);
            SongDetailsMenuItem menuItem = songMenuItemInstance.GetComponent<SongDetailsMenuItem>();
            songMenuItems.Add(menuItem);

            menuItem.transform.localPosition = new Vector3(0f, i * itemHeight, 0f);
        }
    }

    // Inspector usage, mainly
    [ButtonGroup("MenuGen")]
    private void DeleteSongMenuItems() {
        for (int i = 0; i < songMenuItems.Count;) {
            if (Application.isPlaying) {
                Destroy(songMenuItems[songMenuItems.Count - 1].gameObject);
            } else {
                DestroyImmediate(songMenuItems[songMenuItems.Count - 1].gameObject);
            }

            songMenuItems.RemoveAt(songMenuItems.Count - 1);
        }
    }


    [Button]
    public void Render() {
        if (songMenuItems.Count == 0) {
            CreateSongMenuItems();
        }

        // No songs found, don't bother rendering
        if (songsMetadata.Count == 0) {
            noSongsFoundText.gameObject.SetActive(true);
            return;
        }

        // Render each individual item
        for (int i = 0; i < Mathf.Min(itemsToDisplay, songsMetadata.Count); i++) {
            songMenuItems[i].Render(songsMetadata[currentSongIndex + i], 
                i == highlightedItemIndex);
        }

        // Render difficulty menu
        SongDifficultyMenu.Instance.Render(GetSelectedSongMetadata());
    }

    // Menu interaction
    [ButtonGroup("SongManagement")]
    public void PrevSong() {
        // If we're at the end of the list of items, also advance the current song index
        if (highlightedItemIndex == 0) {
            currentSongIndex = Mathf.Max(0, currentSongIndex - 1);
        }

        highlightedItemIndex = Mathf.Max(0, highlightedItemIndex - 1);

        Render();
    }

    // Menu interaction
    [ButtonGroup("SongManagement")]
    public void NextSong() {
        // If we're at the end of the list of items, also advance the current song index
        if (highlightedItemIndex == (itemsToDisplay - 1)) {
            currentSongIndex = Mathf.Min(currentSongIndex + 1, songsMetadata.Count - songMenuItems.Count);
        }

        highlightedItemIndex = Mathf.Min(highlightedItemIndex + 1, songMenuItems.Count - 1);
        Render();
    }

    [Button]
    public SongMetadata GetSelectedSongMetadata() {
        //Debug.Log($"Selected song index: {highlightedItemIndex + currentSongIndex}");
        return songsMetadata[highlightedItemIndex + currentSongIndex];
    }
}
