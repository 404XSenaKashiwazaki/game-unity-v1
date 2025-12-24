using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GroupManager : MonoBehaviour
{
    public CarGroup[] groups;
    public float delayAfterFinish = 2f;

    void Start()
    {
        int index = MissionManager.currentMission - 1;

        // Safety
        if (index < 0 || index >= groups.Length)
        {
            Debug.LogError("Mission index invalid!");
            return;
        }

       
        foreach (var g in groups)
            g.gameObject.SetActive(false);

        // Aktifkan hanya grup sesuai mission
        groups[index].gameObject.SetActive(true);
        groups[index].StartGroup();

        StartCoroutine(WaitGroupFinish(groups[index]));
    }

    IEnumerator WaitGroupFinish(CarGroup group)
    {
        yield return new WaitUntil(() => group.IsGroupFinished());
        yield return new WaitForSeconds(delayAfterFinish);

    
        MissionManager.totalUsedDrones += DroneController.usedDrones;
        MissionManager.totalDestroyedTargets += DroneController.destroyedTargets;

        if (MissionManager.IsLastMission())
        {
            FindFirstObjectByType<GameVictoryScript>().ShowVictory();
        }
        else
        {
            MissionManager.NextMission();
            SceneManager.LoadScene("BriefingScene");
        }
    }
}


// using UnityEngine;
// using System.Collections;

// public class GroupManager : MonoBehaviour
// {
//     public CarGroup[] groups;
//     public float delayNextGroup = 50f;

//     void Start()
//     {
//         StartCoroutine(RunScenario());
//     }

//     IEnumerator RunScenario()
//     {
//         for (int i = 0; i < groups.Length; i++)
//         {
//             groups[i].StartGroup();

//             yield return new WaitUntil(() =>
//                 groups[i].IsGroupFinished()
//             );

//             yield return new WaitForSeconds(delayNextGroup);
//         }
//     }
// }
