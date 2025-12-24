using UnityEngine;

public class CarGroup : MonoBehaviour
{
    public AICarController[] cars;

    public bool IsActive { get; private set; }

    public void StartGroup()
    {
        IsActive = true;

        foreach (var car in cars)
            car.StartMove();
    }

    public bool IsGroupFinished()
    {
        if (!IsActive) return false;

        foreach (var car in cars)
        {
            if (!car.IsFinished())
                return false;
        }

        return true;
    }
}
