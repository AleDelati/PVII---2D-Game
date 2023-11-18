using System;

public static class GameEvents {

    public static event Action onPause;
    public static event Action onResume;
    public static event Action onLevelCleared;
    public static event Action onGameOver;

    public static void TriggerPause() => onPause?.Invoke();
    public static void TriggerResume() => onResume?.Invoke();
    public static void TriggerLevelCleared() => onLevelCleared?.Invoke();
    public static void TriggerGameOver() => onGameOver?.Invoke();

}
