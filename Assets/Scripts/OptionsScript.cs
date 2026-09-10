using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class OptionsScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public Slider enemySlider;

    public GameObject GameObjectPanel;
    public GameObject GameObjectSettingsPanel, panelOsd,panelNoImage;
    public TextMeshProUGUI infoText,firstButton;
    public Slider _slider;
    private DroneAudio droneAudio;

    
    void OnEnable()
    {
        if(panelOsd != null) panelOsd.SetActive(false); 
        if(panelNoImage != null) panelNoImage.SetActive(false);
        PauseManager.Pause();
        _slider.SetValueWithoutNotify(PlayerPrefs.GetFloat("MASTER_VOLUME", 1f));
    }

    public void Apply()
    {
        MissionManager.totalTargets = Mathf.RoundToInt(enemySlider.value);
        PlayerPrefs.SetFloat("ENEMY", enemySlider.value);
        if(infoText != null) infoText.text = $"{Mathf.RoundToInt(enemySlider.value)}";
        PlayerPrefs.Save();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created


    public void GameMainMenu()
    {
        PauseManager.Resume(); 
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
        if(GameObjectPanel != null) GameObjectPanel.SetActive(false);
    }

    public void GameSfx()
    {
        if (AudioManager.Instance == null)
        {
            // Debug.LogWarning("AudioManager belum siap");
            return;
        }
        AudioManager.Instance.SetVolume(_slider.value);
    }
    public void GameContinue()
    {
        Time.timeScale = 1f;
        PauseManager.Resume();
        if(panelOsd != null) panelOsd.SetActive(true);
        if(panelNoImage != null) panelNoImage.SetActive(true);
        if(GameObjectPanel != null) GameObjectPanel.SetActive(false);

        // SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void GameSettings()
    {
        if(GameObjectPanel != null) GameObjectPanel.SetActive(false);
        if(GameObjectSettingsPanel != null) GameObjectSettingsPanel.SetActive(true);
    }
 public void GameExit()
    {
        Application.Quit();
    }
}
