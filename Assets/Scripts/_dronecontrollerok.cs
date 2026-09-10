//  using UnityEngine;
// using TMPro;
// using System.Collections;
// using System.Collections.Generic;


// [RequireComponent(typeof(Rigidbody))]
// public class DroneController : MonoBehaviour
// {
//     [Header("References")]
//     public Transform propFL, propFR, propBL, propBR;

//     public GameObject canvasNoImage;
//     public GameObject respawnPrefab;
//     public GameObject respawn;
//     public GameObject GameObjectOptionsPanel;
//     public GameObject GameObjectPausedPanel;

//     // TIMER
//     public static float raceTimer = 0f;
//     private float bestTime = 0f;
//     bool raceStarted = false;
//     private bool victoryShown = false;
//     private Vector3 impactPosition;
//     private Rigidbody rb;
//     private Rigidbody[] cachedRBs;

//     [Header("Settings")]
//     public float liftForce = 7f;
//     public float forwardSpeed = 15f;
//     public float strafeSpeed = 15f;
//     public float yawSpeed = 80f;

//     [Header("FPV Tilt Control")]
//     public float maxTiltAngle = 180f;
//     public float tiltSpeed = 5f;
//     public float tiltReturnSpeed = 4f;

//     public float rollStabilizer = 8f;
//     public float rollDamping = 2f;
//     public float propSpinSpeed = 2000f;

//     float targetPitch, targetRoll;
//     float pitchInput, rollInput, yawInput, throttle, forwardInput;
//     float smoothRoll, smoothPitch;

//     public bool isCrashed = false;
//     public bool isGameOver = false;
//     public bool isEngineOn = false;

//     [Header("OSD DISPLAY")]
//     public TextMeshProUGUI fpsText, leftTopText, leftText, topText, infoText,killText, usedDroneText,timerText;

//     public float maxCurrent = 80f;
//     public float batteryCapacity = 6000;
//     public int cellCount = 6;

//     float usedmAh = 0f;
//     float batteryVoltage = 25.2f;
//     float altitude = 0f;
//     float cellVoltage;
//     float current;
//     float roll, pitch, yaw;

//     public float hoverCurrentPercent = 0.35f;
//     public float respawnDelay = 2f;
//     public float RollAngle => smoothRoll * maxTiltAngle;
//     public float PitchAngle => smoothPitch * maxTiltAngle;
//     int battery = 100;
//     int fpsRandom = 30;
    

//     float timer = 0f;
//     float timerBattery = 0f;
//     float altTimer = 0f;

//     float currentYaw = 0f;

//     [Header("Kamikaze Explosion")]
//     public GameObject explosionPrefab;
//     public bool isLastDrone = false; // set dari GameManager

//     public static Vector3 lastImpactPosition;
//     public static Quaternion lastImpactRotation;
//     public static bool lastDroneExploded = false;
//     public GameOverScript gameOverScript;

//     private AICarController lastHitCar;

//     public static bool isReplaying = false;
//  //update kamera replay
//     [Header("Replay Cameras")]
//     public Camera mainCamera;
//     public Camera replayCamera;

//     [Header("OSD Drone")]
//     public GameObject GameObjectNOImage;
//     public GameObject GameObjectInfo;
//     public RectTransform gateMarker;
//     public TMP_Text gateDistanceText;

//     public static Transform targetGate;
//  //update kamera replay
//      //
    
//      struct ReplayFrame
//     {
//         public Vector3 pos;
//         public Quaternion rot;
//     }

//     List<ReplayFrame> replayBuffer = new List<ReplayFrame>();
//     [Header("Replay")]
//     public float replayFrameDelay = 0.05f;
//     public float replayDuration = 3f;
//     [Header("Partikel")]
//     private ParticleSystem explosionPS;



//     string FormatTime(float time)
//     {
//         if (time <= 0f)
//             return "--:--.--";

//         int minutes = Mathf.FloorToInt(time / 60);
//         int seconds = Mathf.FloorToInt(time % 60);
//         int centiseconds = Mathf.FloorToInt((time - Mathf.Floor(time)) * 100);

//         return $"{minutes:00}:{seconds:00}.{centiseconds:00}";
//     }

//     void Start()
//     {
        
//         //update kamera replay
//         if (replayCamera != null)
//         replayCamera.enabled = false;
//         //update kamera replay

//         rb = GetComponent<Rigidbody>();
//         cachedRBs = FindObjectsByType<Rigidbody>(FindObjectsSortMode.None);

//         currentYaw = transform.eulerAngles.y;

//         isEngineOn = false;       
//         rb.isKinematic = true;    

//         if (respawn == null && respawnPrefab != null)
//             respawn = Instantiate(respawnPrefab, transform.position, Quaternion.identity);

//         rb.angularDamping = 0.4f;
//         rb.linearDamping = 0.4f;
//         // best time 
//         bestTime = PlayerPrefs.GetFloat("BestTimeLevel2", 0f);
        
//         // ShowNoImageTrue();        // "Press Enter"
//         gateMarker.gameObject.SetActive(false);
//         gateDistanceText.gameObject.SetActive(false);
//     }


//     void Update()
//     {
       
//         UpdatePropellers();
//         if (isReplaying) return;
//         timer += Time.deltaTime;
//         timerBattery += Time.deltaTime;

//         if (timer >= 1f && isEngineOn)
//         {
//             fpsRandom = Random.Range(29, 35);
//             timer = 0f;
//         }

//         if (battery > 0 && timerBattery >= 15f && isEngineOn)
//         {
//             battery--;
//             timerBattery = 0f;
//         }
//         if(battery == 0 && MissionManager.totalUsedDrones <= MissionManager.totalDrones) { if (respawn != null) { setRespawn();} }
       
//         if (Input.GetKeyDown(KeyCode.Escape)) 
//             if (GameObjectPausedPanel != null) GameObjectPausedPanel.SetActive(true);

//         if (Input.GetKeyDown(KeyCode.Tab))
//         {
//             if (GameObjectOptionsPanel != null) GameObjectOptionsPanel.SetActive(true);
//         }

//         // timer osd
//          if (Level.CurrentLevel == 2 &&
//             raceStarted &&
//             !MissionManager.isMissionComplete &&
//             !MissionManager.isGameOver)
            
//         {
//             raceTimer += Time.deltaTime;
//         }
            
//         if (Input.GetKeyDown(KeyCode.Return) && !isEngineOn)
//         {

