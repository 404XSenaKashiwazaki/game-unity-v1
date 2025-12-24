using UnityEngine;

public class PausedScript : MonoBehaviour
{
    public GameObject GameObjectOptionsPanel;
    public GameObject GameObjectPanel;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Update() {
        Time.timeScale = 0;
    }

    public void GameContinue()
    {
        Time.timeScale = 1f;
        if(GameObjectPanel != null) GameObjectPanel.SetActive(false);
    }

    public void GameOptions ()
    {
        Time.timeScale = 0;
        if(GameObjectPanel != null) GameObjectPanel.SetActive(false);
        if(GameObjectOptionsPanel != null) GameObjectOptionsPanel.SetActive(true);
    }

    public void GameExit()
    {
        Application.Quit();
    }
}
