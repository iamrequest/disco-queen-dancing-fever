using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXManager : MonoBehaviour {
    public static SFXManager Instance { get; private set; }

    public GameObject audioSourcePrefab;
    public List<AudioSource> audioSourcePool;
    public List<AudioSource> audioSourcesInUse;
    public int numAudioSources;

    private void Awake() {
        if (Instance == null) {
            Instance = this;
        } else {
            Debug.LogError($"Multiple {GetType()} components detected. This is probably a bug.");
            Destroy(this);
        }

        // Init pool
        for (int i = 0; i < numAudioSources; i++) {
            GameObject obj = Instantiate(audioSourcePrefab, transform);
            AudioSource audioSource = obj.GetComponent<AudioSource>();
            if (audioSource == null) {
                Debug.LogError("Audio source component is missing on the prefab!");
            } else {
                audioSourcePool.Add(audioSource);
            }
        }
    }

    public void PlayRandomSFX(List<AudioClip> clips, Vector3 worldPosition) {
        if (clips.Count == 0) return;
        PlaySFX(clips[Random.Range(0, clips.Count - 1)], transform.position);
    }

    public void PlaySFX(AudioClip clip, Vector3 worldPosition) {
        if (clip == null) return;
        if (audioSourcePool.Count == 0) {
            Debug.Log($"No available audio sources in the pool. Maybe consider bumping up the number of audio sources");
            return;
        }

        AudioSource audioSource = audioSourcePool[audioSourcePool.Count - 1];
        audioSource.transform.position = worldPosition;
        audioSource.PlayOneShot(clip);

        StartCoroutine(ReturnToPoolAfterDelay(audioSource, clip.length));
    }

    private IEnumerator ReturnToPoolAfterDelay(AudioSource audioSource, float lifetime) {
        audioSourcePool.Remove(audioSource);
        audioSourcesInUse.Add(audioSource);

        yield return new WaitForSeconds(lifetime);


        audioSource.clip = null;
        audioSource.Stop();
        audioSourcesInUse.Remove(audioSource);
        audioSourcePool.Add(audioSource);
    }
}
