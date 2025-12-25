 using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;


[RequireComponent(typeof(Rigidbody))]
public class DroneController : MonoBehaviour
{
    [Header("References")]
    public Transform propFL, propFR, propBL, propBR;

    public GameObject canvasNoImage;
    public GameObject respawnPrefab;
    public GameObject respawn;
    public GameObject GameObjectOptionsPanel;
    public GameObject GameObjectPausedPanel;



    private Rigidbody rb;
    private Rigidbody[] cachedRBs;

    [Header("Settings")]
    public float liftForce = 7f;
    public float forwardSpeed = 15f;
    public float strafeSpeed = 15f;
    public float yawSpeed = 80f;

    [Header("FPV Tilt Control")]
    public float maxTiltAngle = 180f;
    public float tiltSpeed = 5f;
    public float tiltReturnSpeed = 4f;

    public float rollStabilizer = 8f;
    public float rollDamping = 2f;
    public float propSpinSpeed = 2000f;

    float targetPitch, targetRoll;
    float pitchInput, rollInput, yawInput, throttle, forwardInput;
    float smoothRoll, smoothPitch;

    bool isCrashed = false;
    public bool isGameOver = false;
    bool isEngineOn = false;

    [Header("OSD DISPLAY")]
    public TextMeshProUGUI fpsText, leftTopText, leftText, topText, infoText,killText, usedDroneText;

    public float maxCurrent = 80f;
    public float batteryCapacity = 6000;
    public int cellCount = 6;

    float usedmAh = 0f;
    float batteryVoltage = 25.2f;
    float altitude = 0f;
    float cellVoltage;
    float current;
    float roll, pitch, yaw;

    public float hoverCurrentPercent = 0.35f;
    public float respawnDelay = 2f;

    int battery = 100;
    int fpsRandom = 30;

    float timer = 0f;
    float timerBattery = 0f;
    float altTimer = 0f;

    float currentYaw = 0f;

    [Header("Kamikaze Explosion")]
    public GameObject explosionPrefab;
    public bool isLastDrone = false; // set dari GameManager

    public static Vector3 lastImpactPosition;
    public static Quaternion lastImpactRotation;
    public static bool lastDroneExploded = false;
    public GameOverScript gameOverScript;


    public static bool isReplaying = false;

     //
    
     struct ReplayFrame
    {
        public Vector3 pos;
        public Quaternion rot;
    }

    List<ReplayFrame> replayBuffer = new List<ReplayFrame>();
    [Header("Replay")]
    public float replayFrameDelay = 0.05f;
    public float replayDuration = 3f;
    [Header("Partikel")]
    private ParticleSystem explosionPS;


    void Start()
    {
        rb = GetComponent<Rigidbody>();
        cachedRBs = FindObjectsByType<Rigidbody>(FindObjectsSortMode.None);

        currentYaw = transform.eulerAngles.y;

        isEngineOn = false;       
        rb.isKinematic = true;    

        if (respawn == null && respawnPrefab != null)
            respawn = Instantiate(respawnPrefab, transform.position, Quaternion.identity);

        rb.angularDamping = 0.4f;
        rb.linearDamping = 0.4f;
        
        // ShowNoImageTrue();        // "Press Enter"
    }


    void Update()
    {

        if (isReplaying) return;
        timer += Time.deltaTime;
        timerBattery += Time.deltaTime;

        if (timer >= 1f && isEngineOn)
        {
            fpsRandom = Random.Range(29, 35);
            timer = 0f;
        }

        if (battery > 0 && timerBattery >= 4f && isEngineOn)
        {
            battery--;
            timerBattery = 0f;
        }
        if(battery == 0 && MissionManager.totalUsedDrones <= MissionManager.totalDrones) { if (respawn != null) { setRespawn();} }
       
        if (Input.GetKeyDown(KeyCode.Escape)) 
            if (GameObjectPausedPanel != null) GameObjectPausedPanel.SetActive(true);

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (GameObjectOptionsPanel != null) GameObjectOptionsPanel.SetActive(true);
            
        }
            
