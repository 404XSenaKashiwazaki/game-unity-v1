using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameVictoryScript : MonoBehaviour
{
    public GameObject GameObjectPanel;
    public GameObject GameObjectNoImagePanel;
    public GameObject GameObjectInfoPanel;
    public TextMeshProUGUI fpvUsed, neutralized;

    void Start()
    {
        if (GameObjectPanel != null)
            GameObjectPanel.SetActive(false);
        
    }

    public void ShowVictory()
    {
        Time.timeScale = 0f;

        int usedDrone = MissionManager.totalUsedDrones;
        int totalDrone = MissionManager.totalDrones;

        int neutralizedTarget = MissionManager.totalDestroyedTargets;
        int totalTarget = MissionManager.totalTargets;

        fpvUsed.text = $"FPV used: {usedDrone} / {totalDrone}";
        neutralized.text = $"neutralized: {neutralizedTarget} / {totalTarget}";

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
        MissionManager.ResetMissionState();
        SceneManager.LoadScene("BriefingScene");
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
