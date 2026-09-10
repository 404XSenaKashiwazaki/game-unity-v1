using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.EventSystems;

public class GameOverScript : MonoBehaviour
{
    public GameObject GameObjectPanel;
    public GameObject GameObjectNoImagePanel, firstButton;
    public TextMeshProUGUI fpvUsed, neutralized;
    public GameObject GameObjectInfoPanel,panelOsd,panelNoImage;


    bool shown = false;


    void Update()
    {
        if (MissionManager.isMissionComplete) return;

        // if (MissionManager.isGameOver && !shown)
        // {
        //     ShowGameOver();
        // }
    }

   public void ShowGameOver()
    {
       if (GameObjectInfoPanel != null)
            GameObjectInfoPanel.SetActive(false);
        if (GameObjectNoImagePanel != null)
            GameObjectNoImagePanel.SetActive(false);
        AudioManager.Instance.PlayGameOver();

        shown = true;

        Time.timeScale = 0f;

        int usedDrone = MissionManager.totalUsedDrones;
        int totalDrone = MissionManager.totalDrones;

    
         int gates = MissionManager.CurrentGates;
        int totalGates = MissionManager.TotalGates;

        if (fpvUsed != null)
            fpvUsed.text = $"crashes: {usedDrone} / {totalDrone}";

        if (neutralized != null)
        
        neutralized.text = $"checkpoints passed: {gates} / {totalGates}";

        if (GameObjectPanel != null)
            GameObjectPanel.SetActive(true);

    
    }

    public void GameRestart()
    {
        Time.timeScale = 1f;
        AudioManager.Instance.StopAllAudio();
        DroneController drone = FindAnyObjectByType<DroneController>();
        if (drone != null)
        {
             drone.isEngineOn = false;
            drone.StopAllCoroutines();
           drone.HideDrone();
        }
        
        MissionManager.ResetMissionState();
      
        DroneController.raceTimer = 0f;

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void MainMenu()
    {
        Time.timeScale = 1f;
        DroneController drone = FindAnyObjectByType<DroneController>();
    if (drone != null)
    {
        drone.isEngineOn = false;
        drone.StopAllCoroutines();
        drone.HideDrone();
    }

        MissionManager.ResetMissionState();
        SceneManager.LoadScene("MainMenu");
    }

    public void GameExit()
    {
        Application.Quit();
    }
}
