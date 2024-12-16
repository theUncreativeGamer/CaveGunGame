using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GamePauser : MonoBehaviour
{
    public static GamePauser instance;

    [SerializeField] private KeyCode pauseButton;
    public UnityEvent OnPause;
    public UnityEvent OnUnpause;
    public UnityEvent OnPrepareEnd;
    public UnityEvent OnEnd;

    private bool isPaused = false;

    private void Awake()
    {
        instance = this;
    }

    void Update()
    {
        // Check for pause input (e.g., "Escape" key)
        if (Input.GetKeyDown(pauseButton))
        {
            Pause();
        }
    }

    public void Pause()
    {
        if (isPaused) return;
        isPaused = true;
        Time.timeScale = 0;
        OnPause.Invoke();
        Cursor.lockState = CursorLockMode.None;
    }

    public void Unpause()
    {
        if (!isPaused) return;
        isPaused = false;
        Time.timeScale = 1;
        OnUnpause.Invoke();
        Cursor.lockState = CursorLockMode.Confined;
    }

    public void EndGame(float delay)
    {
        OnPrepareEnd.Invoke();
        StartCoroutine(ActuallyEndGame(delay));
    }

    private IEnumerator ActuallyEndGame(float delay)
    {
        yield return new WaitForSeconds(delay);
        isPaused = true;
        Time.timeScale = 0;
        OnEnd.Invoke();
        Cursor.lockState = CursorLockMode.None;
    }
}
