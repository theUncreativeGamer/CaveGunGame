using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class FirearmAttack : MonoBehaviour
{
    public enum FirearmAttackType
    {
        SingleShot,
        Automatic
    }

    public FirearmAttackType AttackType { get; protected set; }
    public bool IsFiring { get; protected set; }
    public bool IsOnCooldown { get; protected set; }

    public abstract bool Fire();
    public abstract bool StartFiring();
    public abstract bool StopFiring();
}
