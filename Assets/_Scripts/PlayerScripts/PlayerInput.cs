using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

public class PlayerInput : MonoBehaviour
{
    private Camera _camera => Camera.main;
    [SerializeField] private PlayerController _playerController;
    [SerializeField] private ScoreManager _scoreManager;
    
    private void Update()
    {
        //Check mouse input
        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
        {
            _playerController.OnTouch(_camera.ScreenToWorldPoint(Mouse.current.position.ReadValue()));
            PlayerEvents.SendScreenTouched();
            return;
        }

        //Check touchscreen input
        if (Touchscreen.current != null && Touchscreen.current.primaryTouch.press.wasPressedThisFrame)
        {
            Debug.Log(_camera.ScreenToWorldPoint(Touchscreen.current.primaryTouch.position.ReadValue()));
            _playerController.OnTouch(_camera.ScreenToWorldPoint(Touchscreen.current.touches[0].position.ReadValue()));
            PlayerEvents.SendScreenTouched();
            return;
        }
    }
}
