using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager: MonoBehaviour
{
    public Sound[] soundList;

    public static SoundManager instance;

    public GameObject audioFollowerObject;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);

        //Setup soundList sounds
        foreach (Sound sound in soundList)
        {
            sound.audioSource = gameObject.AddComponent<AudioSource>();
            sound.audioSource.clip = sound.audioClip;
            sound.audioSource.volume = sound.volume;
            sound.audioSource.pitch = sound.pitch;
            sound.audioSource.spatialBlend = sound.spatialBlend;
            sound.audioSource.loop = sound.loop;
        }
    }

    public GameObject PlaySound(string name, Vector3 position, GameObject parent)
    {
        Sound soundNew = Array.Find(soundList, sound => sound.name == name);
        if(position == Vector3.zero)
        {
            if (soundNew != null)
            {
                soundNew.audioSource.Play();
                return null;
            }
            else
            {
             //   Debug.LogWarning("Sound " + name + " not found");
            }
        }
        else
        {
            GameObject soundGameObject = Instantiate(audioFollowerObject, position, Quaternion.identity);
            //objectAudioSource = soundNew.audioSource;
            soundGameObject.GetComponent<AudioSource>().clip = soundNew.audioSource.clip;
            soundGameObject.GetComponent<AudioSource>().volume = soundNew.audioSource.volume;
            soundGameObject.GetComponent<AudioSource>().pitch = soundNew.audioSource.pitch;
            soundGameObject.GetComponent<AudioSource>().spatialBlend = soundNew.audioSource.spatialBlend;
            soundGameObject.GetComponent<AudioSource>().loop = soundNew.audioSource.loop;

            if (parent != null)
            {
                soundGameObject.transform.parent = parent.transform;
            }
            soundGameObject.GetComponent<AudioSource>().PlayOneShot(soundNew.audioClip);
            return soundGameObject;
        }

        return null;
    }

    public float GetClipDuration(string name)
    {
        Sound soundNew = Array.Find(soundList, sound => sound.name == name);
        if (soundNew != null)
        {
       //     Debug.Log("The duration of the clip: " + name + " is: " + soundNew.audioClip.length);
            return soundNew.audioClip.length;
        }
        return 0.1f;
    }

    public bool PlaySoundOnce(string name, Vector3 position, GameObject parent, bool hasPosition)
    {
        Sound soundNew = Array.Find(soundList, sound => sound.name == name);
        if (!hasPosition && position == Vector3.zero)
        {
            if (soundNew != null)
            {
                if (!soundNew.audioSource.isPlaying)
                {
                    soundNew.audioSource.Play();
                    return true;
                }

            }
            else
            {
           //     Debug.LogWarning("Sound " + name + " not found");
            }
        }

        else
        {
            GameObject soundGameObject = Instantiate(audioFollowerObject, position, Quaternion.identity);
            //objectAudioSource = soundNew.audioSource;
            soundGameObject.GetComponent<AudioSource>().clip = soundNew.audioSource.clip;
            soundGameObject.GetComponent<AudioSource>().volume = soundNew.audioSource.volume;
            soundGameObject.GetComponent<AudioSource>().pitch = soundNew.audioSource.pitch;
            soundGameObject.GetComponent<AudioSource>().spatialBlend = soundNew.audioSource.spatialBlend;
            soundGameObject.GetComponent<AudioSource>().loop = soundNew.audioSource.loop;
            //  soundGameObject.GetComponent<AudioFollower>().isLooping = soundNew.audioSource.loop;
              soundGameObject.GetComponent<AudioFollower>().lifeTime = soundNew.audioSource.clip.length;
              soundGameObject.GetComponent<AudioFollower>().lifeTimeCounter = soundNew.audioSource.clip.length;

            if (parent != null)
            {
                soundGameObject.transform.parent = parent.transform;
            }
            if (!soundGameObject.GetComponent<AudioSource>().isPlaying)
            {
                soundGameObject.GetComponent<AudioSource>().PlayOneShot(soundNew.audioClip);
            }
        }
        return false;
    }

    public void StopSound(string name)
    {
        
        Sound soundNew = Array.Find(soundList, sound => sound.name == name);
        if (soundNew != null)
        {
            soundNew.audioSource.Stop();
        }
        else
        {
       //     Debug.LogWarning("Sound " + name + " not found");
        }

    }

    public void PauseSound(string name)
    {
        Sound soundNew = Array.Find(soundList, sound => sound.name == name);
        if (soundNew != null)
        {
            soundNew.audioSource.Pause();
        }
        else
        {
     //       Debug.LogWarning("Sound " + name + " not found");
        }
    }

}
