using UnityEngine;

public class GateRaceManager : MonoBehaviour
{
    int currentGate = 0;

    void Start()
    {
        UpdateBeacons();
    }

    public void NextGate()
    {
        currentGate++;
        UpdateBeacons();
    }

    void UpdateBeacons()
    {
        for(int i = 0; i < transform.childCount; i++)
        {
            Transform gate = transform.GetChild(i);

            Transform beacon = gate.Find("Cylinder");

            if(beacon != null)
                beacon.gameObject.SetActive(i == currentGate);
        }
    }
}