//             isEngineOn = true;
//             rb.isKinematic = false;
//             ShowNoImageFalse();
//             if(Level.CurrentLevel == 2) raceStarted = true;//add race timer start
//             gateMarker.gameObject.SetActive(true);
//             gateDistanceText.gameObject.SetActive(true);
//         }

//         //victory 
//          if (MissionManager.isMissionComplete && !victoryShown)
//         {
//             victoryShown = true;

//             GameVictoryScript victory =
//                 FindAnyObjectByType<GameVictoryScript>();

//             if (victory != null)
//                 victory.ShowVictory();
//         }
 
//     }

//     void FixedUpdate()
//     {
       
//         if (isCrashed || !isEngineOn || isReplaying) return;

//         throttle = 0f;
//         if (Input.GetKey(KeyCode.Space))
//         {
//             throttle += 3f;
//             rb.isKinematic = false;
//             ShowNoImageFalse();
//         }
//         if (Input.GetKey(KeyCode.LeftControl)) throttle -= 3f;

//         pitchInput = (Input.GetKey(KeyCode.UpArrow) ? 1 : 0) -
//                      (Input.GetKey(KeyCode.DownArrow) ? 1 : 0);

//         rollInput = (Input.GetKey(KeyCode.D) ? 1 : 0) -
//                     (Input.GetKey(KeyCode.A) ? 1 : 0);

//         yawInput = (Input.GetKey(KeyCode.RightArrow) ? 1 : 0) -
//                    (Input.GetKey(KeyCode.LeftArrow) ? 1 : 0);

//         forwardInput = (Input.GetKey(KeyCode.W) ? 1 : 0) -
//                        (Input.GetKey(KeyCode.S) ? 1 : 0);

//         smoothRoll = Mathf.Lerp(smoothRoll, rollInput, Time.fixedDeltaTime * 6f);
//         smoothPitch = Mathf.Lerp(smoothPitch, pitchInput, Time.fixedDeltaTime * 6f);

//         float hover = rb.mass * Physics.gravity.magnitude;
//         float upright = Mathf.Clamp01(Vector3.Dot(transform.up, Vector3.up));
//         float effectiveLift = (hover + liftForce * throttle) / Mathf.Max(0.5f, upright);

//         rb.AddForce(Vector3.up * effectiveLift, ForceMode.Force);

//         float wantedPitch = smoothPitch * maxTiltAngle;
//         float wantedRoll = smoothRoll * maxTiltAngle;

//         targetPitch = Mathf.Lerp(targetPitch, wantedPitch, Time.fixedDeltaTime * tiltSpeed);
//         targetRoll = Mathf.Lerp(targetRoll, wantedRoll, Time.fixedDeltaTime * tiltSpeed);

//         if (Mathf.Abs(pitchInput) < 0.05f)
//             targetPitch = Mathf.Lerp(targetPitch, 0f, Time.fixedDeltaTime * tiltReturnSpeed);

//         if (Mathf.Abs(rollInput) < 0.05f)
//             targetRoll = Mathf.Lerp(targetRoll, 0f, Time.fixedDeltaTime * tiltReturnSpeed);

//         currentYaw += yawInput * yawSpeed * Time.fixedDeltaTime;


//         Quaternion yawRot = Quaternion.Euler(0f, currentYaw, 0f);
//         Quaternion tilt = Quaternion.Euler(targetPitch, 0f, -targetRoll);

//         transform.rotation = Quaternion.Slerp(transform.rotation, yawRot * tilt, Time.fixedDeltaTime * 8f);

//         rb.AddForce(transform.forward * forwardSpeed * smoothPitch, ForceMode.Force);
//         rb.AddForce(transform.forward * forwardSpeed * forwardInput, ForceMode.Force);
//         rb.AddForce(transform.right * strafeSpeed * smoothRoll, ForceMode.Force);

//         // SpinProp(propFL, false);
//         // SpinProp(propFR, true);
//         // SpinProp(propBL, true);
//         // SpinProp(propBR, false);

//         // current draw sim
//         float controlLoad =
//             Mathf.Abs(smoothRoll) +
//             Mathf.Abs(smoothPitch) +
//             Mathf.Abs(forwardInput) +
//             Mathf.Clamp01(Mathf.Abs(throttle));

//         controlLoad = Mathf.Clamp01(controlLoad);

//         float baseCurrent = maxCurrent * hoverCurrentPercent;
//         float extraCurrent = controlLoad * (maxCurrent * (1f - hoverCurrentPercent));
//         current = baseCurrent + extraCurrent;

//         usedmAh += current * Time.deltaTime * (1f / 3600f) * 1000f;

//         float percent = Mathf.Clamp01(1f - (usedmAh / batteryCapacity));
//         float maxV = 4.2f * cellCount;
//         float minV = 3.3f * cellCount;
//         batteryVoltage = Mathf.Lerp(minV, maxV, percent);
//         cellVoltage = batteryVoltage / cellCount;

//         roll = NormalizeAngle(transform.eulerAngles.z);
//         pitch = NormalizeAngle(transform.eulerAngles.x);
//         yaw = NormalizeAngle(transform.eulerAngles.y);

//         // raycast altitude 
//         altTimer += Time.fixedDeltaTime;
//         if (altTimer > 0.1f)
//         {
//             if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit))
//                 altitude = hit.distance;
//             altTimer = 0f;
//         }
//         replayBuffer.Add(new ReplayFrame {
//             pos = transform.position,
//             rot = transform.rotation
//         });

//         float maxFrames = replayDuration / Time.fixedDeltaTime;
//         if (replayBuffer.Count > maxFrames)
//             replayBuffer.RemoveAt(0);

//     }

//     public float GetRoll()
//     {
//         return targetRoll;
//     }

//     public float GetPitch()
//     {
//         return targetPitch;
//     }

//     void LateUpdate()
//     {
//         if(!isEngineOn)
//         {
//             gateMarker.gameObject.SetActive(false);
//             gateDistanceText.gameObject.SetActive(false);
//             return;
//         }

//         //update kamera replay
//         // if (!isReplaying && replayCamera.enabled == true) return;
//         // replayCamera.transform.position = transform.position + Vector3.up * 7f;
//         // replayCamera.transform.LookAt(transform);
//         //update kamera replay

//         GameObject[] gates =
//         GameObject.FindGameObjectsWithTag("Gates");

//     float closest = Mathf.Infinity;
//     Transform nearest = null;

//     foreach(GameObject g in gates)
//     {
//         if(!g.activeInHierarchy)
//             continue;

