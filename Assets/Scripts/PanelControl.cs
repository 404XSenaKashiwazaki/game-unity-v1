using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
public class PanelControls : MonoBehaviour
{
    public GameObject PanelControlsCanvas, PanelLevelCanvas, fistButton;
  
    public void BackToLevelPanel ()
    {
        if(PanelControlsCanvas != null) PanelControlsCanvas.SetActive(false);
        if(PanelLevelCanvas != null) PanelLevelCanvas.SetActive(true);
    }

    void Start()
    {
        if(PanelLevelCanvas != null) PanelLevelCanvas.SetActive(false);
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(fistButton);
    }
}