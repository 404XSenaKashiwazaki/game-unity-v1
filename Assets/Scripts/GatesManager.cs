using UnityEngine;

public class GateManager : MonoBehaviour
{
    public GameObject level1Gates;
    public GameObject level2Gates;

    private void Start()
    {
        LoadLevelGates();
    }

    public void LoadLevelGates()
    {
        level1Gates.SetActive(Level.CurrentLevel == 1);
        level2Gates.SetActive(Level.CurrentLevel == 2);
    }
}