//         float dist =
//             Vector3.Distance(
//                 transform.position,
//                 g.transform.position);

//         if(dist < closest)
//         {
//             closest = dist;
//             nearest = g.transform;
//         }
//     }

//     targetGate = nearest;

//         // timmer osd
//        if (timerText != null && Level.CurrentLevel == 2)
//         {
//              string best =
//                 bestTime > 0 ? bestTime.ToString("F2") + "s" : "--";

//             timerText.text =
//                 $"TIME: {FormatTime(raceTimer)}s\nBEST TIME: {FormatTime(bestTime)}";
//         }
       

//         if (topText != null)
//             topText.text = $"{batteryVoltage:F1} V {cellVoltage:F2} V";

//         if (infoText != null)
//             infoText.text =
//                 $"{battery} %\n{usedmAh:F0} mAh\n{current:F1} A\n" +
//                 $"roll:{roll:F1}\npitch:{pitch:F1}\nSN: 85937947";

//         if (leftTopText != null)
//             leftTopText.text = $"ACRO\n - {altitude:F1} alt m";

//         // if (fpsText != null && isEngineOn)
//         //     fpsText.text = $"FPS: {fpsRandom}";
//             // activeSelf
//         if(canvasNoImage != null && canvasNoImage.activeSelf)
//         {
//             leftText.text = $"No-Link\nRX-\nTX-\nFW -  1.3";
//         }
//         else
//         {
//             leftText.text = $"Link - ok\nRX: -8.0\nTX: -0.07\nFW -  1.3";
//         }
//         int usedDrone = MissionManager.totalUsedDrones;
//         int totalDrone = MissionManager.totalDrones;

//         int currenGates = MissionManager.CurrentGates;
//         int totalGates =  MissionManager.TotalGates; 
//         if (killText != null) killText.text = $"checkpoint: {currenGates}/{totalGates}";
//         if (usedDroneText != null) usedDroneText.text = $"CRASHES: {usedDrone}/{totalDrone} | Level: {Level.CurrentLevel}";

//         if (targetGate != null &&
//         gateMarker != null &&
//         gateDistanceText != null &&
//         Camera.main != null)
//         {
           
//             // Vector3 screenPos =
//             //     Camera.main.WorldToScreenPoint(
//             //         targetGate.position);
//             Transform markerPoint =
//                 targetGate.Find("MarkerPoint");
//             Vector3 targetPos =
//                 markerPoint != null
//                 ? markerPoint.position
//                 : targetGate.position;

//             Vector3 screenPos =
//                 Camera.main.WorldToScreenPoint(
//                     targetPos);
//            if(screenPos.z > 0)
//             {
//                 gateMarker.gameObject.SetActive(true);
//                 gateMarker.position = screenPos;

//                 float distances =
//                     Vector3.Distance(
//                         transform.position,
//                         targetPos);

//                float scale = Mathf.Clamp(distances / 30f, 0.2f, 1.2f);

//                 gateMarker.localScale =
//                     Vector3.one * scale;
//             }
//             else
//             {
//                 gateMarker.gameObject.SetActive(false);
//             }

           
//             // float distance =
//             //     Vector3.Distance(
//             //         transform.position,
//             //         targetGate.position);
//             float distance =
//                 Vector3.Distance(
//                     transform.position,
//                     targetPos);
//             gateDistanceText.text =
//                 distance.ToString("F0") + "m";
//         }

      
//     }

//     float NormalizeAngle(float angle)
//     {
//         if (angle > 180f) angle -= 360f;
//         return angle;
//     }

//     void SpinProp(Transform prop, bool reverse)
//     {
//         if (prop == null) return;
//         prop.Rotate(Vector3.forward, propSpinSpeed * Time.fixedDeltaTime * (reverse ? -1 : 1));
//     }

//     private void OnTriggerEnter(Collider other)
//     {
//         Debug.Log($"TRIGGER : {other.name} | Frame: {Time.frameCount}");
//         if (other.CompareTag("Gates"))
//         {
//             Debug.Log("Checkpoint Passed");
//               // Mainkan suara checkpoint
//             AudioManager.Instance.PlayGatePassed();

//             // MissionManager.SetGatePassed();
//             Debug.Log("SEBELUM : " + MissionManager.CurrentGates);

//             MissionManager.SetGatePassed();

//             Debug.Log("SESUDAH : " + MissionManager.CurrentGates);
//             // FindAnyObjectByType<GateRaceManager>()?.NextGate();

//             other.gameObject.SetActive(false);
//            Transform gate = other.transform;

//            Debug.Log("LEWAT : " + gate.name);

//             // if(gate.parent != null)
//             // {
//             //     int nextIndex = gate.GetSiblingIndex() + 1;

//             //     if(nextIndex < gate.parent.childCount)
//             //     {
//             //         targetGate = gate.parent.GetChild(nextIndex);

//             //         Debug.Log("TARGET BARU : " + targetGate.name);
//             //     }
//             // }
//         }
        
//     }


//     private void OnCollisionEnter(Collision other)
//     {
//         if (isReplaying) return;
//         if (isCrashed) return;
//         if (other.gameObject.CompareTag("Respawn") || other.gameObject.CompareTag("MapBoundaries")) return;


//         MissionManager.SetUsedDrone();
//         impactPosition = transform.position;

//         if (other.gameObject.CompareTag("Enemy"))
//             MissionManager.SetDestroyedTarget();
//         AICarController car = other.collider.GetComponent<AICarController>();
//         if (car != null)
//         {
//             lastHitCar = car; 
//         }
//         StartCoroutine(CrashDelayThenKamikaze(other));
//     }

//     IEnumerator CrashDelayThenKamikaze(Collision other)
//     {
//         if (isCrashed) yield break;
//         isCrashed = true;
//         isEngineOn = false;
//         rb.linearVelocity = Vector3.zero;
//         rb.angularVelocity = Vector3.zero;
//         rb.isKinematic = true;
//         ShowNoImageTrue();
//         yield return new WaitForSeconds(1f);
//         MissionManager.CheckMissionState(other);
//         HandleKamikazeImpact(other);
//     }

//     public void setRespawn()
//     {
//         usedmAh = 0f;
//         altitude = 0f;
//         isCrashed = true;
//         isEngineOn = false;

//         rb.linearVelocity = Vector3.zero;
//         rb.angularVelocity = Vector3.zero;
//         rb.isKinematic = true;  
//         // ShowNoImageTrue();
//         StartCoroutine(RespawnCoroutine());
//     }

