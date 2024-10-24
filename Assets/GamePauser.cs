using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GamePauser : MonoBehaviour
{
    [SerializeField] private KeyCode pauseButton;
    public UnityEvent OnPause;
    public UnityEvent OnUnpause;

    private bool isPaused = false;
    void Update()
    {
        // Check for pause input (e.g., "Escape" key)
        if (Input.GetKeyDown(pauseButton))
        {
            Pause();
        }
    }

    private void OnGUI()
    {
        if (isPaused)
        {
            // Check for input while the game is paused
            if (Event.current.type == EventType.KeyDown)
            {
                if (Event.current.keyCode == pauseButton)
                {
                    Unpause();
                }
            }
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
}
