using UnityEngine;
using UnityEngine.InputSystem;

public class InputIndicator : MonoBehaviour
{
    public InputActionReference moveAction;
    public InputActionReference lookAction;

    public RectTransform moveDot;
    public RectTransform lookDot;

    public float radius = 45f;

    private void OnEnable()
    {
        moveAction.action.Enable();
        lookAction.action.Enable();
    }

    private void OnDisable()
    {
        moveAction.action.Disable();
        lookAction.action.Disable();
    }

    void Update()
{
    Vector2 move = Vector2.zero;
    Vector2 look = Vector2.zero;

    if (moveAction.action.activeControl != null &&
        moveAction.action.activeControl.device is Gamepad)
    {
        move = moveAction.action.ReadValue<Vector2>();
    }

    if (lookAction.action.activeControl != null &&
        lookAction.action.activeControl.device is Gamepad)
    {
        look = lookAction.action.ReadValue<Vector2>();
    }

    moveDot.anchoredPosition = move * radius;
    lookDot.anchoredPosition = look * radius;
}
}