//     private IEnumerator RespawnCoroutine()
//     {
//         yield return new WaitForSeconds(respawnDelay);
//         if (respawn != null)
//             onRespawn();
//         rb.isKinematic = true;
//         isCrashed = false;
//         isEngineOn = false;
//         gateMarker.gameObject.SetActive(false);
//         gateDistanceText.gameObject.SetActive(false);
//           //update kamera replay
//         SwitchToMainCamera(); // replay kamera
//          //update kamera replay
//         ShowInfoOsdPanelTrue();
//         ShowNoImageTrue(); // "Press Enter"
//     }

//     private void onRespawn()
//     {
//         ShowDrone();
//         usedmAh = 0;
//         battery = 100;

//         rb.isKinematic = true; 

//         if (Level.CurrentLevel == 2 &&
//             raceStarted &&
//             !MissionManager.isMissionComplete &&
//             !MissionManager.isGameOver)
            
//         {
//             if(Level2Manager.CurrentSpawnMarker != null)
//             {
//                 transform.position = Level2Manager.CurrentSpawnMarker.position;

//                 transform.rotation =
//                 Level2Manager.CurrentSpawnMarker.rotation *
//                 Quaternion.Euler(0, 90, 0);

//                 rb.rotation = transform.rotation;

//                 currentYaw = transform.eulerAngles.y;


//     //         transform.position =
//     //             Level2Manager.CurrentSpawnMarker.position;

//     //         // transform.rotation = Quaternion.Euler(-1.382f, 92.066f, -0.281f);

//     //         rb.rotation = transform.rotation;
//     //        currentYaw =
//     // Level2Manager.CurrentSpawnMarker.rotation.eulerAngles.y;
           
//             }
//         }
//         else
//         {
//             transform.position = respawn.transform.position;
//             transform.rotation = Quaternion.Euler(-1.382f, 92.066f, -0.281f);
//             rb.rotation = transform.rotation;
//             currentYaw = 92.066f;
//         }

      

        

//         targetPitch = 0;
//         targetRoll = 0;
//         smoothPitch = 0;
//         smoothRoll = 0;
//         replayBuffer.Clear();
//     }
//     private void ShowNoImageTrue() => canvasNoImage.SetActive(true);
//     private void ShowNoImageFalse() => canvasNoImage.SetActive(false);
//     private void ShowInfoOsdPanelTrue () => GameObjectInfo.SetActive(true);
//     private void ShowInfoOsdPanelFalse () => GameObjectInfo.SetActive(false);
   

//     void HandleKamikazeImpact(Collision other)
//     {
//         // ledakan selalu tampilP
//         // if (MissionManager.isMissionComplete)
//         // {
//         //     Debug.Log("STOP: MISSION COMPLETE");
//         //     return;
//         // }

//         if (MissionManager.isGameOver)
//         {
//             Debug.Log("STOP: GAME OVER (DRONE HABIS)");
//             return;
//         }


//         StartCoroutine(ExplosionThenReplay());
//     }

//     IEnumerator ExplosionThenReplay()
//     {
//         yield return new WaitForSeconds(0.3f);
//          //update kamera replay
//         ShowInfoOsdPanelFalse();
//         ShowNoImageFalse();
//         SwitchToReplayCamera(); //replay kamera
//          //update kamera replay
//         StartCoroutine(PlayReplayThenRespawn());
//     }

//     IEnumerator PlayReplayThenRespawn()
//     {
//         isReplaying = true;
//         rb.isKinematic = true;
//         rb.detectCollisions = false;
//         rb.useGravity = false;

//          List<ReplayFrame> replayFrames = GetReplayFramesBeforeImpact(10f); // 4 meter
//         foreach (var frame in replayFrames)
//         {
//             transform.position = frame.pos;
//             transform.rotation = frame.rot;
//             yield return new WaitForSeconds(replayFrameDelay);
//         }
//         HideDrone();
//         PlayExplosion();
//         yield return new WaitForSeconds(1.5f);
//         rb.detectCollisions = true;
//         rb.useGravity = true;
// //update kamera replay
//         // rb.isKinematic = false;
// //update kamera replay
//         isReplaying = false;
//         if (lastHitCar != null)
//         {
//             // lastHitCar.TriggerExplosion();
//             lastHitCar.PlayReplayDestruction();
//             lastHitCar = null;
//         }
//         replayBuffer.Clear();
//         // ShowNoImageFalse();
//         StartCoroutine(RespawnCoroutine());
//     }

//     void PlayExplosion()
//     {
//         if (explosionPrefab == null) return;
//         GameObject go = Instantiate(
//             explosionPrefab,
//             transform.position,
//             Quaternion.identity
//         );
//         go.transform.rotation = Quaternion.identity;
//         ParticleSystem[] systems =
//             go.GetComponentsInChildren<ParticleSystem>(true);

//         float maxLife = 0f;
//         foreach (var ps in systems)
//         {
//             var main = ps.main;
//             main.loop = false;

//             float duration = main.duration;

//             float lifetime =
//                 main.startLifetime.mode == ParticleSystemCurveMode.TwoConstants
//                     ? main.startLifetime.constantMax
//                     : main.startLifetime.constant;

//             maxLife = Mathf.Max(maxLife, duration + lifetime);
//             ps.Clear(true);
//             ps.Play(true);
//         }
//         Destroy(go, maxLife + 0.2f);
//     }

//  //update kamera replay
// //replay switch
//     void SwitchToReplayCamera()
//     {
//         if (mainCamera != null) mainCamera.enabled = false;
//         if (replayCamera != null) replayCamera.enabled = true;
//     }

//     void SwitchToMainCamera()
//     {
//         if (replayCamera != null) replayCamera.enabled = false;
//         if (mainCamera != null) mainCamera.enabled = true;
//     }
//  //update kamera replay

//     void UpdatePropellers()
//     {
//         SpinProp(propFL, false);
//         SpinProp(propFR, true);
//         SpinProp(propBL, true);
//         SpinProp(propBR, false);
//     }

//     List<ReplayFrame> GetReplayFramesBeforeImpact(float distance)
//     {
//         List<ReplayFrame> result = new List<ReplayFrame>();

//         float traveled = 0f;
//         Vector3 lastPos = impactPosition;

//         for (int i = replayBuffer.Count - 1; i >= 0; i--)
//         {
//             ReplayFrame frame = replayBuffer[i];
//             traveled += Vector3.Distance(lastPos, frame.pos);
//             lastPos = frame.pos;

