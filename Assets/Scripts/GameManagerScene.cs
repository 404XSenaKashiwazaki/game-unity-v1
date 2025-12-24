using UnityEngine;

public class GameManagerScene : MonoBehaviour
{
    void Awake()
    {
        DroneController.usedDrones = 0;
        DroneController.destroyedTargets = 0;
        DroneController.isReplaying = false;
    }
}
