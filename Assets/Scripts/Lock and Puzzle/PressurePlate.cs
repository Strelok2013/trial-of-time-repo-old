using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlate : MonoBehaviour
{
    public GameObject m_doorToOpen;
    public AnimationCurve m_doorOpenClose;
    public float m_moveLimit;

    float m_moveAmount;
    Vector3 m_objOriginalPos;
    Vector3 m_currentPos;
    float m_timer;


    bool m_plateDown = false;
    // Start is called before the first frame update
    void Start()
    {
        m_currentPos = transform.localPosition;
        m_objOriginalPos = m_doorToOpen.transform.position;
    }


    void FixedUpdate()
    {
        if(m_plateDown)
        {
            m_timer += Time.deltaTime;
            m_moveAmount = m_doorOpenClose.Evaluate(m_timer);
            m_moveAmount *= m_moveLimit;
            if(m_timer < 1.0f)
            {
                m_doorToOpen.transform.position = m_objOriginalPos + new Vector3(0, m_moveAmount, 0);
            }
            else
            {
                m_timer = 1.0f;
            }
        }
        else
        {
            m_timer -= Time.deltaTime;
            m_moveAmount = m_doorOpenClose.Evaluate(m_timer);
            m_moveAmount *= m_moveLimit;
            if (m_timer > -1.0f)
            {
                m_doorToOpen.transform.position = m_objOriginalPos + new Vector3(0, m_moveAmount, 0);
            }
            else
            {
                m_timer = -1.0f;
            }
        }
    }

    void PlateUp()
    {
        if (m_plateDown)
        {
            transform.position = new Vector3(transform.position.x, m_currentPos.y, transform.position.z);
            m_doorToOpen.GetComponent<BoxCollider>().enabled = true;
            m_plateDown = false;
        }
    }

    void PlateDown()
    {
        if (!m_plateDown)
        {
            transform.position = new Vector3(transform.position.x, m_currentPos.y - 0.04f, transform.position.z);
            m_doorToOpen.GetComponent<BoxCollider>().enabled = false;
            m_plateDown = true;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            PlateDown();
        }
        if(other.CompareTag("Weighted"))
        {
            PlateDown();
        }
    }
    private void OnTriggerExit(Collider other)
    {
        PlateUp();
    }
}
