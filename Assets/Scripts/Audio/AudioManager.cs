using UnityEngine.Audio;
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour
{
    public GameObject[] m_gameObjects;
    public Sound[] m_sounds;

    public GameManager m_gameManager;
    public static AudioManager instance;

    private int m_count;
    public string m_currentSoundName;
    public float m_timeToFadeSounds;

    public bool isPlaying = false;
    void Awake()
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

        foreach(Sound sound in m_sounds)
        {
            GameObject m_thisObject = m_gameObjects[m_count];
            //sound.m_source = transform.GetChild(m_childCount).gameObject.AddComponent<AudioSource>();
            sound.m_source = m_thisObject.GetComponent<AudioSource>();
            sound.m_source.clip = sound.m_clip;

            sound.m_source.volume = sound.m_volume;
            sound.m_source.pitch = sound.m_pitch;
            sound.m_source.loop = sound.m_loop;
            sound.m_source.playOnAwake = true;
            m_count++;
        }
    }

    public void ChangeSounds(string currentSound, string nextSound, float targetVol, float timeFade)
    {
        StopAllCoroutines();

        StartCoroutine(CrossFadeSound(currentSound, nextSound, targetVol, timeFade));
    }

    public void PlaySound(string name)
    {
        Sound s = Array.Find(m_sounds, sound => sound.m_name == name);
        if (s == null)
        {
            return;
        }
        m_currentSoundName = s.m_name;
        s.m_source.Play();
    }
    public void StopSound(string name)
    {
        Sound s = Array.Find(m_sounds, sound => sound.m_name == name);
        s.m_source.Stop();
    }

    public IEnumerator CrossFadeSound(string currentSoundName, string nextSoundName, float targetVolume, float timeToFade)
    {
        float timeElapsed = 0;
        Sound currentSound = Array.Find(m_sounds, sound => sound.m_name == currentSoundName);
        Sound nextSound = Array.Find(m_sounds, sound => sound.m_name == nextSoundName);
        float oldVol = currentSound.m_volume;

        PlaySound(nextSoundName);
        
        while(timeElapsed < timeToFade)
        {
            currentSound.m_volume = Mathf.Lerp(oldVol, 0, timeElapsed / timeToFade);
            currentSound.m_source.volume = currentSound.m_volume;

            nextSound.m_volume = Mathf.Lerp(0, targetVolume, timeElapsed / timeToFade);
            nextSound.m_source.volume = nextSound.m_volume;
            timeElapsed += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        StopSound(currentSoundName);
    }
}
