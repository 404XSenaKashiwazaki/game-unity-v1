using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameOverScript : MonoBehaviour
{
    public GameObject GameObjectPanel;
     public DroneController drone;
    public TextMeshProUGUI fpvUsed,neutralized;
    
    int usedDrone;
    int totalDrone;
    int neutralizedTarget;
    int totalTarget;


 

    public void GameOver()
    {
        Time.timeScale = 0f;

        usedDrone = DroneController.usedDrones;
        totalDrone = DroneController.totalDrones;

        neutralizedTarget = DroneController.destroyedTargets;
        totalTarget = DroneController.totalTargets;

        fpvUsed.text = $"FPV used: {usedDrone} / {totalDrone}";
        neutralized.text = $"neutralized: {neutralizedTarget} / {totalTarget}";

        if (GameObjectPanel != null)
            GameObjectPanel.SetActive(true);
    }


    public void GameRestart()
    {
        Time.timeScale = 1f;
        GameManager.ResetForRestart();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void MainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }

    public void GameExit()
    {
        Application.Quit();
    }


}
