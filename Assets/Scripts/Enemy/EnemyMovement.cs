using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    public float m_detectionRange = 5f;
    public float m_slerpingSpeed = 5f;

    public bool m_isPatrolling = true;
    [HideInInspector]
    public bool m_hasAttacked = false;

    public GameManager m_gamemanagerRef;

    Vector3 m_startPos;
    Vector3 m_playerLastPos;
    GameObject m_target;
    NavMeshAgent m_agent;
    public Transform[] m_navPoints;
    private int m_destPoint = 0;

    public Animator m_ghostAnimator;

    public bool m_mainMenuGhost = false;
    // Start is called before the first frame update
    void Start()
    {
        m_startPos = transform.position;
        m_agent = GetComponent<NavMeshAgent>();
        if(!m_mainMenuGhost)
        {
            m_target = m_gamemanagerRef.m_playerRef;
        }
        else
        {
            m_target = null;
        }
        //Disabling auto-braking allows for continous movement between points (ie, it won't slow down as it approaches the destination)
        m_agent.autoBraking = false;

        GoToNextPoint();

    }

    // Update is called once per frame
    void Update()
    {
        DetectPlayer();
       
        if (!m_mainMenuGhost)
        {
            //If the enemy has 'attacked' the player they will remain in place for a set time before either chasing the player again or resuming it's patrolling
            if (!m_hasAttacked)
            {

                if (m_isPatrolling)
                {
                    
                        m_ghostAnimator.SetBool("hasAggro", false);
                        m_agent.destination = m_navPoints[m_destPoint].position;
                    //Choose the next destination point when the agent gets close to the current one
                    if (!m_agent.pathPending && m_agent.remainingDistance < 0.5f)
                    {
                        GoToNextPoint();
                    }
                }
                else
                {
                    
                        //Face the target
                        FaceTarget();
                        m_ghostAnimator.SetBool("hasAggro", true);
                        m_agent.SetDestination(m_target.transform.position);
                }
            }
            else
            {
                m_agent.SetDestination(transform.position);
            }
        }
        else
        {
            m_agent.destination = m_navPoints[m_destPoint].position;

            //Choose the next destination point when the agent gets close to the current one
            if (!m_agent.pathPending && m_agent.remainingDistance < 0.5f)
            {
                GoToNextPoint();
            }
        }     
    }

    void FaceTarget()
    {
        Vector3 direction = (m_target.transform.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * m_slerpingSpeed);
    }

    public void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, m_detectionRange);
    }

    public void GoToNextPoint()
    {
        //returns if no points have been set up
        if(m_navPoints.Length == 0)
        {
            return;
        }
        //Set the agent to go to the currently selected destination
        m_agent.destination = m_navPoints[m_destPoint].position;

        //Choose the next point in the array as the destination, cycling to the start if necessary.
        m_destPoint = (m_destPoint + 1) % m_navPoints.Length;
    }

    public void DetectPlayer()
    {
        Vector3 ghostForward;
        Vector3 ghostToPlayer;

        ghostForward = transform.forward;
        ghostToPlayer = m_target.transform.position - transform.position;

        float distance = ghostToPlayer.magnitude;

        ghostToPlayer = ghostToPlayer.normalized;

        float dot = Vector3.Dot(ghostForward, ghostToPlayer);

        Debug.Log(distance + ", " + m_detectionRange);

        if (GameManager.s_gameWon || GameManager.s_gameLost)
        {
            m_isPatrolling = true;
        }
        else
        {
            if (dot > 0.707f)          
            {
                if(distance <= m_detectionRange)
                {
                    RaycastHit hit;
                    if(Physics.Raycast(transform.position, ghostToPlayer, out hit, m_detectionRange))
                    {
                        if (hit.transform.tag == "Player")
                        {
                            Debug.DrawRay(transform.position, ghostToPlayer * m_detectionRange, Color.yellow);
                            m_isPatrolling = false;

                        }
                        else
                        {
                            Debug.DrawRay(transform.position, ghostToPlayer * m_detectionRange, Color.red);
                            m_isPatrolling = true;
                        }
                    }
                }
                else
                {
                            m_isPatrolling = true;
                }
            }
            else
            {
                m_isPatrolling = true;
            }
        }
    }
}
