using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleMeleeAttack : MonoBehaviour
{
    [Header("Enemy")]
    public List<string> enemyTags;

    [Header("Stats")]
    public float attackInterval = 5;
    public float damage = 2;
    [Tooltip("The direction and force to sent the enemy flying when this attack hit them, assuming you are facing right.")]
    public Vector2 knockback;

    [Header("Object References")]
    [Tooltip("This is used to get the direction the attack's caster is facing.")]
    public CharacterAnimationController animationController;
    public GameObject attackEffectPrefab;
    public Vector2 attackEffectOffset = Vector2.zero;

    private float currentCooldown = 0;

    private void OnTriggerStay2D(Collider2D collision)
    {
        // Debug.Log("Touch Something");

        if (currentCooldown <= 0) 
        {
            bool tagMatched = false;
            foreach(string tag in enemyTags)
            {
                if(collision.CompareTag(tag))
                {
                    tagMatched = true;
                    break;
                }
            }

            if (!tagMatched) { return; }

            if (collision.TryGetComponent<HitpointSystem>(out var hp))
            {
                //Debug.Log("Bonk!");
                currentCooldown = attackInterval;

                // Damage and Knockback
                Vector2 actualKnockback = new Vector2(knockback.x * (animationController.IsFacingLeft ? -1 : 1), knockback.y);
                hp.AddForce(actualKnockback, ForceMode2D.Impulse);
                hp.Hurt(damage);

                // Effect
                Vector2 effectPosition = (Vector2)transform.position + Vector2.ClampMagnitude((Vector2)hp.transform.position + attackEffectOffset - (Vector2)transform.position, 1f);
                Quaternion rotation = animationController.IsFacingLeft ? Quaternion.identity : Quaternion.AngleAxis(180, Vector3.up);
                Instantiate(attackEffectPrefab, effectPosition, rotation);
            }

        }
    }

    private void FixedUpdate()
    {
        currentCooldown -= Time.fixedDeltaTime;
    }
}
