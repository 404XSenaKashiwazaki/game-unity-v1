using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameSettingsScript : MonoBehaviour
{
    [Header("Settings drone")]
    public Slider liftSlider;
    public Slider speedSlider;
    public Slider yawSlider;
    public Slider tiltSlider;
    public Slider controlSlider;
    public Slider dampingSlider;
    DroneController drone;
    public GameObject GameObjectOptionsPanel;
    public GameObject GameObjectSettingsPanel,panelOsd,panelNoImage;

    void OnEnable()
    {
        drone = FindFirstObjectByType<DroneController>();

        liftSlider.SetValueWithoutNotify(PlayerPrefs.GetFloat("LIFT", 7f));
        speedSlider.SetValueWithoutNotify(PlayerPrefs.GetFloat("SPEED", 15f));
        yawSlider.SetValueWithoutNotify(PlayerPrefs.GetFloat("YAW", 80f));
        tiltSlider.SetValueWithoutNotify(PlayerPrefs.GetFloat("TILT", 180f));
        controlSlider.SetValueWithoutNotify(PlayerPrefs.GetFloat("CONTROL", 5f));
        dampingSlider.SetValueWithoutNotify(PlayerPrefs.GetFloat("DAMPING", 2f));

        Apply();
    }

    

    public void Apply()
    {
        
        if (drone == null) return;
        drone.liftForce    = liftSlider.value;
        drone.forwardSpeed = speedSlider.value;
        drone.strafeSpeed  = speedSlider.value;
        drone.yawSpeed     = yawSlider.value;
        drone.maxTiltAngle = tiltSlider.value;
        drone.tiltSpeed    = controlSlider.value;
        drone.rollDamping  = dampingSlider.value;
        PlayerPrefs.SetFloat("LIFT", liftSlider.value);
        PlayerPrefs.SetFloat("SPEED", speedSlider.value);
        PlayerPrefs.SetFloat("YAW", yawSlider.value);
        PlayerPrefs.SetFloat("TILT", tiltSlider.value);
        PlayerPrefs.SetFloat("CONTROL", controlSlider.value);
        PlayerPrefs.SetFloat("DAMPING", dampingSlider.value);
  
        PlayerPrefs.Save();
        
    }

    public void BackToOptions()
    {
        if (GameObjectOptionsPanel != null)
            GameObjectOptionsPanel.SetActive(true);
        if (GameObjectSettingsPanel != null)
            GameObjectSettingsPanel.SetActive(false);
    }

    public void BackToGame()
    {
        Time.timeScale = 1f;
        PauseManager.Resume();
        if(panelOsd != null) panelOsd.SetActive(true); 
        if(panelNoImage != null) panelNoImage.SetActive(true);
        if (GameObjectSettingsPanel != null)
            GameObjectSettingsPanel.SetActive(false);
    }

    public void MainMenu()
    {
        PauseManager.Resume(); 
        Time.timeScale = 1f;

        if (AudioManager.Instance != null)
            AudioManager.Instance.StopAllAudio();

        SceneManager.LoadScene("MainMenu");
    }


    public void GameExit()
    {
        Application.Quit();
    }
}
