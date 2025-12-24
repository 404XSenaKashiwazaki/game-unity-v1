using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class AICarController : MonoBehaviour
{
    public GameObject explosionPrefab;

    public GameObject firePrefab;
    private ParticleSystem[] fireParticles;
    private GameObject fireInstance;

    [Header("Movement")]
    public float speed = 10f;
    public float steerSpeed = 5f;

    [Header("Waypoint System")]
    public Transform[] waypoints;
    public float waypointReachDistance = 3f;

    private int currentWP = 0;
    private Rigidbody rb;

    [Header("State")]
    public CarState state = CarState.Waiting;

    public enum CarState
    {
        Waiting,
        Moving,
        Stopped,
        Destroyed
    }

    [Header("Avoidance")]
    public float followDistance = 7f;
    public float avoidTurnSpeed = 120f;
    public float slowDownMultiplier = 0.7f;

 
    private GameVictoryScript victory;

    [Header("Wheels")]
    public Transform[] wheels;        // drag semua ban ke sini
    public float wheelRotateSpeed = 360f;
    public static Vector3 lastImpactPosition;
    public static Quaternion lastImpactRotation;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();

        
        victory = Object.FindFirstObjectByType<GameVictoryScript>();
    }

    void Explode()
    {
        Debug.Log("EXPLODE DIPANGGIL");
        if (explosionPrefab == null)
    {
        Debug.LogError("Explosion prefab BELUM di-assign!");
        return;
    }

    GameObject exp = Instantiate(
        explosionPrefab,
        transform.position + Vector3.up * 0.5f, // sedikit naik
        Quaternion.identity
    );
    }

    void Start()
    {
        rb.centerOfMass = new Vector3(0, -0.5f, 0);
        rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
        rb.interpolation = RigidbodyInterpolation.Interpolate;
        rb.constraints = RigidbodyConstraints.FreezeRotationX |
                         RigidbodyConstraints.FreezeRotationZ;
    }

    void FixedUpdate()
    {
        if (state != CarState.Moving)
        {
            RotateWheels(); 
            return;
        }
        

        Vector3 rayOrigin = transform.position + Vector3.up * 0.5f;

        if (Physics.SphereCast(rayOrigin, 1.2f, transform.forward,
            out RaycastHit hit, followDistance))
        {
            AICarController frontCar = hit.collider.GetComponent<AICarController>();

            if (frontCar != null && frontCar.state == CarState.Stopped)
            {
                rb.linearVelocity = transform.forward * (speed * slowDownMultiplier);

                rb.MoveRotation(
                    Quaternion.Euler(
                        0,
                        rb.rotation.eulerAngles.y + avoidTurnSpeed * Time.fixedDeltaTime,
                        0
                    )
                );
                RotateWheels();
                return;
            }
        }

        DriveToWaypoint();
        RotateWheels();
    }

    void DriveToWaypoint()
    {
        if (waypoints == null || waypoints.Length == 0) return;

        Transform target = waypoints[currentWP];
        Vector3 toTarget = target.position - rb.position;
        toTarget.y = 0;

        float distance = toTarget.magnitude;
        if (distance < 0.1f) return;

        Vector3 dir = toTarget.normalized;
        float angle = Vector3.SignedAngle(transform.forward, dir, Vector3.up);
        float turnAmount = Mathf.Clamp(angle, -45f, 45f);

        rb.MoveRotation(
            Quaternion.Euler(
                0,
                rb.rotation.eulerAngles.y +
                (turnAmount * steerSpeed * Time.fixedDeltaTime),
                0
            )
        );

        rb.linearVelocity = transform.forward * speed;

        if (distance < waypointReachDistance)
        {
            currentWP++;
            if (currentWP >= waypoints.Length)
                currentWP = 0;
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (state == CarState.Destroyed || state == CarState.Stopped)
            return;

        if (other.collider.GetComponent<AICarController>())
            return;

        if (other.gameObject.name == "Terrain")
            return;

        StopCar();
          PlayFire();
    }

    public void StartMove()
    {
        if (state == CarState.Destroyed) return;

        rb.isKinematic = false;
        rb.linearVelocity = Vector3.zero;
        state = CarState.Moving;
    }

    public void StopCar()
    {
        if (state == CarState.Stopped || state == CarState.Destroyed)
            return;

        state = CarState.Stopped;
        Explode();
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.isKinematic = true;

      
        // if (victory != null)
        //     victory.OnCarDestroyed();
    }

    public bool IsFinished()
    {
        return state == CarState.Stopped || state == CarState.Destroyed;
    }

void PlayFire()
{
   
    if (firePrefab == null || DroneController.isReplaying) return;

    if (fireInstance == null)
    {
        fireInstance = Instantiate(
            firePrefab,
            transform.position,
            Quaternion.identity
        );

    
        fireParticles = fireInstance.GetComponentsInChildren<ParticleSystem>();

        if (fireParticles.Length == 0)
        {
            Debug.LogError("Fire prefab TIDAK punya ParticleSystem!");
            Destroy(fireInstance);
            return;
        }

        float maxLifetime = 60f;

        foreach (var ps in fireParticles)
        {
            var main = ps.main;
            main.loop = false;

            float lifetime =
                main.duration +
                main.startLifetime.constantMax;

            if (lifetime > maxLifetime)
                maxLifetime = lifetime;
        }


        Destroy(fireInstance, maxLifetime);
    }

 
    foreach (var ps in fireParticles)
        ps.Play();
}




    void RotateWheels()
    {
        if (wheels == null || wheels.Length == 0) return;

        float currentSpeed = rb.linearVelocity.magnitude;

        foreach (Transform wheel in wheels)
        {
            if (wheel == null) continue;

 
            wheel.Rotate(
                Vector3.right,
                currentSpeed * wheelRotateSpeed * Time.fixedDeltaTime,
                Space.Self
            );
        }
    }

}