        if (Input.GetKeyDown(KeyCode.Return) && !isEngineOn)
        {
            isEngineOn = true;
            rb.isKinematic = false;
            ShowNoImageFalse();
        }
 
    }

    void FixedUpdate()
    {
        if (isCrashed || !isEngineOn || isReplaying) return;

        throttle = 0f;
        if (Input.GetKey(KeyCode.Space))
        {
            throttle += 3f;
            rb.isKinematic = false;
            ShowNoImageFalse();
        }
        if (Input.GetKey(KeyCode.LeftControl)) throttle -= 3f;

        pitchInput = (Input.GetKey(KeyCode.UpArrow) ? 1 : 0) -
                     (Input.GetKey(KeyCode.DownArrow) ? 1 : 0);

        rollInput = (Input.GetKey(KeyCode.D) ? 1 : 0) -
                    (Input.GetKey(KeyCode.A) ? 1 : 0);

        yawInput = (Input.GetKey(KeyCode.RightArrow) ? 1 : 0) -
                   (Input.GetKey(KeyCode.LeftArrow) ? 1 : 0);

        forwardInput = (Input.GetKey(KeyCode.W) ? 1 : 0) -
                       (Input.GetKey(KeyCode.S) ? 1 : 0);

        smoothRoll = Mathf.Lerp(smoothRoll, rollInput, Time.fixedDeltaTime * 6f);
        smoothPitch = Mathf.Lerp(smoothPitch, pitchInput, Time.fixedDeltaTime * 6f);

        float hover = rb.mass * Physics.gravity.magnitude;
        float upright = Mathf.Clamp01(Vector3.Dot(transform.up, Vector3.up));
        float effectiveLift = (hover + liftForce * throttle) / Mathf.Max(0.5f, upright);

        rb.AddForce(Vector3.up * effectiveLift, ForceMode.Force);

        float wantedPitch = smoothPitch * maxTiltAngle;
        float wantedRoll = smoothRoll * maxTiltAngle;

        targetPitch = Mathf.Lerp(targetPitch, wantedPitch, Time.fixedDeltaTime * tiltSpeed);
        targetRoll = Mathf.Lerp(targetRoll, wantedRoll, Time.fixedDeltaTime * tiltSpeed);

        if (Mathf.Abs(pitchInput) < 0.05f)
            targetPitch = Mathf.Lerp(targetPitch, 0f, Time.fixedDeltaTime * tiltReturnSpeed);

        if (Mathf.Abs(rollInput) < 0.05f)
            targetRoll = Mathf.Lerp(targetRoll, 0f, Time.fixedDeltaTime * tiltReturnSpeed);

        currentYaw += yawInput * yawSpeed * Time.fixedDeltaTime;

        Quaternion yawRot = Quaternion.Euler(0f, currentYaw, 0f);
        Quaternion tilt = Quaternion.Euler(targetPitch, 0f, -targetRoll);

        transform.rotation = Quaternion.Slerp(transform.rotation, yawRot * tilt, Time.fixedDeltaTime * 8f);

        rb.AddForce(transform.forward * forwardSpeed * smoothPitch, ForceMode.Force);
        rb.AddForce(transform.forward * forwardSpeed * forwardInput, ForceMode.Force);
        rb.AddForce(transform.right * strafeSpeed * smoothRoll, ForceMode.Force);

        SpinProp(propFL, false);
        SpinProp(propFR, true);
        SpinProp(propBL, true);
        SpinProp(propBR, false);

        // current draw sim
        float controlLoad =
            Mathf.Abs(smoothRoll) +
            Mathf.Abs(smoothPitch) +
            Mathf.Abs(forwardInput) +
            Mathf.Clamp01(Mathf.Abs(throttle));

        controlLoad = Mathf.Clamp01(controlLoad);

        float baseCurrent = maxCurrent * hoverCurrentPercent;
        float extraCurrent = controlLoad * (maxCurrent * (1f - hoverCurrentPercent));
        current = baseCurrent + extraCurrent;

        usedmAh += current * Time.deltaTime * (1f / 3600f) * 1000f;

        float percent = Mathf.Clamp01(1f - (usedmAh / batteryCapacity));
        float maxV = 4.2f * cellCount;
        float minV = 3.3f * cellCount;
        batteryVoltage = Mathf.Lerp(minV, maxV, percent);
        cellVoltage = batteryVoltage / cellCount;

        roll = NormalizeAngle(transform.eulerAngles.z);
        pitch = NormalizeAngle(transform.eulerAngles.x);
        yaw = NormalizeAngle(transform.eulerAngles.y);

        // raycast altitude 
        altTimer += Time.fixedDeltaTime;
        if (altTimer > 0.1f)
        {
            if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit))
                altitude = hit.distance;
            altTimer = 0f;
        }
        replayBuffer.Add(new ReplayFrame {
            pos = transform.position,
            rot = transform.rotation
        });

        float maxFrames = replayDuration / Time.fixedDeltaTime;
        if (replayBuffer.Count > maxFrames)
            replayBuffer.RemoveAt(0);

    }

    void LateUpdate()
    {
        if (topText != null)
            topText.text = $"{batteryVoltage:F1} V {cellVoltage:F2} V";

        if (infoText != null)
            infoText.text =
                $"{battery} %\n{usedmAh:F0} mAh\n{current:F1} A\n" +
                $"roll:{roll:F1}\npitch:{pitch:F1}\nSN: 85937947";

        if (leftTopText != null)
            leftTopText.text = $"ACRO\n - {altitude:F1} alt m";

        if (fpsText != null && isEngineOn)
            fpsText.text = $"FPS: {fpsRandom}";
            // activeSelf
        if(canvasNoImage != null && canvasNoImage.activeSelf)
        {
            leftText.text = $"No-Link\nRX-\nTX-\nFW -  1.3";
        }
        else
        {
            leftText.text = $"Link - ok\nRX: -8.0\nTX: -0.07\nFW -  1.3";
        }
        int usedDrone = MissionManager.totalUsedDrones;
        int totalDrone = MissionManager.totalDrones;

        int neutralizedTarget = MissionManager.totalDestroyedTargets;
        int totalTarget =  MissionManager.totalTargets; 
        if (killText != null) killText.text = $"neutralized: {neutralizedTarget}/{totalTarget}";
        if (usedDroneText != null) usedDroneText.text = $"FPV used: {usedDrone}/{totalDrone}";
    }

    float NormalizeAngle(float angle)
    {
        if (angle > 180f) angle -= 360f;
        return angle;
    }

    void SpinProp(Transform prop, bool reverse)
    {
        if (prop == null) return;
        prop.Rotate(Vector3.forward, propSpinSpeed * Time.fixedDeltaTime * (reverse ? -1 : 1));
    }

    private void OnCollisionEnter(Collision other)
    {
       if (isReplaying) return;
    if (isCrashed) return;
    if (other.gameObject.CompareTag("Respawn") || other.gameObject.CompareTag("MapBoundaries")) return;

    MissionManager.SetUsedDrone();

    if (other.gameObject.CompareTag("Enemy"))
        MissionManager.SetDestroyedTarget();


  
    StartCoroutine(CrashDelayThenKamikaze(other));
    }

    IEnumerator CrashDelayThenKamikaze(Collision other)
{
    if (isCrashed) yield break;

    isCrashed = true;
    isEngineOn = false;

    // ✅ HENTIKAN VELOCITY SAAT MASIH NON-KINEMATIC
    rb.linearVelocity = Vector3.zero;
    rb.angularVelocity = Vector3.zero;

    // baru jadikan kinematic
    rb.isKinematic = true;

    ShowNoImageTrue();

    yield return new WaitForSeconds(2f);
    MissionManager.CheckMissionState(other);

    HandleKamikazeImpact(other);
}



    public void setRespawn()
    {
         
        usedmAh = 0f;
        altitude = 0f;
        isCrashed = true;
        isEngineOn = false;

        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.isKinematic = true;  
        ShowNoImageTrue();
        StartCoroutine(RespawnCoroutine());
    }


   private IEnumerator RespawnCoroutine()
{
    // ⛔ JANGAN CEK GAME OVER DI SINI
    // ⛔ JANGAN PAKAI isLastDrone DI SINI

    yield return new WaitForSeconds(respawnDelay);

    if (respawn != null)
        onRespawn();

    rb.isKinematic = true;
    isCrashed = false;
    isEngineOn = false;

    ShowNoImageTrue(); // "Press Enter"
}



   private void onRespawn()
    {
        usedmAh = 0;
        battery = 100;

        rb.isKinematic = true; 

        transform.position = respawn.transform.position;
        transform.rotation = Quaternion.Euler(-1.382f, 92.066f, -0.281f);
        rb.rotation = transform.rotation;

        currentYaw = 92.066f;

        targetPitch = 0;
        targetRoll = 0;
        smoothPitch = 0;
        smoothRoll = 0;

    
    }
    private void ShowNoImageTrue() {
        canvasNoImage.SetActive(true);

    }
    private void ShowNoImageFalse() => canvasNoImage.SetActive(false);

    void HandleKamikazeImpact(Collision other)
    {
        // ledakan selalu tampil
        PlayExplosion();

        // 🏆 MENANG → STOP
        if (MissionManager.isMissionComplete)
        {
            Debug.Log("STOP: MISSION COMPLETE");
            return;
        }

        // 💀 GAME OVER → STOP (TAPI BUKAN KARENA NABRAK)
        if (MissionManager.isGameOver)
        {
            Debug.Log("STOP: GAME OVER (DRONE HABIS)");
            return;
        }

        // 🔁 MASIH ADA DRONE → REPLAY / RESPAWN
        StartCoroutine(ExplosionThenReplay());
    }



