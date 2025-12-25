using UnityEngine;

public static class MissionManager
{
    public static int currentMission = 1;
    public static int totalMission = 3;

    public static int totalUsedDrones = 0;
    public static int totalDrones = 4;

    public static int totalDestroyedTargets = 0;
    public static int totalTargets = 2;

    public static bool isGameOver = false;
    public static bool isMissionComplete = false;

    public static void NextMission()
    {
        currentMission++;
    }

    public static bool IsLastMission()
    {
        return currentMission >= totalMission;
    }

    public static void SetUsedDrone()
    {
        totalUsedDrones++;
    }

    public static void SetDestroyedTarget()
    {
        totalDestroyedTargets++;
    }

    public static void CheckMissionState(Collision other)
    {

        if (isMissionComplete || isGameOver) return;

      
        if (totalDestroyedTargets >= totalTargets)
        {
            isMissionComplete = true;
            isGameOver = false;
            return;
        }

        if (totalUsedDrones >= totalDrones && !other.gameObject.CompareTag("Enemy"))
        {
       
            isGameOver = true;
            isMissionComplete = false;
            Debug.Log("GAME OVER - DRONE HABIS");

            GameOverScript over =
                UnityEngine.Object.FindAnyObjectByType<GameOverScript>();

            if (over != null)
                over.ShowGameOver();
        }
    }

    public static void ResetMissionState()
    {
        totalUsedDrones = 0;
        totalDestroyedTargets = 0;
        isGameOver = false;
        isMissionComplete = false;
    }
}
