using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class KillCounter : MonoBehaviour
{
    public int minDominateRequirement;
    public int maxDominateRequirement;
    public UnityEvent dominateEvent;

    private int killCount = 0;
    private int nextDominateRequirement;

    public void GetKill()
    {
        killCount++;
        if (killCount >= nextDominateRequirement)
        {
            killCount = 0;
            dominateEvent.Invoke();
            nextDominateRequirement = Random.Range(minDominateRequirement, maxDominateRequirement + 1);
        }
    }

    private void Awake()
    {
        nextDominateRequirement = Random.Range(minDominateRequirement, maxDominateRequirement + 1);
    }


}