//             result.Add(frame);

//             if (traveled >= distance)
//                 break;
//         }

//         result.Reverse(); 
//         return result;
//     }

//     void HideDrone()
//     {
//         foreach (var r in GetComponentsInChildren<Renderer>())
//             r.enabled = false;

//         foreach (var c in GetComponentsInChildren<Collider>())
//             c.enabled = false;
//     }

//     void ShowDrone()
//     {
//         foreach (var r in GetComponentsInChildren<Renderer>())
//             r.enabled = true;

//         foreach (var c in GetComponentsInChildren<Collider>())
//             c.enabled = true;
//     }

// }



// using UnityEngine;
// using TMPro;
// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine.InputSystem;


// [RequireComponent(typeof(Rigidbody))]
// public class DroneController : MonoBehaviour
// {
//     [Header("References")]
//     public Transform propFL, propFR, propBL, propBR;

//     public GameObject canvasNoImage;
//     public GameObject respawnPrefab;
//     public GameObject respawn;
//     public GameObject GameObjectOptionsPanel;
//     public GameObject GameObjectPausedPanel;

//     // TIMER
//     public static float raceTimer = 0f;
//     private float bestTime = 0f;
//     bool raceStarted = false;
//     private bool victoryShown = false;
//     private Vector3 impactPosition;
//     private Rigidbody rb;
//     private Rigidbody[] cachedRBs;

//     [Header("Settings")]
//     public float liftForce = 7f;
//     public float forwardSpeed = 15f;
//     public float strafeSpeed = 15f;
//     public float yawSpeed = 80f;

//     [Header("FPV Tilt Control")]
//     public float maxTiltAngle = 180f;
//     public float tiltSpeed = 5f;
//     public float tiltReturnSpeed = 4f;

//     public float rollStabilizer = 8f;
//     public float rollDamping = 2f;
//     public float propSpinSpeed = 2000f;

//     float targetPitch, targetRoll;
//     float pitchInput, rollInput, yawInput, throttle, forwardInput;
//     float smoothRoll, smoothPitch;

//     public bool isCrashed = false;
//     public bool isGameOver = false;
//     public bool isEngineOn = false;

//     [Header("OSD DISPLAY")]
//     public TextMeshProUGUI fpsText, leftTopText, leftText, topText, infoText,killText, usedDroneText,timerText;

//     public float maxCurrent = 80f;
//     public float batteryCapacity = 6000;
//     public int cellCount = 6;

//     float usedmAh = 0f;
//     float batteryVoltage = 25.2f;
//     float altitude = 0f;
//     float cellVoltage;
//     float current;
//     float roll, pitch, yaw;

//     public float hoverCurrentPercent = 0.35f;
//     public float respawnDelay = 2f;
//     public float RollAngle => smoothRoll * maxTiltAngle;
//     public float PitchAngle => smoothPitch * maxTiltAngle;
//     int battery = 100;
//     int fpsRandom = 30;

//     float timer = 0f;
//     float timerBattery = 0f;
//     float altTimer = 0f;

//     float currentYaw = 0f;

//     [Header("Kamikaze Explosion")]
//     public GameObject explosionPrefab;
//     public bool isLastDrone = false; // set dari GameManager

//     public static Vector3 lastImpactPosition;
//     public static Quaternion lastImpactRotation;
//     public static bool lastDroneExploded = false;
//     public GameOverScript gameOverScript;

//     private AICarController lastHitCar;

//     public static bool isReplaying = false;
//  //update kamera replay
//     [Header("Replay Cameras")]
//     public Camera mainCamera;
//     public Camera replayCamera;

//     [Header("OSD Drone")]
//     public GameObject GameObjectNOImage;
//     public GameObject GameObjectInfo;
//     public RectTransform gateMarker;
//     public TMP_Text gateDistanceText;

//     public static Transform targetGate;

//     private DroneControls controls;

//     private Vector2 moveInput;
//     private Vector2 lookInput;

//     public float throttleValue = 0.5f;
//     public float throttleSpeed = 4f; 
//     private bool throttleActivated = false;
  

    
//     struct ReplayFrame
//     {
//         public Vector3 pos;
//         public Quaternion rot;
//     }

//     List<ReplayFrame> replayBuffer = new List<ReplayFrame>();
//     [Header("Replay")]
//     public float replayFrameDelay = 0.05f;
//     public float replayDuration = 3f;
//     [Header("Partikel")]
//     private ParticleSystem explosionPS;

//     void Awake()
//     {
//         controls = new DroneControls();
//     }

//     void OnEnable()
//     {
//         controls.Enable();
//     }

//     void OnDisable()
//     {
//         controls.Disable();
//     }


//     string FormatTime(float time)
//     {
//         if (time <= 0f)
//             return "--:--.--";

//         int minutes = Mathf.FloorToInt(time / 60);
//         int seconds = Mathf.FloorToInt(time % 60);
//         int centiseconds = Mathf.FloorToInt((time - Mathf.Floor(time)) * 100);

//         return $"{minutes:00}:{seconds:00}.{centiseconds:00}";
//     }

//     public void ArmDrone()
//     {
//         if (isEngineOn)
//             return;

//         isEngineOn = true;
//         rb.isKinematic = false;
//         ShowNoImageFalse();

//         if (Level.CurrentLevel == 2)
//             raceStarted = true;

//         gateMarker.gameObject.SetActive(true);
//         gateDistanceText.gameObject.SetActive(true);
//     }

//     public void RespawnDrone ()
//     {
//         onRespawn();
//     }

//     public void OptionsDrone ()
//     {
//         if(GameObjectOptionsPanel != null) GameObjectOptionsPanel.SetActive(true);
//     }

//     public void PauseDrone()
//     {
//         if(GameObjectPausedPanel != null) GameObjectPausedPanel.SetActive(true);
//     }

//     void ResetOsd ()
//     {
//         if (infoText != null)
//             infoText.text =
//                 $"100 %\n0 mAh\n0 A\n" +
//                 $"roll:- \npitch:- \nSN: 85937947";
//     }

//     void Start()
//     {
//         //update kamera replay
//         if (replayCamera != null)
//         replayCamera.enabled = false;
//         //update kamera replay

//         rb = GetComponent<Rigidbody>();
//         cachedRBs = FindObjectsByType<Rigidbody>();

//         currentYaw = transform.eulerAngles.y;

