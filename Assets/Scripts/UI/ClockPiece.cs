using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClockPiece : MonoBehaviour
{
    public float m_gravity;
    public float m_drag;
    public float m_lifetime;
    public float m_velocityModifier;
    public float m_angularVelocityModifier;
    public bool m_clockwiseRotation;

    float m_timer;
    
    
    Vector2 m_randomVelocity;

    Vector2 m_objectVelocity;

    public GameObject m_centerPoint;

    RectTransform m_uiObjectTransform;

    // Start is called before the first frame update
    void Start()
    {
        if(m_clockwiseRotation)
        {
            m_angularVelocityModifier *= -1;
        }
        m_uiObjectTransform = gameObject.GetComponent<RectTransform>();
        //m_randomVelocity = new Vector2(Random.value, Random.value);
        //m_randomVelocity = m_randomVelocity.normalized;
        //m_randomVelocity *= m_velocityModifier;
        //m_objectVelocity = m_centerPoint.transform.position - transform.position;
        m_objectVelocity = transform.position - m_centerPoint.transform.position;
        m_objectVelocity.Normalize();
        m_objectVelocity *= m_velocityModifier;
    }

    // Update is called once per frame

    void FixedUpdate()
    {
        Vector2 dumbPos = m_uiObjectTransform.position;
        Vector3 dumbRot = new Vector3(0, 0, m_angularVelocityModifier);
        // Velocity = acceleration * time.deltatime
        // position = velocity * time.deltatime


        m_timer += Time.deltaTime;
        //dumbPos.x += dumbPos.x + m_randomVelocity.x * m_velocityModifier;
        if (m_timer < m_lifetime)
        {
            m_objectVelocity += (new Vector2(0, -1) * m_gravity) * Time.deltaTime;
            dumbPos += m_objectVelocity * Time.deltaTime;
            m_uiObjectTransform.position = dumbPos;
            transform.eulerAngles += dumbRot;
        }
        //m_timer += Time.deltaTime;
        //while(m_timer < m_lifetime)
        //{
        //    m_uiObjectTransform.position +=  Vector3.MoveTowards(m_uiObjectTransform.position, new Vector3(m_randomVelocity.x, m_randomVelocity.y), 5);
        //}
    }
}
