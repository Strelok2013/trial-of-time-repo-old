using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMusic : MonoBehaviour
{
    public AudioManager m_audioManager;

    public string m_soundName;
    public float targetVolume = 0.4f;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            if(m_audioManager.isPlaying && m_soundName != m_audioManager.m_currentSoundName)
            {
                m_audioManager.ChangeSounds(m_audioManager.m_currentSoundName, m_soundName, targetVolume, m_audioManager.m_timeToFadeSounds);
            }
            if (!m_audioManager.isPlaying)
            {
                m_audioManager.PlaySound(m_soundName);
                m_audioManager.isPlaying = true;
            }
        }
    }
}
