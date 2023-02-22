    // Start is called before the first frame update
﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeTrap : MonoBehaviour
{
    public float m_spikeDamage = 1.0f;
    public float m_spikeStartTimer = 3.0f;
    public float m_spikeKnockback = 5.0f;
    Animator m_spikeAnimator;
    float m_timer;

    bool m_isPlayerPresent = false;
    // Start is called before the first frame update
    void Start()
    {
        m_spikeAnimator = gameObject.GetComponentInChildren<Animator>();
        m_timer = m_spikeStartTimer;
    }

    // Update is called once per frame
    void Update()
    {
        m_timer += Time.deltaTime;
        if(m_isPlayerPresent)
        {
            SpikesUp();
        }
        else
        {
            SpikesDown();
        }
    }


    private void OnTriggerStay(Collider other)
    {
        if(other.GetComponent<PlayerMovement>())
        {

            m_isPlayerPresent = true;
            other.GetComponent<PlayerMovement>().DamagePlayer(-other.transform.forward, m_spikeKnockback, m_spikeDamage);
        }
    }


    private void OnTriggerExit(Collider other)
    {
        m_isPlayerPresent = false;
    }

    void SpikesUp()
    {
        m_spikeAnimator.SetBool("PlayerPresent", true);
    }

    void SpikesDown()
    {
        m_spikeAnimator.SetBool("PlayerPresent", false);
    }
}
