using UnityEngine;

public class LimitScript : MonoBehaviour
{
    public GameOverScript gameOverScript;
    public GameObject GameObjectGameOverPanel;
 
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    // Update is called once per frame
   
// private bool triggered = false;

    // private void OnTriggerEnter(Collider other)
    // {
    //     if (triggered) return;

    //     AICarController car = other.GetComponent<AICarController>();
    //     if (car == null) return;

    //     triggered = true;

    //     Debug.Log("GAME OVER: MOBIL MENEMBUS ZONA OPERATOR");

    //     // STOP SEMUA MOBIL (opsional tapi bagus)
    //     Time.timeScale = 0f;

    //     // SERAHKAN KE GAME FLOW
    //     GameFlowManager.Instance.GameOver();
    // }
    private void OnCollisionEnter(Collision other)
{
    
    if (!MissionManager.isGameOver)
    {
        MissionManager.isGameOver = true;
        gameOverScript.ShowGameOver();
        Debug.Log("IT'S ZOVER");
    }
}

}
