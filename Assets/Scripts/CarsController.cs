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
    
            return;
        }

        rb.centerOfMass = new Vector3(0, -0.5f, 0);

      
        rb.collisionDetectionMode = CollisionDetectionMode.Continuous;

        
        rb.constraints = RigidbodyConstraints.FreezeRotationX |
                         RigidbodyConstraints.FreezeRotationZ;
    }

    void FixedUpdate()
    {
        if (isHit || rb == null) return;

 
        rb.MovePosition(rb.position + transform.forward * speed * Time.fixedDeltaTime);
    }

    private void OnCollisionEnter(Collision other)
    {
        

        if (isHit) return;
        if (other.gameObject.name == "Terrain") return;
        if (rb == null) return;

  
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        rb.isKinematic = true;

        isHit = true;
    }
}
