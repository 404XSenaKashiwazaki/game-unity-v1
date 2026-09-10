using UnityEngine;

[RequireComponent(typeof(DroneController))]
public class DroneAudio : MonoBehaviour
{
    public AudioClip motorLoop;   

    DroneController drone;

    float targetPitch = 1f;
    float currentPitch = 1f;

    float targetVolume = 0.6f;
    float currentVolume = 0.6f;
    [Header("Motor Audio")]
    public float motorVolumeMultiplier = 2.5f; // 1.5 – 2.5 normal
    public static bool isPaused = false;
    public float throttleInput;
    private DroneControls controls;
    private void Awake() {
        controls = new DroneControls();
    }

    void OnEnable()
    {
        controls.Enable();
    }

    void OnDisable()
    {
        controls.Disable();
         if (AudioManager.Instance == null) return;
        StopMotor();
    }
    void Start()
    {
        drone = GetComponent<DroneController>();
    }

    void Update()
    {
        if (AudioManager.Instance == null) return;
        if (!drone.isEngineOn || drone.isCrashed || MissionManager.isMissionComplete || DroneController.isReplaying || MissionManager.isGameOver || PauseManager.IsPaused)
        {
            StopMotor();
            return;
        }

        if (AudioManager.Instance.loopSource.clip != motorLoop)
            AudioManager.Instance.PlayLoop(motorLoop);
            Vector2 move = controls.Drone.Move.ReadValue<Vector2>();
            Vector2 look = controls.Drone.Look.ReadValue<Vector2>();

            float pitchInput = move.y;
            float throttleInput = look.y;

            bool isMoving =
                Mathf.Abs(throttleInput) > 0.1f ||
                Mathf.Abs(pitchInput) > 0.1f;

      
                targetPitch = isMoving ? 1.25f : 1.0f;
        targetVolume = isMoving ? 1.0f : 0.6f;
        currentPitch = Mathf.Lerp(currentPitch, targetPitch, Time.deltaTime * 4f);
        currentVolume = Mathf.Lerp(currentVolume, targetVolume, Time.deltaTime * 4f);
        AudioManager.Instance.loopSource.pitch = currentPitch;
        AudioManager.Instance.loopSource.volume =
        currentVolume
        * motorVolumeMultiplier
        * PlayerPrefs.GetFloat("MASTER_VOLUME", 1f);

    }

    public void StopMotor()
    {
        if (AudioManager.Instance == null) return;
        AudioManager.Instance.StopLoop();
    }

  

}
