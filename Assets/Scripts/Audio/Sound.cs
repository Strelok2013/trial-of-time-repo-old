using UnityEngine.Audio;
using UnityEngine;

[System.Serializable]
public class Sound
{
    public string m_name;

    public AudioClip m_clip;

    [Range(0f, 1f)]
    public float m_volume;

    [Range(0.3f, 3f)]
    public float m_pitch;

    [HideInInspector]
    public AudioSource m_source;

    public bool m_loop;
}
