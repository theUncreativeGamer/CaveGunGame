using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GamePauser : MonoBehaviour
{
    [SerializeField] private KeyCode pauseButton;
    public UnityEvent OnPause;
    public UnityEvent OnUnpause;
    public UnityEvent OnEnd;

    private bool isPaused = false;
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
    }

    public void Unpause()
    {
        if (!isPaused) return;
        isPaused = false;
        Time.timeScale = 1;
        OnUnpause.Invoke();
    }

    public void EndGame(float delay)
    {
        StartCoroutine(ActuallyEndGame(delay));
    }

    private IEnumerator ActuallyEndGame(float delay)
    {
        yield return new WaitForSeconds(delay);
        isPaused = true;
        Time.timeScale = 0;
        OnEnd.Invoke();
    }
}
