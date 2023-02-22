using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RelicEmber : MonoBehaviour
{
    public float m_lifetime;




    public Vector3 m_targetPos;
    public Vector3 m_velocity;
    public float m_speedModifier;

    float m_timer;


    // Update is called once per frame
    void Update()
    {
        m_timer += Time.deltaTime;

        if(m_timer > m_lifetime)
        {
            gameObject.GetComponent<ParticleSystem>().Stop();
        }
        if(m_timer > m_lifetime + 2.0f)
        {
            Destroy(gameObject);
        }
    }

    private void FixedUpdate()
    {
        transform.position += m_velocity * m_speedModifier * Time.deltaTime;
    }

    public void SetTargetPos(Vector3 target)
    {

        m_targetPos = target;

        m_velocity = Vector3.Normalize(m_targetPos);
    }
}
