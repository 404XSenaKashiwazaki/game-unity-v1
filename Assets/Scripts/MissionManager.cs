using UnityEngine;

public static class MissionManager
{
    public static int currentMission = 1;
    public static int totalMission = 3;
    public static int totalUsedDrones = 0;
    public static int totalDestroyedTargets = 0;
    public static void NextMission()
    {
        currentMission++;
    }

    public static void Reset()
    {
        currentMission = 1;
        totalUsedDrones = 0;
        totalDestroyedTargets = 0;
    }

    public static bool IsLastMission()
    {
        return currentMission >= totalMission;
    }
}


// public static class MissionManager
// {
//     public static int currentMission = 1;
//     public static int totalMission = 3;

//     public static void NextMission()
//     {
//         currentMission++;
//     }

//     public static bool IsLastMission()
//     {
//         return currentMission >= totalMission;
//     }

//     public static void Reset()
//     {
//         currentMission = 1;
//     }
// }
