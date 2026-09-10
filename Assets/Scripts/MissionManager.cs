using Unity.VisualScripting;
using UnityEngine;

public static class MissionManager
{
    public static int CurrentGates = 0;
    
    public static int TotalGates =  6;   
    public static int currentMission = 1;
    public static int totalMission = 3;

    public static int totalUsedDrones = 0;
    public static int totalDrones = 12;

    public static int totalDestroyedTargets = 0;
    public static int totalTargets = 2;

    public static bool isGameOver = false;
    public static bool isMissionComplete = false;

    public static void NextMission()
    {
        currentMission++;
        ResetMissionState();
    }

    public static void SetGatePassed()
{
    CurrentGates++;

    // Debug.Log($"SetGatePassed | Current = {CurrentGates} | Frame = {Time.frameCount}");
    // Debug.Log(System.Environment.StackTrace);

    if (CurrentGates >= TotalGates)
    {
        isMissionComplete = true;
    }
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
            //  Debug.Log("IT'S ZOVER");
            return;
        }
        // Debug.Log("CEK " + (totalUsedDrones >= totalDrones && !other.gameObject.CompareTag("Enemy")));
        if (totalUsedDrones >= totalDrones && !other.gameObject.CompareTag("Enemy"))
        {
            isGameOver = true;
            isMissionComplete = false;

        }
    }

    public static void ResetMissionState()
    {
        CurrentGates = 0;
        totalUsedDrones = 0;
        totalDestroyedTargets = 0;
        isGameOver = false;
        isMissionComplete = false;
    }
}
