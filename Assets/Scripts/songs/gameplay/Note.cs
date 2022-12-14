using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Note : MonoBehaviour {
    public NoteHitEventChannel noteHitEventChannel;

    [HideInInspector]
    public NoteBoard noteBoard;
    [HideInInspector]
    public NOTE_BOARD_LANES lane;

    [Tooltip("The time that this note spawned, in seconds")]
    public float spawnTime;
    public float t;
    public Renderer m_renderer;

    public void SetMaterial(Material m) {
        m_renderer.material = m;
    }

    public void Move() {
        t += Time.deltaTime / SongPlayer.Instance.difficultySettings.noteLifetime;
        transform.position = noteBoard.GetNotePosition(t, lane);

        // If the note is expired by a greater amount than the worst hit timing threshold, destroy the note
        if (GetTimeToDestination() < -SongPlayer.Instance.difficultySettings.thresholdGood) {
            noteHitEventChannel.SendOnNoteMiss(lane, 0); //noteHitEventChannel.SendOnNoteMiss(lane, noteBoard.playerIndex);
            Destroy();
        }
    }

    /// <summary>
    /// Returns the time in seconds, until this note reaches the end of the note board
    /// </summary>
    /// <returns></returns>
    public float GetTimeToDestination() {
        return (1 - t) * SongPlayer.Instance.difficultySettings.noteLifetime;
    }

    // Destroys game object, and removes its reference from the note lane
    public void Destroy() {
        Note tmp = noteBoard.noteLanes[(int)lane].DequeueNote();

        if (tmp != this) {
            Debug.LogError($"Just dequeued a note that wasn't the one we're about to destroy! Lane: {lane}, me: [t = {t}, spawn time = {spawnTime}], it: [{t}, {tmp.spawnTime}]. ");
        }

        Destroy(gameObject);
    }
}
