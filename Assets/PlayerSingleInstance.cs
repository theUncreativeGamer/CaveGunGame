using UnityEngine;

public class PlayerSingleInstance : MonoBehaviour
{
    public static HitpointSystem HitpointInstance;
    public static KillCounter KillCounterInstance;

    [SerializeField] private HitpointSystem playerHitpoint;
    [SerializeField] private KillCounter playerKillCounter;


    void Start()
    {
        if (HitpointInstance == null) HitpointInstance = playerHitpoint;
        if (KillCounterInstance == null) KillCounterInstance = playerKillCounter;
    }

}
