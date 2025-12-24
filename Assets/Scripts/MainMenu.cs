using UnityEngine;
using UnityEngine.SceneManagement;
public class MainMenu : MonoBehaviour
{
    public GameObject GameObjectPanel;
    public GameObject GameObjectOptionsPanel;
    
    void Start() {
        if(GameObjectOptionsPanel != null)  GameObjectOptionsPanel.SetActive(false);
    }

    public void GamePlay()
    {
        MissionManager.Reset();
        Time.timeScale = 1f;
        SceneManager.LoadScene("BriefingScene");
    }

    public void GameOptions ()
    {
        if(GameObjectPanel != null)  GameObjectPanel.SetActive(false);
        if(GameObjectOptionsPanel != null) GameObjectOptionsPanel.SetActive(true);
    }
    
    public void GameExit()
    {
        Application.Quit();
    }
}
