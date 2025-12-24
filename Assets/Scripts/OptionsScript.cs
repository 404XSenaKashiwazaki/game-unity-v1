using UnityEngine;
using UnityEngine.SceneManagement;

public class OptionsScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public GameObject GameObjectPanel;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Update() {
        Time.timeScale = 0;
        Debug.Log("Slider value realtime = " );
    }

   
    public void GameMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
        if(GameObjectPanel != null) GameObjectPanel.SetActive(false);
    }

    public void GameSfx()
    {
         Debug.Log("Slider value realtime = " );
    }
    public void GameContinue()
    {
        Time.timeScale = 1f;
        if(GameObjectPanel != null) GameObjectPanel.SetActive(false);
        // SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
 public void GameExit()
    {
        Application.Quit();
    }
}
