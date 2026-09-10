using UnityEngine;

public class GameManager : MonoBehaviour
{
    static bool initialized = false;

    void Awake()
    {
        if (initialized)
        {
            Destroy(gameObject);
            return;
        }
        initialized = true;
        MissionManager.totalUsedDrones = 0;
        MissionManager.totalDestroyedTargets = 0;
        DroneController.lastDroneExploded = false;
    }
    public static void ResetForRestart()
    {
          MissionManager.totalUsedDrones = 0;
        MissionManager.totalDestroyedTargets = 0;
        DroneController.lastDroneExploded = false;
    }
}
