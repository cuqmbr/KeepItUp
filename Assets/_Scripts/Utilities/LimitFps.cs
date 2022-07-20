using UnityEngine;

public class LimitFps : MonoBehaviour
{
    [Tooltip("Set to 0 to set targeted framerate to the screen's refresh rate")]
    [Range(-1,240)]
    [SerializeField] private int _targetedFramerate;
    
    
    void Awake()
    {
        Application.targetFrameRate = _targetedFramerate == 0 ? Screen.currentResolution.refreshRate : _targetedFramerate;
    }
}