//         isEngineOn = false;       
//         rb.isKinematic = true;    

//         if (respawn == null && respawnPrefab != null)
//             respawn = Instantiate(respawnPrefab, transform.position, Quaternion.identity);

//         rb.angularDamping = 0.4f;
//         rb.linearDamping = 0.4f;
//         // best time 
//         bestTime = PlayerPrefs.GetFloat("BestTimeLevel2", 0f);
        
//         // ShowNoImageTrue();        // "Press Enter"
//         gateMarker.gameObject.SetActive(false);
//         gateDistanceText.gameObject.SetActive(false);
//     }

//     void Update()
//     {
//         UpdatePropellers();
//         if (isReplaying) return;
//         timer += Time.deltaTime;
//         timerBattery += Time.deltaTime;

//         if (timer >= 1f && isEngineOn)
//         {
//             fpsRandom = Random.Range(29, 35);
//             timer = 0f;
//         }

//         if (battery > 0 && timerBattery >= 15f && isEngineOn)
//         {
//             battery--;
//             timerBattery = 0f;
//         }
//         if(battery == 0 && MissionManager.totalUsedDrones <= MissionManager.totalDrones) { if (respawn != null) { setRespawn();} }

//         if (controls.Drone.Arm.WasPressedThisFrame()) ArmDrone();
//         if (controls.Drone.Respawn.WasPressedThisFrame()) onRespawn();
//         if (controls.Drone.Pause.WasPressedThisFrame()) PauseDrone();
//         if (controls.Drone.Options.WasPressedThisFrame()) OptionsDrone();

//         // timer osd
//         if (Level.CurrentLevel == 2 &&
//             raceStarted &&
//             !MissionManager.isMissionComplete &&
//             !MissionManager.isGameOver)
            
//         {
//             raceTimer += Time.deltaTime;
//         }
            
//         if (controls.Drone.Arm.WasPressedThisFrame() && !isEngineOn)
//         {
//             isEngineOn = true;
//             rb.isKinematic = false;
//             ShowNoImageFalse();

//             if (Level.CurrentLevel == 2)
//                 raceStarted = true;

//             gateMarker.gameObject.SetActive(true);
//             gateDistanceText.gameObject.SetActive(true);
//         }

//         //victory 
//         if (MissionManager.isMissionComplete && !victoryShown)
//         {
//             victoryShown = true;

//             GameVictoryScript victory =
//                 FindAnyObjectByType<GameVictoryScript>();

//             if (victory != null)
//                 victory.ShowVictory();
//         }
//     }

//     void FixedUpdate()
//     {
//         if (isCrashed || !isEngineOn || isReplaying) return;

//         moveInput = controls.Drone.Move.ReadValue<Vector2>();
//         lookInput = controls.Drone.Look.ReadValue<Vector2>();

//         throttle = moveInput.y;
//         yawInput = moveInput.x;

//         pitchInput = lookInput.y;
//         rollInput = lookInput.x;

//         if (Mathf.Abs(throttle) > 0.01f)
//         {
//             rb.isKinematic = false;
//             ShowNoImageFalse();
//         }
//         smoothRoll = Mathf.Lerp(smoothRoll, rollInput, Time.fixedDeltaTime * 6f);
//         smoothPitch = Mathf.Lerp(smoothPitch, pitchInput, Time.fixedDeltaTime * 6f);
        
//         // float hover = rb.mass * Physics.gravity.magnitude;
//         // float thrust = hover + liftForce * throttle;

//         // rb.AddForce(transform.up * thrust, ForceMode.Force);
//         if (throttle > 0.01f)
//             throttleActivated = true;

//         if (throttleActivated)
//         {
//             float hover = rb.mass * Physics.gravity.magnitude;
//             float thrust = hover + liftForce * throttle;
//             rb.AddForce(transform.up * thrust, ForceMode.Force);
//         }


//         float wantedPitch = smoothPitch * maxTiltAngle;
//         float wantedRoll = smoothRoll * maxTiltAngle;

//         targetPitch = Mathf.Lerp(targetPitch, wantedPitch, Time.fixedDeltaTime * tiltSpeed);
//         targetRoll = Mathf.Lerp(targetRoll, wantedRoll, Time.fixedDeltaTime * tiltSpeed);

//         if (Mathf.Abs(pitchInput) < 0.05f)
//             targetPitch = Mathf.Lerp(targetPitch, 0f, Time.fixedDeltaTime * tiltReturnSpeed);

//         if (Mathf.Abs(rollInput) < 0.05f)
//             targetRoll = Mathf.Lerp(targetRoll, 0f, Time.fixedDeltaTime * tiltReturnSpeed);

//         currentYaw += yawInput * yawSpeed * Time.fixedDeltaTime;


//         Quaternion yawRot = Quaternion.Euler(0f, currentYaw, 0f);
//         Quaternion tilt = Quaternion.Euler(targetPitch, 0f, -targetRoll);

//         transform.rotation = Quaternion.Slerp(transform.rotation, yawRot * tilt, Time.fixedDeltaTime * 8f);

//         rb.AddForce(
//             transform.right * strafeSpeed * smoothRoll,
//             ForceMode.Force);

//         float controlLoad =
//         Mathf.Abs(smoothRoll) +
//         Mathf.Abs(smoothPitch) +
//         Mathf.Clamp01(Mathf.Abs(throttle));

//         controlLoad = Mathf.Clamp01(controlLoad);

//         float baseCurrent = maxCurrent * hoverCurrentPercent;
//         float extraCurrent = controlLoad * (maxCurrent * (1f - hoverCurrentPercent));
//         current = baseCurrent + extraCurrent;

//         usedmAh += current * Time.deltaTime * (1f / 3600f) * 1000f;

//         float percent = Mathf.Clamp01(1f - (usedmAh / batteryCapacity));
//         float maxV = 4.2f * cellCount;
//         float minV = 3.3f * cellCount;
//         batteryVoltage = Mathf.Lerp(minV, maxV, percent);
//         cellVoltage = batteryVoltage / cellCount;

//         roll = NormalizeAngle(transform.eulerAngles.z);
//         pitch = NormalizeAngle(transform.eulerAngles.x);
//         yaw = NormalizeAngle(transform.eulerAngles.y);

//         // raycast altitude 
//         altTimer += Time.fixedDeltaTime;
//         if (altTimer > 0.1f)
//         {
//             if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit))
//                 altitude = hit.distance;
//             altTimer = 0f;
//         }
//         replayBuffer.Add(new ReplayFrame {
//             pos = transform.position,
//             rot = transform.rotation
//         });

