using UnityEngine;

public class ArtilleryWaypoint : MonoBehaviour
{
    public GameObject smokePrefab;
    public float smokeDuration = 10f;

    [Header("Smoke Transform")]
    public float yOffset = 2.059999f;
    public Vector3 smokeRotation = new Vector3(-103.641f, 0f, 0f);

    void Start()
    {
        Fire();
    }

    public void Fire()
    {
        if (smokePrefab == null)
        {
            // Debug.LogError("SMOKE PREFAB NULL on " + name);
            return;
        }

        // Vector3 pos = transform.position;
        // pos.y += yOffset;

        // Quaternion rot = Quaternion.Euler(smokeRotation);

        // GameObject smoke = Instantiate(smokePrefab, pos, rot);
        // Destroy(smoke, smokeDuration);
    }
}
