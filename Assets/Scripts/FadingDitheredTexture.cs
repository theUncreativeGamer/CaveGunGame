using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadingDitheredTexture : MonoBehaviour
{
    public float lengthSeconds = 5;
    public AnimationCurve curve = AnimationCurve.Linear(0, 1, 1, 0);
    public bool destroyGameObjectWhenFinished = true;

    private new Renderer renderer;
    private float timerSeconds;

    [SerializeField] private bool playOnEnable = false;

    private void Awake()
    {
        renderer = GetComponent<Renderer>();
    }

    private void Start()
    {
        float fullness = curve.Evaluate(0);
        timerSeconds = 0;
        renderer.material.SetFloat("_DitherThreshold", 2 * fullness);
    }

    private void OnEnable()
    {
        if (playOnEnable)
        {
            Start();
        }
    }

    // Update is called once per frame
    void Update()
    {
        timerSeconds += Time.deltaTime;

        if(timerSeconds > lengthSeconds)
        {
            if (destroyGameObjectWhenFinished)
                Destroy(gameObject);
            else
            {
                ChangeFullness();
            }
            return;
        }

        ChangeFullness();
    }

    private void ChangeFullness()
    {
        float fullness = curve.Evaluate(timerSeconds / lengthSeconds);
        renderer.material.SetFloat("_DitherThreshold", 2 * fullness);
    }
}
