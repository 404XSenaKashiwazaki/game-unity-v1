using UnityEngine;

public class CarsController : MonoBehaviour
{
    public float speed = 10f;
    private Rigidbody rb;
    bool isHit = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        if (rb == null)
        {
            Debug.LogError("Rigidbody tidak ditemukan!");
            return;
        }

        // Biar mobil ga jungkir balik
        rb.centerOfMass = new Vector3(0, -0.5f, 0);

        // BIAR COLLISION SELALU TERDETEKSI
        rb.collisionDetectionMode = CollisionDetectionMode.Continuous;

        // Biar mobil tetap stabil, tapi bisa tabrakan normal
        rb.constraints = RigidbodyConstraints.FreezeRotationX |
                         RigidbodyConstraints.FreezeRotationZ;
    }

    void FixedUpdate()
    {
        if (isHit || rb == null) return;

        // Gerak stabil
        rb.MovePosition(rb.position + transform.forward * speed * Time.fixedDeltaTime);
    }

    private void OnCollisionEnter(Collision other)
    {
        Debug.Log("TABRAK: " + other.gameObject.name);

        if (isHit) return;
        if (other.gameObject.name == "Terrain") return;
        if (rb == null) return;

        // Stop mobil ✅ FIX
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        rb.isKinematic = true;

        isHit = true;
    }
}
