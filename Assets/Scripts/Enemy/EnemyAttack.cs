using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAttack : MonoBehaviour
{
    public GameManager m_gameManagerRef;

    [SerializeField]
    private GameObject m_player;
    EnemyMovement m_movement;

    public float m_timeDamage;
    private float m_collisionTimer;
    public float m_collisionTimerLength = 3f;

    public float m_ghostKnockback = 8.0f;

    private float m_chasePauseTimer;
    public float m_chasePauseTimerLength = 2f;

    public bool m_mainMenuGhost = false;

    private void Start()
    {
        m_collisionTimer = m_collisionTimerLength;
        m_chasePauseTimer = m_chasePauseTimerLength;
        m_movement = GetComponent<EnemyMovement>();
        if(!m_mainMenuGhost)
        {
            m_player = m_gameManagerRef.m_playerRef;
        }
    }
    // Update is called once per frame
    void Update()
    {        
            if (!m_mainMenuGhost)
            {
                if (!gameObject.GetComponent<Collider>().enabled)
                {
                    if (m_movement.m_hasAttacked)
                    {
                        m_chasePauseTimer -= Time.deltaTime;
                    }

                    m_collisionTimer -= Time.deltaTime;
                    if (m_collisionTimer <= 0)
                    {
                        m_collisionTimer = m_collisionTimerLength;
                        gameObject.GetComponent<Collider>().enabled = true;
                    }
                    if (m_chasePauseTimer <= 0)
                    {
                        m_chasePauseTimer = m_chasePauseTimerLength;
                        m_movement.m_hasAttacked = false;
                    }
                }
            }
    }

    public void DealTimeDamage()
    {
         Vector3 pushVector;

         pushVector = m_player.transform.position - transform.position;
         pushVector = Vector3.Normalize(pushVector);
         m_gameManagerRef.SubtractTime(m_timeDamage);
         m_player.GetComponent<PlayerMovement>().DamagePlayer(pushVector, m_ghostKnockback, m_timeDamage);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            if (!GameManager.s_gameWon && !GameManager.s_gameLost)
            {
                DealTimeDamage();
                gameObject.GetComponent<Collider>().enabled = false;
                m_movement.m_hasAttacked = true;
            }
        }
    }
}
