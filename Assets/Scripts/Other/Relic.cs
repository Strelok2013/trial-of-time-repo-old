using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Relic : MonoBehaviour
{
    [SerializeField]
    private Transform m_target;
    public bool m_playerPresent;
    public bool m_canPickUp = false;
    public bool m_hasRelic = false;

    public float m_detectionRadius = 1f;
    
    public float m_countDown = 0f;
    public float m_pickUpTimer = 3f;

    private void Start()
    {
        m_countDown = m_pickUpTimer;
        m_target = PlayerManager.s_instance.m_gameObject_player.transform;
    }

    // Update is called once per frame
    void Update()
    {
        //Detection();

        /*if (m_playerPresent)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                if (m_canPickUp && m_countDown <= 0)
                {
                    PickObject();
                }
                else
                {
                    DropObject();
                }
            }
        }*/
        if (!m_hasRelic)
        {
            m_countDown--;
        }
    }

    public void PickObject()
    {
        transform.parent = m_target;
        m_canPickUp = false;
        m_hasRelic = true;
        m_countDown = m_pickUpTimer;
    }

    public void DropObject()
    {
        transform.parent = null;
        m_hasRelic = false;
    }

    public void Detection()
    {
        if (m_target != null)
        {
            float distanceToPlayer = Vector3.Distance(m_target.position, transform.position);

            if (distanceToPlayer < m_detectionRadius)
            {
                m_playerPresent = true;
            }

            if (distanceToPlayer > m_detectionRadius)
            {
                //m_transform_target = null;
                m_canPickUp = false;
                m_playerPresent = false;
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(gameObject.transform.position, m_detectionRadius);
    }
}
