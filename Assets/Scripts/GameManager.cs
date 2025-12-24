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

        DroneController.usedDrones = 0;
        DroneController.destroyedTargets = 0;
        DroneController.lastDroneExploded = false;
    }

     public static void ResetForRestart()
    {
         DroneController.destroyedTargets = 0;
         DroneController.usedDrones = 0;
        DroneController.lastDroneExploded = false;
    }
}
