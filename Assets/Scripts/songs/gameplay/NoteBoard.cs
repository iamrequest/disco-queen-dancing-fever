using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum NOTE_BOARD_LANES { LEFT=0, UP = 1, DOWN=2, RIGHT=3}
public class NoteBoard : MonoBehaviour {
    private Vector3 lrOffset; // Offset to the edge of the note board
    private Vector3 centerOffset; // Offset from a note lane to the center of 2 note lates

    public GameStateEventChannel gameStateEventChannel;
    public InputEventChannel inputEventChannel;
    public NoteHitEventChannel noteHitEventChannel;
    public ScoreEventChannel scoreEventChannel;

    [HideInInspector]
    public List<NoteLane> noteLanes;

    public Transform noteOrigin, noteDestination;
    public GameObject notePrefab;

    public float boardWidth;
    private const int numPaths = 4;

    [Range(0, 64)]
    public int maxNumNotesPerLane;

    private void OnEnable() {
        inputEventChannel.onInput += OnInput;
        gameStateEventChannel.onGameStateChange += OnGameStateChange;

        // Populate note lanes. Doing this programically ensures that the list is ordered correctly
        if (noteLanes == null) {
            noteLanes = new List<NoteLane>();
        } else {
            noteLanes.Clear();
        }

        NoteLane[] foundNoteLanes = GetComponentsInChildren<NoteLane>();
        foreach (NoteLane noteLane in foundNoteLanes) {
            noteLanes.Insert((int) noteLane.lane, noteLane);
        }
    }

    private void OnDisable() {
        inputEventChannel.onInput -= OnInput;
        gameStateEventChannel.onGameStateChange -= OnGameStateChange;
    }

    private void Start() {
        UpdateBoardSize();
    }

    private void Update() {
        if (GameManager.Instance.gameState == GAME_STATE.GAME_ACTIVE) {
            foreach (NoteLane lane in noteLanes) {
                lane.MoveNotes();
            }
        }
    }

    private void ClearAllNoteLanes() {
        foreach (NoteLane lane in noteLanes) {
            if (lane != null) lane.Clear();
        }
    }


    public Vector3 GetNotePosition(float t, NOTE_BOARD_LANES laneNum) {
        float notePathWidth = boardWidth / numPaths;

        // Using unclamped lerp, so that notes go past the end of the board
        return Vector3.LerpUnclamped(noteOrigin.position - lrOffset + (Vector3.right * (int)laneNum * notePathWidth) + centerOffset, 
            noteDestination.position - lrOffset + (Vector3.right * (int)laneNum * notePathWidth) + centerOffset,
            t);
    }

    // Update calculated offsets, used when calculating note position movements
    private void UpdateBoardSize() {
        lrOffset = new Vector3(boardWidth / 2, 0f, 0f); // Offset to the right side 
        centerOffset = new Vector3(boardWidth / numPaths / 2, 0f, 0f);
    }

    private void OnDrawGizmosSelected() {
        UpdateBoardSize();

        // -- Draw the bounds of the board
        Gizmos.color = Color.white;
        Gizmos.DrawLine(noteOrigin.position - lrOffset, noteDestination.position - lrOffset);
        Gizmos.DrawLine(noteOrigin.position + lrOffset, noteDestination.position + lrOffset);

        // -- Draw the individual note paths
        Gizmos.color = Color.cyan;
        float notePathWidth = boardWidth / numPaths;

        for (int i = 0; i < numPaths; i++) {
            Vector3 pathOffset = Vector3.right * i * notePathWidth;
            pathOffset.x += notePathWidth / 2; // Center the note paths

            Gizmos.DrawLine(noteOrigin.position - lrOffset + (Vector3.right * i * notePathWidth) + centerOffset,
                noteDestination.position - lrOffset + (Vector3.right * i * notePathWidth) + centerOffset);

            //Gizmos.DrawLine(noteOrigin.position - lrOffset + pathOffset,
            //    noteDestination.position - lrOffset + pathOffset);
        }



        // -- Draw note thresholds
        // Fetch a reference to the song player (the singleton isn't initialized in editor)
        SongPlayer songPlayer;
        if (Application.isPlaying) {
            songPlayer = SongPlayer.Instance;
        } else {
            songPlayer = FindObjectOfType<SongPlayer>();
            if (songPlayer == null) return;
        }

        // Calculate a multiplying vector that converts a time in seconds, to distance along the note board
        Vector3 dirDestToOrigin = (noteOrigin.position - noteDestination.position).normalized;
        float magnitude = (noteOrigin.position - noteDestination.position).magnitude;
        Vector3 timeToDistMultiplier = dirDestToOrigin * (magnitude / songPlayer.difficultySettings.noteLifetime);

        if (songPlayer.difficultySettings) {
            // Draw the note threshold in each direction
            Gizmos.color = Color.red;
            Gizmos.DrawLine(noteDestination.position, noteDestination.position + timeToDistMultiplier * songPlayer.difficultySettings.thresholdGood);
            Gizmos.DrawLine(noteDestination.position, noteDestination.position - timeToDistMultiplier * songPlayer.difficultySettings.thresholdGood);

            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(noteDestination.position, noteDestination.position + timeToDistMultiplier * songPlayer.difficultySettings.thresholdGreat);
            Gizmos.DrawLine(noteDestination.position, noteDestination.position - timeToDistMultiplier * songPlayer.difficultySettings.thresholdGreat);

            Gizmos.color = Color.green;
            Gizmos.DrawLine(noteDestination.position, noteDestination.position + timeToDistMultiplier * songPlayer.difficultySettings.thresholdPerfect);
            Gizmos.DrawLine(noteDestination.position, noteDestination.position - timeToDistMultiplier * songPlayer.difficultySettings.thresholdPerfect);
        }

        Gizmos.color = Color.white;
    }

