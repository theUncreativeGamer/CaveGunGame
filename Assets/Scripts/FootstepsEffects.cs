using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootstepsEffects : MonoBehaviour
{
    public List<AudioClip> walkingSounds;
    public List<AudioClip> jumpingSounds;
    public List<AudioClip> landingSounds;
    [Tooltip("The delay between each walking sound in seconds.")]
    public float walkingInterval;
    public float volume = 5f;


    private float _currInterval = 0;
    private float currentWalkSpeedMultiplier = 1f;
    private bool _doWalk = false;

    public void Walk(float walkSpeedMultiplier = 1f)
    {
        _doWalk = true;
        currentWalkSpeedMultiplier *= walkSpeedMultiplier;
    }

    public void Jump()
    {
        var clipToPlay = jumpingSounds[Random.Range(0, jumpingSounds.Count)];
        AudioSource.PlayClipAtPoint(clipToPlay, transform.position, volume);
    }

    public void Land()
    {
        var clipToPlay = landingSounds[Random.Range(0, landingSounds.Count)];
        AudioSource.PlayClipAtPoint(clipToPlay, transform.position, volume);
    }

    private void LateUpdate()
    {
        _currInterval -= Time.deltaTime * currentWalkSpeedMultiplier;
        if (_doWalk && _currInterval <= 0) 
        {
            var clipToPlay = walkingSounds[Random.Range(0, walkingSounds.Count)];
            AudioSource.PlayClipAtPoint(clipToPlay, transform.position, volume);
            _currInterval = walkingInterval;
        }
        _doWalk = false;
        currentWalkSpeedMultiplier = 1f;
    }
}
