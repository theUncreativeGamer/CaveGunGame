using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightDroneAttack : MonoBehaviour
{
    public GameObject projectilePrefab;
    public LightDroneAnimationController animationController;
    public float attackIntervalSeconds = 3;

    private float timer = 0;
    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        while (timer >= attackIntervalSeconds)
        {
            timer -= attackIntervalSeconds;
            Quaternion rotation = Quaternion.Euler(0, 0, animationController.IsFacingLeft ? 90 : -90);
            Instantiate(projectilePrefab, transform.position, rotation);
        }
    }
}