    private void OnDrawGizmos() {
    }


    /*
    // TODO: Optionally, make the distance to note origin user-configurable
    public float boardDistance;

    /// <summary>
    /// Save the distance from noteOrigin to noteDestination. Useful for inspector config
    /// </summary>
    [ButtonGroup("Board Distance")]
    private void SaveBoardDistance() {
        boardDistance = (noteDestination.position - noteOrigin.position).magnitude;
    }

    /// <summary>
    /// Move the note origin transform $boardDistance meters away from note destination 
    /// </summary>
    [ButtonGroup("Board Distance")]
    public void SetBoardDistance() {
        Vector3 destToOrigin = (noteOrigin.position - noteDestination.position).normalized;
        noteOrigin.position = destToOrigin * boardDistance;
    }
    */

    // Inits the board to a default state
    private void OnGameStateChange(GAME_STATE oldGameState, GAME_STATE newGameState) {
        switch (newGameState) {
            case GAME_STATE.GAME_OVER:
                ClearAllNoteLanes();
                break;
            case GAME_STATE.GAME_ACTIVE:
                if(oldGameState != GAME_STATE.GAME_PAUSED) ClearAllNoteLanes();
                break;
        }
    }



    private void OnInput(INPUT_DIRS inputDir, int playerIndex) {
        // Only one note board atm
        // if (this.playerIndex = playerIndex) { }

        if (GameManager.Instance.gameState == GAME_STATE.GAME_ACTIVE) {
            // Note: If these don't line up in the enum, the player will have weird input issues
            TryHitNote((NOTE_BOARD_LANES)inputDir);
        }
    }

    private void TryHitNote(NOTE_BOARD_LANES lane) {
        Note oldestNote = noteLanes[(int) lane].GetNoteAtOrder(0);

        // No notes in this lane
        if (oldestNote == null) return;

        // Assumption: Note threshold is the same before and after the note
        float timeToDestination = Mathf.Abs(oldestNote.GetTimeToDestination());
        //Debug.Log($"{timeToDestination}/{SongPlayer.Instance.difficultySettings.thresholdPerfect}");

        // Compare against each of the note thresholds (perfect/great/good) for this difficulty
        if (timeToDestination < SongPlayer.Instance.difficultySettings.thresholdPerfect) {
            scoreEventChannel.SendOnRequestScoreChanged(SongPlayer.Instance.difficultySettings.scoreValuePerfect);
            noteHitEventChannel.SendOnNoteHit(lane, NOTE_HIT_TYPES.PERFECT, 0);
            oldestNote.Destroy();

        } else if (timeToDestination < SongPlayer.Instance.difficultySettings.thresholdGreat) {
            scoreEventChannel.SendOnRequestScoreChanged(SongPlayer.Instance.difficultySettings.scoreValueGreat);
            noteHitEventChannel.SendOnNoteHit(lane, NOTE_HIT_TYPES.GREAT, 0);
            oldestNote.Destroy();

        } else if (timeToDestination < SongPlayer.Instance.difficultySettings.thresholdGood) {
            scoreEventChannel.SendOnRequestScoreChanged(SongPlayer.Instance.difficultySettings.scoreValueGood);
            noteHitEventChannel.SendOnNoteHit(lane, NOTE_HIT_TYPES.GOOD, 0);
            oldestNote.Destroy();
        }
    }

    // --------------------------------------------------------------------------------
    // Editor Debug
    // --------------------------------------------------------------------------------
    // TODO: Object pool
    [Button]
    public void SpawnNote(NOTE_BOARD_LANES lane) {
        if (noteLanes.Count < (int)lane) {
            Debug.LogError($"Unable to spawn note in lane [{lane}]: That lane doesn't exist!");
            return;
        } else if (noteLanes[(int)lane] == null) {
            Debug.LogError($"Unable to spawn note in lane [{lane}]: This note lane is null!");
            return;
        }

        noteLanes[(int)lane].SpawnNote();
    }

    [ButtonGroup("Spawn Notes")] [Button("<")]
    private void SpawnNoteLeft() { SpawnNote(NOTE_BOARD_LANES.LEFT); }
    [ButtonGroup("Spawn Notes")] [Button("^")]
    private void SpawnNoteUp() { SpawnNote(NOTE_BOARD_LANES.UP); }
    [ButtonGroup("Spawn Notes")] [Button("v")]
    private void SpawnNoteDown() { SpawnNote(NOTE_BOARD_LANES.DOWN); }
    [ButtonGroup("Spawn Notes")] [Button(">")]
    private void SpawnNoteRight() { SpawnNote(NOTE_BOARD_LANES.RIGHT); }
}
