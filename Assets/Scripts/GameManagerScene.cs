using UnityEngine;

public class GameManagerScene : MonoBehaviour
{
    // public GameObject checkpointLevel1;
    public GameObject checkpointLevel2;

    void Awake()
    {
        MissionManager.totalUsedDrones = 0;
        MissionManager.totalDestroyedTargets = 0;
        DroneController.isReplaying = false;

        // checkpointLevel1.SetActive(
        //     Level.CurrentLevel == 1);

        // checkpointLevel2.SetActive(
        //     Level.CurrentLevel == 2);
    }
}