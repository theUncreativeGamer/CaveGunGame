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
        Cursor.lockState = CursorLockMode.None;
    }

    public void ResetProgress()
    {
        PlayerPrefs.DeleteKey(TimerManager.timerPlayerPrefsKey);
        PlayerPrefs.DeleteKey(CheckpointManager.PPK_Checkpoint);
        LevelLoader.ClearData();
    }

    public void LoadScene(SceneObject scene)
    {
        SceneManager.LoadScene(scene);
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
