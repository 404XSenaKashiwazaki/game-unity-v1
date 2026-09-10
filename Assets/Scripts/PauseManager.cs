using UnityEngine;

public static class PauseManager
{
    public static bool IsPaused { get; private set; }

    public static void Pause()
    {
        IsPaused = true;
        Time.timeScale = 0f;
        AudioListener.pause = true;
    }

    public static void Resume()
    {
        IsPaused = false;
        Time.timeScale = 1f;
        AudioListener.pause = false;
    }
}
