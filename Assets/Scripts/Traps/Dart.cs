using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dart : MonoBehaviour
{
    public float m_dartSpeed = 3.0f;
    public float m_dartDamage = 1.0f;
    public float m_dartKnockback = 5.0f;

    // Update is called once per frame
    void Update()
    {
        transform.position += transform.forward * m_dartSpeed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("DartTrap"))
        {
            // Do nothing, keep going
        }
        else if(other.CompareTag("Player"))
        {

            other.GetComponent<PlayerMovement>().DamagePlayer(transform.forward, m_dartKnockback, m_dartDamage);

            // Deal damage
            Destroy(gameObject);
        }
        else if(!other.CompareTag("DartTrap"))
        {
            Destroy(gameObject);
        }     
    }
}
