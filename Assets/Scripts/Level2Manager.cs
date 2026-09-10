using UnityEngine;

public class Level2Manager: MonoBehaviour
{
    public GameObject[] gateLayouts;
    public static Transform CurrentSpawnMarker;

    void Start()
    {
       foreach (GameObject g in gateLayouts)
            g.SetActive(false);

        int indexRandom = Random.Range(0, gateLayouts.Length);
        // Debug.Log("RANDOM : "+indexRandom);
        
        gateLayouts[indexRandom].SetActive(true);

        Transform firstGate =
            gateLayouts[indexRandom].transform.GetChild(0);

        DroneController.targetGate = firstGate;
        DroneController drone =
        FindAnyObjectByType<DroneController>();

        if (drone != null && drone.respawnPrefab != null)
        {
            Transform spawnMarker =
                firstGate.parent.Find("SpawnMarker");

            if (spawnMarker != null)
            {
              
                CurrentSpawnMarker = spawnMarker;
                drone.respawnPrefab.transform.position =
                    spawnMarker.position;

                drone.respawnPrefab.transform.rotation =
                    spawnMarker.rotation;

                drone.transform.position =
                    spawnMarker.position;

                    drone.transform.rotation =
                spawnMarker.rotation * Quaternion.Euler(0, 90, 0);
                
            }
        }

        // MissionManager.TotalGates = 10;
    }
}