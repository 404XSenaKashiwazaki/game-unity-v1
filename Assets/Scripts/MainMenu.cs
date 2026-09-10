using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
public class MainMenu : MonoBehaviour
{
    public GameObject GameObjectPanel;
    public GameObject GameObjectOptionsPanel;
    public GameObject firstButton;
    
    void Start() {
        if (GameObjectOptionsPanel != null)
        GameObjectOptionsPanel.SetActive(false);

        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(firstButton);
    }

    public void GamePlay()
    {
        MissionManager.ResetMissionState();
        Time.timeScale = 1f;
        SceneManager.LoadScene("LevelScene");
    }

    public void GameOptions ()
    {
        if(GameObjectPanel.activeInHierarchy)
        {
            GameObjectPanel.SetActive(false); 
            GameObjectOptionsPanel.SetActive(true);
        }
    }

    public void BackToMainMenu ()
    {
        if(GameObjectOptionsPanel.activeInHierarchy)
        {
            GameObjectPanel.SetActive(true); 
            GameObjectOptionsPanel.SetActive(false);
            
        }
    }
    
    public void GameExit()
    {
        Application.Quit();
    }
}
