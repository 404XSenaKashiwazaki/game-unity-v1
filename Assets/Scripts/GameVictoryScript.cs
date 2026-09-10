using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.EventSystems;

public class GameVictoryScript : MonoBehaviour
{
    public GameObject GameObjectPanel;
    public GameObject GameObjectNoImagePanel, firstButton;
    public GameObject GameObjectInfoPanel;
    public TextMeshProUGUI fpvUsed, neutralized, timerText, BestTime;



    string FormatTime(float time)
    {
        if (time <= 0f)
            return "--:--.--";

        int minutes = Mathf.FloorToInt(time / 60);
        int seconds = Mathf.FloorToInt(time % 60);
        int centiseconds = Mathf.FloorToInt((time - Mathf.Floor(time)) * 100);

        return $"{minutes:00}:{seconds:00}.{centiseconds:00}";
    }

    public void ShowVictory()
    {
        //  Debug.Log("ZINEMA ");

        AudioManager.Instance.PlayVictory();
        
        Time.timeScale = 0f;

        int usedDrone = MissionManager.totalUsedDrones;
        int totalDrone = MissionManager.totalDrones;

        int gates = MissionManager.CurrentGates;
        int totalGates = MissionManager.TotalGates;

        fpvUsed.text = $"crashes: {usedDrone} / {totalDrone}";
        neutralized.text = $"checkpoints passed: {gates} / {totalGates}";

         // Level 2 Best Time
        if (Level.CurrentLevel == 2)
        {
            float currentTime = DroneController.raceTimer;
            float bestTime = PlayerPrefs.GetFloat("BestTimeLevel2", 0f);
            //last time
            PlayerPrefs.SetFloat("LastTimeLevel2", currentTime);
            if (bestTime == 0 || currentTime < bestTime)
            {
                bestTime = currentTime;

                PlayerPrefs.SetFloat("BestTimeLevel2", bestTime);
                
                
            }
            PlayerPrefs.Save();
            timerText.text =
                $"TIME: {FormatTime(currentTime)}s\nBEST TIME: {FormatTime(bestTime)}s";
            BestTime.text =  $"TIME: {FormatTime(currentTime)}s\nBEST TIME: {FormatTime(bestTime)}s";
        }
 else
    {
        // Level 1 (atau level lain)
        BestTime.text = "";
    }
        

        if (GameObjectPanel != null)
            GameObjectPanel.SetActive(true);

        if (GameObjectNoImagePanel != null)
            GameObjectNoImagePanel.SetActive(false);
        if (GameObjectInfoPanel != null)
            GameObjectInfoPanel.SetActive(false);
    }


    // public void ShowVictory()
    // {
    //     // PAUSE GAME (AMAN)
    //     Time.timeScale = 0f;

    //     int usedDrone = DroneController.usedDrones;
    //     int totalDrone = DroneController.totalDrones;

    //     int neutralizedTarget = DroneController.destroyedTargets;
    //     int totalTarget = DroneController.totalTargets;

    //     fpvUsed.text = $"FPV used: {usedDrone} / {totalDrone}";
    //     neutralized.text = $"neutralized: {neutralizedTarget} / {totalTarget}";

    //     if (GameObjectPanel != null)
    //         GameObjectPanel.SetActive(true);

    //     if (GameObjectNoImagePanel != null)
    //         GameObjectNoImagePanel.SetActive(false);
    // }

    public void GameTry()
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

    AudioManager.Instance.StopAllAudio();

    MissionManager.ResetMissionState();
    SceneManager.LoadScene("MainMenu");
    }

    public void GameExit()
    {
        Application.Quit();
    }
}


// using UnityEngine;
// using UnityEngine.SceneManagement;
// using System.Collections;
// using TMPro;

// public class GameVictoryScript : MonoBehaviour
// {
//     public GameObject GameObjectPanel;
//     public GameObject GameObjectNoImagePanel;
//     public DroneController drone;
//     public TextMeshProUGUI fpvUsed,neutralized;

//     [Header("Victory Delay")]
//     public float victoryDelay = 2f; // ⏱️ delay sebelum victory muncul

//     private int aliveCars;
//     private bool victoryShown = false;
//     int usedDrone;
//     int totalDrone;
//     int neutralizedTarget;
//     int totalTarget;




//     void Start()
//     {
//         aliveCars = Object.FindObjectsByType<AICarController>(
//             FindObjectsSortMode.None
//         ).Length;

//         if (GameObjectPanel != null)
//             GameObjectPanel.SetActive(false);
//     }

//     public void OnCarDestroyed()
//     {
//         if (victoryShown) return;
// Debug.Log(aliveCars);
//         aliveCars--;

//         if (aliveCars <= 0)
//         {
//             victoryShown = true;
//             StartCoroutine(VictoryDelayCoroutine()); // 🔥 delay di sini
//         }
//     }

//     IEnumerator VictoryDelayCoroutine()
//     {
//         // tunggu dulu (game masih jalan)
//         yield return new WaitForSeconds(victoryDelay);

//         // tampilkan victory
//         Time.timeScale = 0f;
//         usedDrone = DroneController.usedDrones;
//         totalDrone = DroneController.totalDrones;

//         neutralizedTarget = DroneController.destroyedTargets;
//         totalTarget = DroneController.totalTargets;

//         fpvUsed.text = $"FPV used: {usedDrone} / {totalDrone}";
//         neutralized.text = $"neutralized: {neutralizedTarget} / {totalTarget}";

//         if (GameObjectPanel != null)
//             GameObjectPanel.SetActive(true);

//         if (GameObjectNoImagePanel != null)
//             GameObjectNoImagePanel.SetActive(false);
//     }

//     public void GameTry()
//     {
//         Time.timeScale = 1f;
//         GameManager.ResetMissionStateForRestart();
//         SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
//     }

//     public void MainMenu()
//     {
//         Time.timeScale = 1f;
//         SceneManager.LoadScene("MainMenu");
//     }

//     public void GameExit()
//     {
//         Application.Quit();
//     }
// }
