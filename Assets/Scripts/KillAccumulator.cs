using UnityEngine;

public class KillAccumulator : MonoBehaviour
{
    public KillCounter killCounter = null;

    public void AddKill()
    {
        if (killCounter == null)
            PlayerSingleInstance.KillCounterInstance.GetKill();
        else
            killCounter.GetKill();
    }
}
