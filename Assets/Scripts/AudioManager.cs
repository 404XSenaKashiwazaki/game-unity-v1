using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Audio Sources")]
    public AudioSource uiSource;
    public AudioSource sfxSource;
    public AudioSource loopSource; 

    [Header("Clips")]
    public AudioClip uiClick;
    public AudioClip victoryClip;
    public AudioClip gameOverClip;
    public AudioClip gatePassedClip;

    float masterVolume = 1f;
    [Header("UI Volume Multiplier")]
    [Range(1f, 3f)]
    public float uiClickMultiplier = 5f;

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        masterVolume = PlayerPrefs.GetFloat("MASTER_VOLUME", 1f);
        ApplyVolume();
    }

    public void SetVolume(float value)
    {
        masterVolume = value;
        PlayerPrefs.SetFloat("MASTER_VOLUME", value);
        PlayerPrefs.Save();
        ApplyVolume();
    }

    void ApplyVolume()
    {
        uiSource.volume   = masterVolume;
        sfxSource.volume  = masterVolume;
        loopSource.volume = masterVolume;
    }

    // public void PlayClick()
    // {
    //     uiSource.PlayOneShot(uiClick, uiClickMultiplier);
    // }

    // public void PlaySfx(AudioClip clip)
    // {
    //     sfxSource.PlayOneShot(clip, 1f);
    // }

    public void PlayClick()
    {

        if (uiClick == null || uiSource == null) return;
        uiSource.PlayOneShot(uiClick, uiClickMultiplier);
    }

    public void PlaySfx(AudioClip clip)
    {
        if (clip == null || sfxSource == null) return;
        sfxSource.PlayOneShot(clip, 1f);
    }

    // public void PlaySfx(AudioClip clip)
    // {
    //     if (clip == null || sfxSource == null) return;
    //     sfxSource.PlayOneShot(clip, uiClickMultiplier);
    // }

    public void PlayLoop(AudioClip clip)
    {
        if (loopSource.clip == clip && loopSource.isPlaying) return;
        loopSource.clip = clip;
        loopSource.loop = true;
        loopSource.Play();
    }

    public void StopLoop()
    {
        if (this == null) return;
        if (loopSource == null) return;

        loopSource.Stop();
        loopSource.clip = null;
    }

    public void PlayVictory()
    {
        StopLoop();
        PlaySfx(victoryClip);
    }

    public void PlayGatePassed ()
    {
        PlaySfx(gatePassedClip);
    }

    public void PlayGameOver()
    {
        StopLoop();
        PlaySfx(gameOverClip);
    }

    public void StopAllAudio()
{
    uiSource.Stop();

    sfxSource.Stop();
    sfxSource.clip = null;

    loopSource.Stop();
    loopSource.clip = null;
}
}
