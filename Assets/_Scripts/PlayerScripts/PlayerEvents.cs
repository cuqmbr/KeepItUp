using System;

public class PlayerEvents
{
    public static event Action OnBallTouched;

    public static event Action OnWallTouched;

    public static event Action OnDeath;

    public static void SendBallTouched() => OnBallTouched?.Invoke();

    public static void SendWallTouched() => OnWallTouched?.Invoke();

    public static void SendDeath() => OnDeath?.Invoke();
}