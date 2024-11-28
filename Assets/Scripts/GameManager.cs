using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    private void Awake()
    {
        instance = this;
    }

    public void ResetProgress()
    {
        PlayerPrefs.DeleteKey(TimerManager.timerPlayerPrefsKey);
        PlayerPrefs.DeleteKey(CheckpointManager.PlayerPrefKey);
    }

    public void Reload(float delay)
    {
        Invoke(nameof(ActualReload), delay);
    }

    private void ActualReload()
    {
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
        Time.timeScale = 1f;
    }

    public void TeleportToNextCheckPoint()
    {
        CheckpointManager.instance.TeleportToNextCheckpoint();
    }
}
