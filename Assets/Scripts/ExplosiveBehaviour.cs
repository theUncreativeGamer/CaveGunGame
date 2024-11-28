using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ExplosiveBehaviour : MonoBehaviour
{
    public GameObject explosionEffectPrefab;
    public float explodeRadius = 1;
    public float maxDamage = 2;
    public AnimationCurve damageFallOff;
    public float knockbackForce = 5;
    public AnimationCurve knockbackForceFallOff;
    public bool destroyOnExplode = false;
    public List<string> targetTags = new();
    public List<string> damageTargetTags = new();
    public LayerMask targetLayers;

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Handles.color = new Color(1f, 1f, 0f);
        Handles.DrawWireDisc(transform.position, Vector3.forward, explodeRadius);
    }
#endif

    public void Explode()
    {
        if (explosionEffectPrefab != null)
            Instantiate(explosionEffectPrefab, transform.position, Quaternion.identity);

        if (destroyOnExplode)
            Destroy(gameObject);

        HashSet<Collider2D> hits = new();
        var hitTargets = Physics2D.OverlapCircleAll(transform.position, explodeRadius, targetLayers);
        foreach (var item in hitTargets)
        {
            foreach (string tag in targetTags)
            {
                if (item.CompareTag(tag))
                {
                    hits.Add(item);
                    break;
                }

            }
        }

        foreach (var item in hits)
        {
            if (item.TryGetComponent<HitpointSystem>(out var hitPointSystem))
            {
                Debug.Log(gameObject.name + " hit " + hitPointSystem.ToString());
                Vector2 dirVector = (Vector2)(item.bounds.center - transform.position);
                float distance = dirVector.magnitude;

                Vector2 knockbackVector =
                    knockbackForce * knockbackForceFallOff.Evaluate(distance / explodeRadius) * dirVector.normalized;
                hitPointSystem.AddForce(knockbackVector, ForceMode2D.Impulse);

                foreach (string tag in damageTargetTags)
                {
                    if (item.CompareTag(tag))
                    {
                        float currentDamage = maxDamage * damageFallOff.Evaluate(distance / explodeRadius);
                        hitPointSystem.Hurt(currentDamage);
                        break;
                    }
                }

            }

        }
    }
}
