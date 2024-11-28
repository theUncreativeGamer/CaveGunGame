using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animation))]
public class AnimationPlayer : MonoBehaviour
{
    private new Animation animation;

    private void Awake()
    {
        animation = GetComponent<Animation>();
    }

    public void Play()
    {
        bool result = animation.Play();
        if (!result) Debug.LogWarning("Animation not played.");
    }

}
