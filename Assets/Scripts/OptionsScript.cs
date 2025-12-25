using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class OptionsScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

 public Slider enemySlider;

    public GameObject GameObjectPanel;
    public GameObject GameObjectSettingsPanel;
     public TextMeshProUGUI infoText;


 void OnEnable()
    {
      enemySlider.SetValueWithoutNotify(PlayerPrefs.GetFloat("ENEMY", 2f));
    }

     public void Apply()
    {

        MissionManager.totalTargets = Mathf.RoundToInt(enemySlider.value);
        PlayerPrefs.SetFloat("ENEMY", enemySlider.value);
        if(infoText != null) infoText.text = $"{Mathf.RoundToInt(enemySlider.value)}";
        PlayerPrefs.Save();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Update() {
        Time.timeScale = 0;
   
    }

    public void GameMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
        if(GameObjectPanel != null) GameObjectPanel.SetActive(false);
    }

    public void GameSfx()
    {
         Debug.Log("Slider value realtime = " );
    }
    public void GameContinue()
    {
        Time.timeScale = 1f;
        if(GameObjectPanel != null) GameObjectPanel.SetActive(false);
        // SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void GameSettings()
    {
        if(GameObjectPanel != null) GameObjectPanel.SetActive(false);
        if(GameObjectSettingsPanel != null) GameObjectSettingsPanel.SetActive(true);
    }

}
