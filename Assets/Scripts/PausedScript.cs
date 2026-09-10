using UnityEngine;
using UnityEngine.SceneManagement;

public class PausedScript : MonoBehaviour
{
    public GameObject GameObjectOptionsPanel;
    public GameObject GameObjectPanel,panelOsd,panelNoImage;
     private DroneAudio droneAudio;
     void OnEnable()
    {
        if(panelOsd != null) panelOsd.SetActive(false); 
        if(panelNoImage != null) panelNoImage.SetActive(false);
        PauseManager.Pause();
       
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Update() {
        Time.timeScale = 0;
    }

    public void GameContinue()
    {
        Time.timeScale = 1f;
        PauseManager.Resume();
        if(panelOsd != null) panelOsd.SetActive(true); 
        if(panelNoImage != null) panelNoImage.SetActive(true);
        if(GameObjectPanel != null) GameObjectPanel.SetActive(false);
    }

    public void GameOptions ()
    {
        Time.timeScale = 0;
        if(GameObjectPanel != null) GameObjectPanel.SetActive(false);
        if(GameObjectOptionsPanel != null) GameObjectOptionsPanel.SetActive(true);
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
