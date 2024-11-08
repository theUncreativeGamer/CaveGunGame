using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillAccumulator : MonoBehaviour
{
    public KillCounter killCounter = null;

    private void Start()
    {
        if(killCounter == null)
        {
            killCounter = FindFirstObjectByType<KillCounter>();
        }
    }

    public void AddKill()
    {
        killCounter.GetKill();
    }
}
