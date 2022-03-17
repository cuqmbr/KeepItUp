using System;
using UnityEngine;

public class PlayerEvents
{
    public static event Action OnScreenTouched;

    public static event Action OnWallTouched;

    public static event Action OnFloorTouched;

    public static void SendScreenTouched() => OnScreenTouched?.Invoke();

    public static void SendWallTouched() => OnWallTouched?.Invoke();

    public static void SendFloorTouched() => OnFloorTouched?.Invoke();
}
