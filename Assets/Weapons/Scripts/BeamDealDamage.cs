using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utilities;

public class BeamDealDamage : MonoBehaviour
{
    [Header("Targets and Walls")]
    public List<string> enemyTags = new List<string>();
    public LayerMask hitboxLayers;
    public LayerMask wallLayers;

    [Header("Stats")]
    public float damage;
    public float pullForce;

    [Header("Raycasting")]
    public float beamWidth = 2;
    public float maxDistance = 300f;

    [Header("Also Pull the Caster")]
    public Rigidbody2D caster;
    public float reversedRecoilMaxForce;

    private MyRotation m_Rotation;

    Vector2 endPoint;
    Vector2 raycastDir;
    float distance;

    private HashSet<HitpointSystem> GetAllHits()
    {
        float angle = m_Rotation.Value;
        raycastDir = MathUtilities.RotationToVector2(angle);

        
        RaycastHit2D raycastHit = Physics2D.Raycast(transform.position, raycastDir, maxDistance, wallLayers);
        if (raycastHit.collider == null)
        {
            // No Hit
            endPoint = (Vector2)transform.position + raycastDir * maxDistance;
            distance = maxDistance;
        }
        else
        {
            // Hit
            endPoint = raycastHit.point;
            distance = raycastHit.distance;
        }
        Vector2 midPoint = ((Vector2)transform.position + endPoint) / 2;

        var colliders = Physics2D.OverlapBoxAll(midPoint, new Vector2(distance, beamWidth), angle, hitboxLayers);

        HashSet<HitpointSystem> hitpointSystems = new HashSet<HitpointSystem>();
        foreach (var collider in colliders)
        {
            // Check if the hit target has one of the enemy tags.
            bool foundTag = false;
            foreach (string tag in enemyTags)
            {
                if (collider.CompareTag(tag))
                {
                    foundTag = true;
                    break;
                }
            }
            if (!foundTag)
            {
                continue;
            }

            if (collider.TryGetComponent<HitpointSystem>(out var item))
            {
                hitpointSystems.Add(item);
            }
        }

        return hitpointSystems;
    }

    private void HitTargets(HashSet<HitpointSystem> hits)
    {
        foreach (var hit in hits)
        {
            hit.AddForce(-raycastDir * pullForce, ForceMode2D.Impulse);
            hit.Hurt(damage);
        }
    }

    private void Awake()
    {
        m_Rotation = GetComponent<MyRotation>();
    }
    // Update is called once per frame
    void Start()
    {
        var hits = GetAllHits();
        HitTargets(hits);
        if (caster.velocity.y < 0f) caster.velocity = new Vector2(caster.velocity.x, 0f);
        caster.AddForce((1 - (distance / maxDistance)) * reversedRecoilMaxForce * raycastDir, ForceMode2D.Impulse);
    }
}
