using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoicelineController : MonoBehaviour
{
    public List<AudioClip> idleVoicelines = new();
    public List<AudioClip> attackVoicelines = new();
    public List<AudioClip> dominateVoicelines = new();
    public List<AudioClip> jumpVoicelines = new();
    public List<AudioClip> hurtVoicelines = new();
    public List<AudioClip> defeatVoicelines = new();

    public bool playIdleOnStart = true;
    public float idleRequireSeconds = 20f;

    private AudioSource audioSource;
    private float idleTimer;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        idleTimer = idleRequireSeconds;
    }

    private void Start()
    {
        if (playIdleOnStart) PlayIdle();
    }

    private void Update()
    {
        idleTimer -= Time.deltaTime;
        if(idleTimer <= 0 )
        {
            PlayIdle();
        }
    }

    public void PlayIdle()
    {
        audioSource.clip = idleVoicelines[Random.Range(0, idleVoicelines.Count)];
        audioSource.Play();
        idleTimer = idleRequireSeconds;
    }

    public void PlayAttack()
    {
        if (audioSource.isPlaying) return;
        audioSource.clip = attackVoicelines[Random.Range(0, attackVoicelines.Count)];
        audioSource.Play();
        idleTimer = idleRequireSeconds;
    }

    public void PlayDominate()
    {
        audioSource.clip = dominateVoicelines[Random.Range(0, dominateVoicelines.Count)];
        audioSource.Play();
        idleTimer = idleRequireSeconds;
    }

    public void PlayJump()
    {
        if (jumpVoicelines.Count == 0) return;
        audioSource.clip = jumpVoicelines[Random.Range(0, jumpVoicelines.Count)];
        audioSource.Play();
        idleTimer = idleRequireSeconds;
    }

    public void PlayHurt()
    {
        audioSource.clip = hurtVoicelines[Random.Range(0, hurtVoicelines.Count)];
        audioSource.Play();
        idleTimer = idleRequireSeconds;
    }

    public void PlayDefeat()
    {
        audioSource.clip = defeatVoicelines[Random.Range(0, defeatVoicelines.Count)];
        audioSource.Play();
        idleTimer = idleRequireSeconds;
    }
}
