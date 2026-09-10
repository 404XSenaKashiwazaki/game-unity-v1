using UnityEngine;
using System.Collections;

public class ArtilleryAmbientSound : MonoBehaviour
{
    public AudioClip[] explosionClips;
    public float minDelay = 4f;
    public float maxDelay = 10f;

    [Range(0f, 1f)]
    public float volumeMultiplier = 0.35f; 

    void Start()
    {
        StartCoroutine(Loop());
    }

    IEnumerator Loop()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(minDelay, maxDelay));
            if (DroneController.isReplaying)
            {
                if (AudioManager.Instance != null &&
                    AudioManager.Instance.sfxSource.isPlaying)
                {
                    AudioManager.Instance.sfxSource.Stop();
                }
                continue;
            }

            if (AudioManager.Instance == null || explosionClips.Length == 0)
                continue;

            AudioClip clip = explosionClips[
                Random.Range(0, explosionClips.Length)
            ];

            float prevVolume = AudioManager.Instance.sfxSource.volume;
            AudioManager.Instance.sfxSource.volume =
                PlayerPrefs.GetFloat("SFX_VOLUME", 1f) * volumeMultiplier;

            AudioManager.Instance.PlaySfx(clip);

            AudioManager.Instance.sfxSource.volume = prevVolume;
        }
    }
}
