// using UnityEngine;

// [RequireComponent(typeof(LineRenderer))]
// public class PathLine : MonoBehaviour
// {
//     public Transform drone;

//     private LineRenderer lr;

//     void Start()
//     {
//         lr = GetComponent<LineRenderer>();
//         lr.positionCount = 2;
//     }

//     void Update()
//     {
//         if (DroneController.targetGate == null)
//         {
//             lr.enabled = false;
//             return;
//         }

//         lr.enabled = true;

//         lr.SetPosition(0, drone.position);
//         lr.SetPosition(1, DroneController.targetGate.position);
//     }
// }

using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class CurvedPath : MonoBehaviour
{
    public Transform drone;

    public Transform checkpointLevel1;
    public Transform checkpointLevel2;

    private List<Transform> gates = new();

    private LineRenderer line;

    public int currentGate = 0;
    public int segments = 20;
    public float curveHeight = 3f;
    public DroneController droneController;

    void Start()
    {
        line = GetComponent<LineRenderer>();

        gates.Clear();

        if (Level.CurrentLevel == 1)
        {
            foreach (Transform gate in checkpointLevel1)
            {
                gates.Add(gate);
            }
        }
        else if (Level.CurrentLevel == 2)
        {
            foreach (Transform group in checkpointLevel2)
            {
                foreach (Transform gate in group)
                {
                    gates.Add(gate);
                }
            }
        }

        //gate respawn 
    }

    void Update()
    {
        if (currentGate >= gates.Count)
            return;

        Transform target = gates[currentGate];

        line.positionCount = segments;

        for (int i = 0; i < segments; i++)
        {
            float t = i / (float)(segments - 1);

            Vector3 startPos =
                drone.position +
                drone.forward * 2f;

            Vector3 p = Vector3.Lerp(
                startPos,
                target.position,
                t
            );

            p.y += Mathf.Sin(t * Mathf.PI) * curveHeight;

            line.SetPosition(i, p);
        }
    }

    public void NextGate()
    {
        currentGate++;
    }
}