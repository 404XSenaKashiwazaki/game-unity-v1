using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Audio Sources")]
    public AudioSource musicSource;
    public AudioSource sfxSource;

    void Awake()
    {
     
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void SetMusicVolume(float value)
    {
        if (musicSource != null)
            musicSource.volume = value;
    }

    public void SetSfxVolume(float value)
    {
        Debug.Log(value);
        if (sfxSource != null)
            sfxSource.volume = value;
    }
}
