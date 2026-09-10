using UnityEngine;

public class HorizonIndicator : MonoBehaviour
{
    public RectTransform horizonLine;
    public DroneController drone;

   void Update()
{
    float roll = drone.GetRoll();
    float pitch = drone.GetPitch();

    horizonLine.localRotation = Quaternion.Euler(0, 0, -roll);
    horizonLine.anchoredPosition = new Vector2(0, pitch * 2f);
}
}