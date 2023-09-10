using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class LightDarkRifleAttack : FirearmAttack
{
    [Header("Transforms")]
    public Transform flashStartingTransform;
    public Transform beamStartingTransform;

    [Header("Prefabs")]
    public GameObject flashPrefab;
    public GameObject beamPrefab;

    [Header("Stats")]
    public float cooldownSeconds = 1;

    [Header("Recoil")]
    public Rigidbody2D recoilReceiver;
    public float recoilForce;
    public float reversedRecoilMaxForce;


    private float currentCooldown = 0;

    // This is to make sure that new attacks always render on top of old ones.
    private int nextAttackSortingIndex = 0;
    public override bool Fire()
    {
        if(currentCooldown>0)
        {
            return false;
        }

        currentCooldown = cooldownSeconds;
        Vector3 recoil = gameObject.transform.forward * recoilForce * -1;
        //Debug.Log(recoil.ToString());
        recoilReceiver.AddForce(recoil, ForceMode2D.Impulse);

        Vector3 flashPosition = new Vector3(flashStartingTransform.position.x, flashStartingTransform.position.y, 2);
        Quaternion newStuffRotation = Quaternion.FromToRotation(Vector3.right, transform.forward);
        
        GameObject stuff = Instantiate(flashPrefab, flashPosition, newStuffRotation);
        stuff.GetComponent<SortingGroup>().sortingOrder = nextAttackSortingIndex;
        nextAttackSortingIndex++;
        //Debug.Log("Current rotation: " + transform.rotation.eulerAngles + "; Instantiated object rotation: " + stuff.transform.rotation.eulerAngles);


        return true;
    }

    public bool AltFire()
    {
        if (currentCooldown > 0)
        {
            return false;
        }

        currentCooldown = cooldownSeconds;

        Vector3 beamPosition = new Vector3(beamStartingTransform.position.x, beamStartingTransform.position.y, 2);
        Quaternion newStuffRotation = Quaternion.FromToRotation(Vector3.right, transform.forward);

        GameObject stuff = Instantiate(beamPrefab, beamPosition, newStuffRotation);
        stuff.GetComponent<SortingGroup>().sortingOrder = nextAttackSortingIndex;
        nextAttackSortingIndex++;

        if(stuff.TryGetComponent<BeamDealDamage>(out var item))
        {
            item.caster = recoilReceiver;
            item.reversedRecoilMaxForce = reversedRecoilMaxForce;
        }

        return true;
    }


    public override bool StartFiring()
    {
        return false;
    }

    public override bool StopFiring()
    {
        return false;
    }

    private void Update()
    {
        currentCooldown -= Time.deltaTime;
    }
}
