using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerSaver : MonoBehaviour
{
    public void SaveTime()
    {
        TimerManager.instance.SaveTime();
    }
}
