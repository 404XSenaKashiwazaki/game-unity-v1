using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameOverScript : MonoBehaviour
{
    public GameObject GameObjectPanel;
    public GameObject GameObjectNoImagePanel;
    public TextMeshProUGUI fpvUsed, neutralized;
    public GameObject GameObjectInfoPanel;

    bool shown = false;

    void Update()
    {
        if (MissionManager.isMissionComplete) return;

        if (MissionManager.isGameOver && !shown)
        {
            ShowGameOver();
        }
    }

   public void ShowGameOver()
    {

        shown = true;

        Time.timeScale = 0f;

        int usedDrone = MissionManager.totalUsedDrones;
        int totalDrone = MissionManager.totalDrones;

        int neutralizedTarget = MissionManager.totalDestroyedTargets;
        int totalTarget = MissionManager.totalTargets; // ini boleh tetap dari DroneController

        if (fpvUsed != null)
            fpvUsed.text = $"FPV used: {usedDrone} / {totalDrone}";

        if (neutralized != null)
            neutralized.text = $"neutralized: {neutralizedTarget} / {totalTarget}";

        if (GameObjectPanel != null)
            GameObjectPanel.SetActive(true);

        if (GameObjectInfoPanel != null)
            GameObjectInfoPanel.SetActive(false);
            Debug.Log(GameObjectNoImagePanel);
        if (GameObjectNoImagePanel != null)
            GameObjectNoImagePanel.SetActive(false);
         
    }

    // ================= BUTTON =================

    public void GameRestart()
    {
        Time.timeScale = 1f;
        MissionManager.ResetMissionState();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void MainMenu()
    {
        Time.timeScale = 1f;
        MissionManager.ResetMissionState();
        SceneManager.LoadScene("MainMenu");
    }

    public void GameExit()
    {
        Application.Quit();
    }
}