IEnumerator ExplosionThenReplay()
{
    PlayExplosion();

    yield return null;
    yield return null;
    yield return null;
    yield return null;

    StartCoroutine(PlayReplayThenRespawn());
}



    IEnumerator PlayReplayThenRespawn()
{
    isReplaying = true;

    // MATIKAN PHYSICS SETELAH PARTICLE AMAN
    rb.isKinematic = true;
    rb.detectCollisions = false;
    rb.useGravity = false;

    foreach (var frame in replayBuffer)
    {
        transform.position = frame.pos;
        transform.rotation = frame.rot;
        yield return new WaitForSeconds(replayFrameDelay); // ⬅️ BUKAN realtime
    }

    rb.detectCollisions = true;
    rb.useGravity = true;

    isReplaying = false;
    replayBuffer.Clear();

    ShowNoImageFalse();
    StartCoroutine(RespawnCoroutine());
}

   void PlayExplosion()
{
    if (explosionPrefab == null) return;

    GameObject go = Instantiate(
        explosionPrefab,
        transform.position,
        Quaternion.identity
    );

    // Pastikan muncul di world
    go.transform.rotation = Quaternion.identity;

    ParticleSystem[] systems =
        go.GetComponentsInChildren<ParticleSystem>(true);

    float maxLife = 0f;

    foreach (var ps in systems)
    {
        // WAJIB: pastikan tidak loop
        var main = ps.main;
        main.loop = false;

        // HITUNG lifetime dengan BENAR
        float duration = main.duration;

        float lifetime =
            main.startLifetime.mode == ParticleSystemCurveMode.TwoConstants
                ? main.startLifetime.constantMax
                : main.startLifetime.constant;

        maxLife = Mathf.Max(maxLife, duration + lifetime);

        // 🔥 PAKSA PLAY
        ps.Clear(true);
        ps.Play(true);
    }

    // BUFFER waktu biar pasti kelihatan
    Destroy(go, maxLife + 0.2f);
    Debug.Log($"Explosion spawned, life = {maxLife}");

}



}
