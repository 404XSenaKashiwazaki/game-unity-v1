using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class Options : MonoBehaviour
{
    public GameObject GameObjectPanel;
    public GameObject GameObjectMainMenuPanel;
    public GameObject GameObjectOptionsPanel;
    public Slider _slider;
    [Header("Audio UI")]
    public Slider musicSlider;

    public TextMeshProUGUI infoText;
    public Slider enemySlider;

    void OnEnable()
    {

        if (AudioManager.Instance != null)
        {
            musicSlider.SetValueWithoutNotify(
                PlayerPrefs.GetFloat("MUSIC_VOLUME", 1f)
            );

            _slider.SetValueWithoutNotify(
                PlayerPrefs.GetFloat("MASTER_VOLUME", 1f)
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
        if (GameObjectMainMenuPanel != null)
            GameObjectMainMenuPanel.SetActive(true);
        if (GameObjectPanel != null)
            GameObjectPanel.SetActive(false);
    }

    public void GameOptions ()
    {
        if(GameObjectOptionsPanel != null) GameObjectOptionsPanel.SetActive(true);
        if(GameObjectPanel != null) GameObjectPanel.SetActive(false);
    }
 
    public void GameMusic(float value)
    {
        
    }

    public void GameSfx(float value)
    {
 
        if (AudioManager.Instance == null)
        {
            // Debug.LogWarning("AudioManager belum siap");
            return;
        }
        AudioManager.Instance.SetVolume(_slider.value);
    }

    public void GameExit()
    {
        Application.Quit();
    }
}
