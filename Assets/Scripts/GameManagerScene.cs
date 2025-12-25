using UnityEngine;

public class GameManagerScene : MonoBehaviour
{
    void Awake()
    {
        MissionManager.totalUsedDrones = 0;
        MissionManager.totalDestroyedTargets = 0;
        DroneController.isReplaying = false;
    }
}
