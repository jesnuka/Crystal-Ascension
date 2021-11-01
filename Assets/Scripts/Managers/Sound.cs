using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[Serializable]
public class Sound
{
    public string name;

    [HideInInspector]
    public AudioSource audioSource;

    public AudioClip audioClip;

    public bool loop;

    [Range(0f, 1f)]
    public float volume;
    [Range(0f, 1f)]
    public float pitch;
    [Range(0f, 1f)]
    public float spatialBlend;
}
