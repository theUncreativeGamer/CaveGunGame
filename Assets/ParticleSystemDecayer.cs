using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class ParticleSystemDecayer : MonoBehaviour
{
    public float decayTimeSeconds;
    public float deleteTimeSeconds;
    public AnimationCurve particleSizeOverTime;
    public float initialParticleSize;
    public float endParticleSize;

    private new ParticleSystem particleSystem;
    private float timer = 0f;
    ParticleSystem.MainModule particleMain;
    ParticleSystem.EmissionModule emission;

    private void Start()
    {
        particleSystem = GetComponent<ParticleSystem>();
        particleMain = particleSystem.main;
        emission = particleSystem.emission;
        Destroy(gameObject, decayTimeSeconds + deleteTimeSeconds);
    }
    private void Update()
    {
        timer += Time.deltaTime;
        if (timer<decayTimeSeconds)
        {
            float timeRatio = particleSizeOverTime.Evaluate(timer / decayTimeSeconds);
            float size = Mathf.Lerp(initialParticleSize, endParticleSize, timeRatio);
            particleMain.startSize = size;            
        }
        else
        {
            transform.parent = null;
            emission.enabled = false;
            enabled = false;
        }


    }
}
