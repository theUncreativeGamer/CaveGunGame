using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.UI.Image;
using Utilities;

[RequireComponent(typeof(MyRotation))]
public class FlashDealDamage : MonoBehaviour
{
    [Header("Targets and Walls")]
    public List<string> enemyTags = new List<string>();
    public LayerMask hitboxLayers;
    public LayerMask wallLayers;

    [Header("Stats")]
    public float maxDamage;
    public float minDamage;
    public float maxKnockback;
    public float minKnockback;

    [Header("Raycasting")]
    public float fovDegrees = 30f;
    [Tooltip("Higher the value, higher the cost and accuracy.")]
    public int raycastCount = 10;
    public float maxDistance = 300f;
    
    private MyRotation m_Rotation;

    private HashSet<HitpointSystem> GetAllHits()
    {
        float myRotation = m_Rotation.Value;

        float angle = myRotation + (fovDegrees / 2);
        float angleIncrease = fovDegrees / raycastCount;
        Vector3 origin = Vector3.zero;

        HashSet<HitpointSystem> hits = new();

        for (int i = 0; i <= raycastCount; i++)
        {
            float angleRad = angle * Mathf.Deg2Rad;
            Vector3 raycastDir = new Vector3(Mathf.Cos(angleRad), Mathf.Sin(angleRad));

            RaycastHit2D[] raycastHits = Physics2D.RaycastAll(transform.position + origin,
                                                             raycastDir,
                                                             maxDistance,
                                                             hitboxLayers | wallLayers);

            // Sort the hits because they actually don't come out sorted from RaycastAll.
            List<RaycastHit2D> hitList = Raycast2DUtil.SortHits(raycastHits);

            foreach (var item in hitList)
            {
                //Debug.Log("Hit "+item.collider.name);
                // Stop checking when detect wall.
                if (LayerUtil.LayerIsInMask(item.collider.gameObject.layer, wallLayers))
                {
                    //Debug.Log("Object " + item.collider.name + " is wall.");
                    break;
                }

                // Check if the hit target has one of the enemy tags.
                bool foundTag = false;
                foreach (string tag in enemyTags)
                {
                    if (item.collider.CompareTag(tag))
                    {
                        foundTag = true;
                        break;
                    }
                }
                if (!foundTag)
                {
                    //Debug.Log("Object " + item.collider.name + " doesn't have the tag.");
                    continue;
                }


                // Store info of hit target to deal with it later.
                HitpointSystem hitpointSystem = item.collider.GetComponent<HitpointSystem>();
                if (hitpointSystem != null)
                {
                    hits.Add(hitpointSystem);
                }
                else
                {
                    //Debug.Log("Object " + item.collider.name + " doesn't have a hitbox system.");
                }
                
            }

            angle -= angleIncrease;
        }

        return hits;
    }
    private void HitTargets(HashSet<HitpointSystem> hits)
    {
        foreach (var hit in hits)
        {
            float distance = (hit.transform.position - transform.position).magnitude;


            float knockbackForce = Mathf.Lerp(maxKnockback, minKnockback, distance / maxDistance);
            float damage = Mathf.Lerp(maxDamage, minDamage, distance / maxDistance);
            Vector3 direction = hit.gameObject.transform.position - transform.position;
            direction.Normalize();

            hit.AddForce(direction * knockbackForce, ForceMode2D.Impulse);
            hit.Hurt(damage);
            
        }
    }


    private void Awake()
    {
        m_Rotation = GetComponent<MyRotation>();
    }

    void Start()
    {
        var hits = GetAllHits();
        HitTargets(hits);
    }

    
}
