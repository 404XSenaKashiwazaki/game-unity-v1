using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class Options : MonoBehaviour
{
    public GameObject GameObjectPanel;
    public GameObject GameObjectMainMenuPanel;
    public Slider _slider;

    [Header("Audio UI")]
    public Slider musicSlider;
    public Slider sfxSlider;
         public TextMeshProUGUI infoText;
         public Slider enemySlider;

   void OnEnable()
    {
        Debug.Log("OPTIONS PANEL ENABLED");
enemySlider.SetValueWithoutNotify(PlayerPrefs.GetFloat("ENEMY", 2f));
        if (AudioManager.Instance != null)
        {
            musicSlider.SetValueWithoutNotify(
                PlayerPrefs.GetFloat("MUSIC_VOLUME", 1f)
            );

            sfxSlider.SetValueWithoutNotify(
                PlayerPrefs.GetFloat("SFX_VOLUME", 1f)
            );
        }
    }

 public void Apply()
    {

        MissionManager.totalTargets = Mathf.RoundToInt(enemySlider.value);
        PlayerPrefs.SetFloat("ENEMY", enemySlider.value);
        if(infoText != null) infoText.text = $"{Mathf.RoundToInt(enemySlider.value)}";
        PlayerPrefs.Save();
    }


    public void GameMainMenu()
    {
        Debug.Log("VOLUME sfx");
       
        if (GameObjectMainMenuPanel != null)
            GameObjectMainMenuPanel.SetActive(true);

        if (GameObjectPanel != null)
            GameObjectPanel.SetActive(false);
    }

 
    public void GameMusic(float value)
    {
         Debug.Log("VOLUME sfx");
        AudioManager.Instance.SetMusicVolume(value);
    }

    public void GameSfx(float value)
    {
        //  Debug.LogError(_slider.value);
    //    Debug.LogError("SFX VALUE = " + value);
       
         if (AudioManager.Instance == null)
        {
            Debug.LogWarning("AudioManager belum siap");
            return;
        }

        AudioManager.Instance.SetSfxVolume(_slider.value);
    }

    public void GameExit()
    {
        Application.Quit();
    }
}