//         float maxFrames = replayDuration / Time.fixedDeltaTime;
//         if (replayBuffer.Count > maxFrames)
//             replayBuffer.RemoveAt(0);

//     }

//     public float GetRoll()
//     {
//         return targetRoll;
//     }

//     public float GetPitch()
//     {
//         return targetPitch;
//     }

//     void LateUpdate()
//     {
//         if(!isEngineOn)
//         {
//             gateMarker.gameObject.SetActive(false);
//             gateDistanceText.gameObject.SetActive(false);
//             return;
//         }

//         GameObject[] gates =
//         GameObject.FindGameObjectsWithTag("Gates");

//     float closest = Mathf.Infinity;
//     Transform nearest = null;

//     foreach(GameObject g in gates)
//     {
//         if(!g.activeInHierarchy)
//             continue;

//         float dist =
//             Vector3.Distance(
//                 transform.position,
//                 g.transform.position);

//         if(dist < closest)
//         {
//             closest = dist;
//             nearest = g.transform;
//         }
//     }

//     targetGate = nearest;

//         // timmer osd
//        if (timerText != null && Level.CurrentLevel == 2)
//         {
//              string best =
//                 bestTime > 0 ? bestTime.ToString("F2") + "s" : "--";

//             timerText.text =
//                 $"TIME: {FormatTime(raceTimer)}s\nBEST TIME: {FormatTime(bestTime)}";
//         }
       

//         if (topText != null)
//             topText.text = $"{batteryVoltage:F1} V {cellVoltage:F2} V";

//         if (infoText != null)
//             infoText.text =
//                 $"{battery} %\n{usedmAh:F0} mAh\n{current:F1} A\n" +
//                 $"roll:{roll:F1}\npitch:{pitch:F1}\nSN: 85937947";

//         if (leftTopText != null)
//             leftTopText.text = $"ACRO\n - {altitude:F1} alt m";

//         if (fpsText != null && isEngineOn)
//             fpsText.text = $"FPS: {fpsRandom}";
//             // activeSelf
//         if(canvasNoImage != null && canvasNoImage.activeSelf)
//         {
//             leftText.text = $"No-Link\nRX-\nTX-\nFW -  1.3";
//         }
//         else
//         {
//             leftText.text = $"Link - ok\nRX: -8.0\nTX: -0.07\nFW -  1.3";
//         }
//         int usedDrone = MissionManager.totalUsedDrones;
//         int totalDrone = MissionManager.totalDrones;

//         int currenGates = MissionManager.CurrentGates;
//         int totalGates =  MissionManager.TotalGates; 
//         if (killText != null) killText.text = $"checkpoint: {currenGates}/{totalGates}";
//         if (usedDroneText != null) usedDroneText.text = $"CRASHES: {usedDrone}/{totalDrone} | Level: {Level.CurrentLevel}";

//         if (targetGate != null &&
//         gateMarker != null &&
//         gateDistanceText != null &&
//         Camera.main != null)
//         {
    
//             Transform markerPoint =
//                 targetGate.Find("MarkerPoint");
//             Vector3 targetPos =
//                 markerPoint != null
//                 ? markerPoint.position
//                 : targetGate.position;

//             Vector3 screenPos =
//                 Camera.main.WorldToScreenPoint(
//                     targetPos);
//             if(screenPos.z > 0)
//             {
//                 gateMarker.gameObject.SetActive(true);
//                 gateMarker.position = screenPos;

//                 float distances =
//                     Vector3.Distance(
//                         transform.position,
//                         targetPos);

//                 float scale = Mathf.Clamp(distances / 30f, 1f, 2f);

//                 gateMarker.localScale =
//                     Vector3.one * scale;
//             }
//             else
//             {
//                 gateMarker.gameObject.SetActive(false);
//             }
//             float distance =
//                 Vector3.Distance(
//                     transform.position,
//                     targetPos);
//             gateDistanceText.text =
//                 distance.ToString("F0") + "m";
//         }
//     }

//     float NormalizeAngle(float angle)
//     {
//         if (angle > 180f) angle -= 360f;
//         return angle;
//     }

//     void SpinProp(Transform prop, bool reverse)
//     {
//         if (prop == null) return;
//         prop.Rotate(Vector3.forward, propSpinSpeed * Time.fixedDeltaTime * (reverse ? -1 : 1));
//     }

//     private void OnTriggerEnter(Collider other)
//     {
//         // Debug.Log($"TRIGGER : {other.name} | Frame: {Time.frameCount}");
//         if (other.CompareTag("Gates"))
//         {
//               // Mainkan suara checkpoint
//             // AudioManager.Instance.PlayGatePassed();

//             MissionManager.SetGatePassed();

//             // other.gameObject.SetActive(false);
//             // Transform gate = other.transform;
//         }
        
//     }


//     private void OnCollisionEnter(Collision other)
//     {
//         if (isReplaying) return;
//         if (isCrashed) return;
//         if (other.gameObject.CompareTag("Respawn") || other.gameObject.CompareTag("MapBoundaries")) return;


//         MissionManager.SetUsedDrone();
//         impactPosition = transform.position;

//         if (other.gameObject.CompareTag("Enemy"))
//             MissionManager.SetDestroyedTarget();
//         AICarController car = other.collider.GetComponent<AICarController>();
//         if (car != null)
//         {
//             lastHitCar = car; 
//         }
//         StartCoroutine(CrashDelayThenKamikaze(other));
//     }

//     IEnumerator CrashDelayThenKamikaze(Collision other)
//     {
//         if (isCrashed) yield break;
//         isCrashed = true;
//         isEngineOn = false;
//         rb.linearVelocity = Vector3.zero;
//         rb.angularVelocity = Vector3.zero;
//         rb.isKinematic = true;
//         ShowNoImageTrue();
//         yield return new WaitForSeconds(1f);
//         MissionManager.CheckMissionState(other);
//         HandleKamikazeImpact(other);
//     }

//     public void setRespawn()
//     {
//         usedmAh = 0f;
//         altitude = 0f;
//         isCrashed = true;
//         isEngineOn = false;

//         rb.linearVelocity = Vector3.zero;
//         rb.angularVelocity = Vector3.zero;
//         rb.isKinematic = true;  
//         // ShowNoImageTrue();
//         StartCoroutine(RespawnCoroutine());
//     }

