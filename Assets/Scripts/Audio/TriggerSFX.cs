using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class TriggerSFX : MonoBehaviour
{
    public AudioSource m_audio;
    public bool m_isSpecialSFX = false;


    private void OnTriggerEnter(Collider other)
    {
        if(!m_isSpecialSFX)
        {
            if(!m_audio.isPlaying)
            {
                m_audio.Play();
            }
        }
        else
        {
            if(other.CompareTag("Relic"))
            {
                m_audio.Play();
            }
        }
    }
}
