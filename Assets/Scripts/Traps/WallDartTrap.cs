using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallDartTrap : MonoBehaviour
{
    public float m_shootTimer;
    public GameObject m_dartReference;
    public float m_dartSpawnDistance = 1.0f;
    public AudioSource m_audioSourceRef;

    float timer = 0.0f;

    public Sound[] m_audioSounds;

    private void Awake()
    {
        m_audioSourceRef = gameObject.GetComponent<AudioSource>();
        /*foreach (Sound sound in m_audioSounds)
        {

            
            sound.m_source.clip = sound.m_clip;
            sound.m_source.volume = sound.m_volume;
            sound.m_source.pitch = sound.m_pitch;
            sound.m_source.loop = sound.m_loop;
        }*/
    }

    private void FixedUpdate()
    {
        timer += Time.deltaTime;

        if(timer > m_shootTimer)
        {
            timer = 0.0f;
            int randomNum = Random.Range(0, 1);
            PlayASound(randomNum);
            Instantiate(m_dartReference, transform.position + transform.forward * m_dartSpawnDistance, transform.rotation);
        }
    }

    public void PlayASound(int randomNumber)
    {
        /*Sound s = m_audioSounds[randomNumber];
        if (s == null)
        {
            return;
        }
        s.m_source.Play();*/
        m_audioSourceRef.Play();
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawIcon(transform.position + transform.forward * m_dartSpawnDistance, "SpawnPosition");
    }
}
