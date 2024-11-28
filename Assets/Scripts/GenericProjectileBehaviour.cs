using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class GenericProjectileBehaviour : MonoBehaviour
{
    public float lifeTime = 5f;
    public float initialMoveSpeed = 1f;
    public float damage = 0f;
    public int pierce = 0;
    public float knockbackForce = 0f;

    public List<string> targetTags = new();

    public UnityEvent ExhaustEvent;
    public float destroyGameObjectSecondsAfterExhaust;

    private Rigidbody2D rb;
    private float timer = 0f;
    private bool exhausted = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = initialMoveSpeed * transform.right;
    }

    private void OnExhaust()
    {
        if (exhausted) return;
        exhausted = true;

        ExhaustEvent.Invoke();
        Destroy(this);
        Destroy(gameObject, destroyGameObjectSecondsAfterExhaust);
    }

    void FixedUpdate()
    {
        timer += Time.fixedDeltaTime;
        if (timer > lifeTime)
        {
            OnExhaust();
            return;
        }

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        OnExhaust();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTags(targetTags)) return;

        if (!other.TryGetComponent<HitpointSystem>(out var hpSys)) return;

        hpSys.Hurt(damage);
        hpSys.AddForce(transform.forward * knockbackForce, ForceMode2D.Impulse);

        pierce--;
        if(pierce <= 0)
        {
            OnExhaust();
            enabled = false;
        }
    }

}
