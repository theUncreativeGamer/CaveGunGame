using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using Utilities;

public class SeekingMissileBehaviour : MonoBehaviour
{
    [Tooltip("If this value isn't set, it will be automatically set to the player's hitbox.")]
    public Collider2D targetCollider = null;
    public List<string> enemyTags = new();
    public LayerMask enemyLayers;
    public LayerMask wallLayers;
    public float initialFlyingSpeed = 2;
    public float aiDelay = 0.5f;
    [Tooltip("The amount of time it takes to be destroyed automatically, in case if it never reachs the target.")]
    public float lifeSpanSeconds = 50f;
    public UnityEvent startAiEvents = null;

    [Header("Explosion")]
    public GameObject explosionEffectPrefab;
    public float igniteRadius = 0.8f;
    public float explodeRadius = 1;
    public float damage = 2;
    public float knockbackForce = 5;

    private Seeker seeker;
    private AIPath aiPath;
    private new Rigidbody2D rigidbody;

    public void Explode()
    {
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (LayerUtil.LayerIsInMask(collision.collider.gameObject.layer, wallLayers))
            Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        foreach(var item in enemyTags)
        {
            if(collision.CompareTag(item))
            {
                Destroy(gameObject);
                return;
            }
        }
    }

    private void OnDestroy()
    {
        if (!this.gameObject.scene.isLoaded) return;

        Instantiate(explosionEffectPrefab, transform.position, Quaternion.identity);
        HashSet<Collider2D> hits = new HashSet<Collider2D>();
        var hitTargets = Physics2D.OverlapCircleAll(transform.position, explodeRadius, enemyLayers);
        foreach(var item in hitTargets)
        {
            foreach(string tag in enemyTags)
            {
                if (item.CompareTag(tag))
                {
                    hits.Add(item);
                    break;
                }

            }
        }

        foreach(var item in hits)
        {
            if(item.TryGetComponent<HitpointSystem>(out var hitPointSystem))
            {
                Debug.Log("Arrow hit " + hitPointSystem.ToString());
                Vector2 dir = (Vector2)(item.bounds.center - transform.position).normalized * knockbackForce;

                hitPointSystem.AddForce(dir, ForceMode2D.Impulse);
                hitPointSystem.Hurt(damage);
            }
            
        }
    }

    private void Awake()
    {
        seeker = GetComponent<Seeker>();
        aiPath = GetComponent<AIPath>();
        rigidbody = GetComponent<Rigidbody2D>();
    }

    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, lifeSpanSeconds);

        if (targetCollider == null)
        {
            var list = GameObject.FindGameObjectsWithTag("Player");
            foreach(var item in list)
            {
                if(item.GetComponent<HitpointSystem>() != null)
                {
                    targetCollider = item.GetComponent<Collider2D>();
                    break;
                }
            }
        }


        if(aiDelay > 0)
        {
            aiPath.enabled = false;
        }
        else
        {
            aiPath.enabled = true;
            seeker.enabled = true;
            aiPath.destination = targetCollider.bounds.center;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (targetCollider == null)
        {
            Destroy(gameObject);
            return;
        }

        aiDelay -= Time.deltaTime;
        if(aiDelay <= 0)
        {
            aiPath.enabled = true;
            seeker.enabled = true;
            aiPath.destination = targetCollider.bounds.center;
            startAiEvents.Invoke();
        }
        else
        {
            rigidbody.velocity = transform.up * initialFlyingSpeed;
        }

        if (Vector2.Distance(transform.position, targetCollider.bounds.center) <= igniteRadius) 
        {
            Destroy(gameObject);
        }
    }
}
