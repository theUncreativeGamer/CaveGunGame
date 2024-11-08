using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class VolumeSetter : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private List<Slider> audioSliders;

    public void SetVolume(float volume, string sliderType)
    {
        audioMixer.SetFloat(sliderType, Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat(sliderType, volume);
    }

    private void Awake()
    {
        foreach (var slider in audioSliders)
        {
            slider.value = PlayerPrefs.GetFloat(slider.gameObject.name, 1);
            audioMixer.SetFloat(slider.gameObject.name, Mathf.Log10(slider.value) * 20);
            slider.onValueChanged.AddListener(value => SetVolume(value, slider.gameObject.name));
        }
    }
}