//     private IEnumerator RespawnCoroutine()
//     {
//         yield return new WaitForSeconds(respawnDelay);
//         if (respawn != null)
//             onRespawn();
//         rb.isKinematic = true;
//         isCrashed = false;
//         isEngineOn = false;
//         gateMarker.gameObject.SetActive(false);
//         gateDistanceText.gameObject.SetActive(false);
//           //update kamera replay
//         SwitchToMainCamera(); // replay kamera
//          //update kamera replay
//         ShowInfoOsdPanelTrue();
//         ShowNoImageTrue(); // "Press Enter"
//         ResetOsd();
//     }

//     private void onRespawn()
//     {
//         ShowDrone();
//         usedmAh = 0;
//         battery = 100;

//         rb.isKinematic = true; 

//         if (Level.CurrentLevel == 2 &&
//             raceStarted &&
//             !MissionManager.isMissionComplete &&
//             !MissionManager.isGameOver)
            
//         {
//             if(Level2Manager.CurrentSpawnMarker != null)
//             {
//                 transform.position = Level2Manager.CurrentSpawnMarker.position;

//                 transform.rotation =
//                 Level2Manager.CurrentSpawnMarker.rotation *
//                 Quaternion.Euler(0, 90, 0);

//                 rb.rotation = transform.rotation;

//                 currentYaw = transform.eulerAngles.y;
//             }
//         }
//         else
//         {
//             transform.position = respawn.transform.position;
//             transform.rotation = Quaternion.Euler(-1.382f, 92.066f, -0.281f);
//             rb.rotation = transform.rotation;
//             currentYaw = 92.066f;
//         }

//         targetPitch = 0;
//         targetRoll = 0;
//         smoothPitch = 0;
//         smoothRoll = 0;
//         replayBuffer.Clear();
//     }
//     private void ShowNoImageTrue() => canvasNoImage.SetActive(true);
//     private void ShowNoImageFalse() => canvasNoImage.SetActive(false);
//     private void ShowInfoOsdPanelTrue () => GameObjectInfo.SetActive(true);
//     private void ShowInfoOsdPanelFalse () => GameObjectInfo.SetActive(false);

//     void HandleKamikazeImpact(Collision other)
//     {
//         if (MissionManager.isGameOver)
//         {
//             // Debug.Log("STOP: GAME OVER (DRONE HABIS)");
//             return;
//         }


//         StartCoroutine(ExplosionThenReplay());
//     }

//     IEnumerator ExplosionThenReplay()
//     {
//         yield return new WaitForSeconds(0.3f);
//          //update kamera replay
//         ShowInfoOsdPanelFalse();
//         ShowNoImageFalse();
//         SwitchToReplayCamera(); //replay kamera
//          //update kamera replay
//         StartCoroutine(PlayReplayThenRespawn());
//     }

//     IEnumerator PlayReplayThenRespawn()
//     {
//         isReplaying = true;
//         rb.isKinematic = true;
//         rb.detectCollisions = false;
//         rb.useGravity = false;

//          List<ReplayFrame> replayFrames = GetReplayFramesBeforeImpact(10f); // 4 meter
//         foreach (var frame in replayFrames)
//         {
//             transform.position = frame.pos;
//             transform.rotation = frame.rot;
//             yield return new WaitForSeconds(replayFrameDelay);
//         }
//         HideDrone();
//         PlayExplosion();
//         yield return new WaitForSeconds(1.5f);
//         rb.detectCollisions = true;
//         rb.useGravity = true;

//         isReplaying = false;
//         if (lastHitCar != null)
//         {
//             // lastHitCar.TriggerExplosion();
//             lastHitCar.PlayReplayDestruction();
//             lastHitCar = null;
//         }
//         replayBuffer.Clear();
//         // ShowNoImageFalse();
//         StartCoroutine(RespawnCoroutine());
//     }

//     void PlayExplosion()
//     {
//         if (explosionPrefab == null) return;
//         GameObject go = Instantiate(
//             explosionPrefab,
//             transform.position,
//             Quaternion.identity
//         );
//         go.transform.rotation = Quaternion.identity;
//         ParticleSystem[] systems =
//             go.GetComponentsInChildren<ParticleSystem>(true);

//         float maxLife = 0f;
//         foreach (var ps in systems)
//         {
//             var main = ps.main;
//             main.loop = false;

//             float duration = main.duration;

//             float lifetime =
//                 main.startLifetime.mode == ParticleSystemCurveMode.TwoConstants
//                     ? main.startLifetime.constantMax
//                     : main.startLifetime.constant;

//             maxLife = Mathf.Max(maxLife, duration + lifetime);
//             ps.Clear(true);
//             ps.Play(true);
//         }
//         Destroy(go, maxLife + 0.2f);
//     }

//  //update kamera replay
// //replay switch
//     void SwitchToReplayCamera()
//     {
//         if (mainCamera != null) mainCamera.enabled = false;
//         if (replayCamera != null) replayCamera.enabled = true;
//     }

//     void SwitchToMainCamera()
//     {
//         if (replayCamera != null) replayCamera.enabled = false;
//         if (mainCamera != null) mainCamera.enabled = true;
//     }
//  //update kamera replay

//     void UpdatePropellers()
//     {
//         SpinProp(propFL, false);
//         SpinProp(propFR, true);
//         SpinProp(propBL, true);
//         SpinProp(propBR, false);
//     }

//     List<ReplayFrame> GetReplayFramesBeforeImpact(float distance)
//     {
//         List<ReplayFrame> result = new List<ReplayFrame>();

//         float traveled = 0f;
//         Vector3 lastPos = impactPosition;

//         for (int i = replayBuffer.Count - 1; i >= 0; i--)
//         {
//             ReplayFrame frame = replayBuffer[i];
//             traveled += Vector3.Distance(lastPos, frame.pos);
//             lastPos = frame.pos;

//             result.Add(frame);

//             if (traveled >= distance)
//                 break;
//         }

//         result.Reverse(); 
//         return result;
//     }

//     void HideDrone()
//     {
//         foreach (var r in GetComponentsInChildren<Renderer>())
//             r.enabled = false;

//         foreach (var c in GetComponentsInChildren<Collider>())
//             c.enabled = false;
//     }

//     void ShowDrone()
//     {
//         foreach (var r in GetComponentsInChildren<Renderer>())
//             r.enabled = true;

//         foreach (var c in GetComponentsInChildren<Collider>())
//             c.enabled = true;
//     }

// }

