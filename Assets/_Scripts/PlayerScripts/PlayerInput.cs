using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{
    private Camera _camera => Camera.main;
    [SerializeField] private PlayerController _playerController;

    private void Update()
    {
        // Check mouse input
        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
        {
            _playerController.OnTouch(_camera.ScreenToWorldPoint(Mouse.current.position.ReadValue()));
            return;
        }

        // Check touchscreen input
        if (Touchscreen.current != null && Touchscreen.current.primaryTouch.press.wasPressedThisFrame)
        {
            _playerController.OnTouch(_camera.ScreenToWorldPoint(Touchscreen.current.touches[0].position.ReadValue()));
            return;
        }
    }
}
