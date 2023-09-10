using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HitpointSystem : MonoBehaviour
{
    [Header("Component References")]
    public Rigidbody2D connectedRigidbody;
    public GameObject parentGameObject;

    [Header("Stats")]
    public float maxHitpoint = 10;
    public float currentHitpoint;
    public float deathDeleteTimeSeconds = 10;

    [Header("Events")]
    public UnityEvent hurtEvent;
    public UnityEvent deathEvent;
    public bool doHurtEventOnDeath = false;

    private void Start()
    {
        currentHitpoint = maxHitpoint;
        BroadcastMessage("OnUpdateHitpoint", currentHitpoint, SendMessageOptions.DontRequireReceiver);
    }
    public void AddForce(Vector2 force, ForceMode2D forceMode)
    {
        if(connectedRigidbody!=null)
            connectedRigidbody.AddForce(force, forceMode);
    }

    public bool Hurt(float damage)
    {
        currentHitpoint -= damage;
        BroadcastMessage("OnUpdateHitpoint", currentHitpoint, SendMessageOptions.DontRequireReceiver);
        if (currentHitpoint<=0)
        {
            if(doHurtEventOnDeath) { hurtEvent.Invoke(); }
            deathEvent.Invoke();
            Destroy(parentGameObject, deathDeleteTimeSeconds);
            Destroy(gameObject);
            return true;
        }
        hurtEvent.Invoke();
        return false;
    }
}
