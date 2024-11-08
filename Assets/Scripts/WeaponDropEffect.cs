using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D), typeof(Rigidbody2D))] 
public class WeaponDropEffect : MonoBehaviour
{
    private new Collider2D collider;
    private new Rigidbody2D rigidbody;

    private void Awake()
    {
        collider = GetComponent<Collider2D>();
        collider.enabled = false;
        rigidbody = GetComponent<Rigidbody2D>();
        rigidbody.simulated = false;
    }

    public void Drop()
    {
        transform.parent = null;
        collider.enabled = true;
        rigidbody.simulated = true;
    }
}
