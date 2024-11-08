using CustomCallBacks;
using System;
using TMPro;
using UnityEngine;

public class TimerManager : MonoBehaviour
{
    public static TimerManager instance;
    public const string timerPlayerPrefsKey = "timer";

    public bool startTimerOnStart = false;
    public TMP_Text text = null;

    public SingleFloatCallBack OnTimerStopped = new();

    private bool timerGoing = false;

    public float ElapsedTime { get; private set; }

    private void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        if (timerGoing)
        {
            ElapsedTime += Time.deltaTime;
            if (text != null)
            {
                text.text = TimeSpan.FromSeconds(ElapsedTime).ToString("mm':'ss'.'ff");
            }
        }
    }

    private void Start()
    {
        if (startTimerOnStart)
        {
            StartTimer(PlayerPrefs.GetFloat(timerPlayerPrefsKey, 0f));
        }
    }

    public void StartTimerFromMemory()
    {
        StartTimer(PlayerPrefs.GetFloat(timerPlayerPrefsKey, 0f));
    }

    public void SaveTime()
    {
        PlayerPrefs.SetFloat(timerPlayerPrefsKey, ElapsedTime);
    }

    public void StartTimer(float initialTime = 0f)
    {
        ElapsedTime = initialTime;
        timerGoing = true;
    }

    public void StopTimer()
    {
        timerGoing = false;
        OnTimerStopped.Invoke(ElapsedTime);
    }

    public void PauseTimer()
    {
        timerGoing = false;
    }

    public void UnpauseTimer()
    {
        timerGoing = true;
    }
}
