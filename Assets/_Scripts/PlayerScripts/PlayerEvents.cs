using System;

public class PlayerEvents
{
    public static event Action OnBallTouched;

    public static event Action OnWallTouched;

    public static event Action OnFloorTouched;

    public static void SendBallTouched() => OnBallTouched?.Invoke();

    public static void SendWallTouched() => OnWallTouched?.Invoke();

    public static void SendFloorTouched() => OnFloorTouched?.Invoke();
}
