using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MissionBriefingSceneController : MonoBehaviour
{
    public TextMeshProUGUI briefingText;
    public string gameplaySceneName = "GameScene";

    void Start()
    {
        int usedDrone = MissionManager.totalUsedDrones;
        int totalDrone = DroneController.totalDrones;
        int neutralizedTarget = MissionManager.totalDestroyedTargets;
        int totalTarget = DroneController.totalTargets;
        briefingText.text =
            $"MISSION: {MissionManager.currentMission}\n\n\n" +
            $"pv used: {usedDrone}/{totalDrone}\n\nneutralized: {neutralizedTarget}/{totalTarget}\n\n\n"+
            "OBJECTIVE:\n\n\n" +
            "- HANCURKAN SEMUA KENDARAAN MUSUH\n\n" +
            "- JANGAN BIARKAN MENEMBUS ZONA OPERATOR\n\n\n";
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            SceneManager.LoadScene(gameplaySceneName);
        }
    }


    public void GamePlay()
    {

        SceneManager.LoadScene(gameplaySceneName);
    
    }

}
