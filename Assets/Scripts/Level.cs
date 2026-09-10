using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
public class Level : MonoBehaviour
{
    public GameObject GameObjectPanel, PanelControls;
    public static int CurrentLevel = 1;
    public static string CurrentScene;
    public GameObject firstButton;
    public TMP_Text RecordText;



    string FormatTime(float time)
    {
        if (time <= 0f)
            return "--:--.--";

        int minutes = Mathf.FloorToInt(time / 60);
        int seconds = Mathf.FloorToInt(time % 60);
        int centiseconds = Mathf.FloorToInt((time - Mathf.Floor(time)) * 100);

        return $"{minutes:00}:{seconds:00}.{centiseconds:00}";
    }
    public void Start()
    {
        if(PanelControls != null) PanelControls.SetActive(false);
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(firstButton);
        float lastTime = PlayerPrefs.GetFloat("LastTimeLevel2", 0f);
        float bestTime = PlayerPrefs.GetFloat("BestTimeLevel2", 0f);

        RecordText.text =
        $"BEST TIME : {FormatTime(bestTime)}\n" +
        $"LAST TIME : {FormatTime(lastTime)}";
    }

    public void ControlsPanel ()
    {
        if(GameObjectPanel != null) GameObjectPanel.SetActive(false);
        if(PanelControls != null) PanelControls.SetActive(true);
    }

    public void Level1()
    {
        MissionManager.ResetMissionState();
        Time.timeScale = 1f;

        CurrentLevel = 1;
        MissionManager.TotalGates = 6;
        // SceneManager.LoadScene("Level1");
        LoadingScreen.SceneToLoad = "Level1";
        SceneManager.LoadScene("LoadingScene");
    }

    public void Level2()
    {
        MissionManager.ResetMissionState();
        Time.timeScale = 1f;

        CurrentLevel = 2;
        MissionManager.TotalGates = 6;
        DroneController.raceTimer = 0f; // reset timer
    
        LoadingScreen.SceneToLoad = "Level2";
        SceneManager.LoadScene("LoadingScene");
    }

    public void Level3()
    {

        MissionManager.ResetMissionState();
        Time.timeScale = 1f;

        CurrentLevel = 3;
        SceneManager.LoadScene("GameDrone");
    }

    public void Menu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void Shop ()
    {
        SceneManager.LoadScene("Shop");
    }
}
