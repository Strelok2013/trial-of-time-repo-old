using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HiddenWall : MonoBehaviour
{
    public float m_detectionRadius = 5f;
    public Vector3 m_detectionSize;


    // Update is called once per frame
    void Update()
    {
        Detection();
        //Physics.OverlapSphere(transform.position, m_detectionRadius);
    }

    public void Detection()
    {
        Collider[] hitColliders = Physics.OverlapBox(gameObject.transform.position, m_detectionSize);

        for (int i = 0; i < hitColliders.Length; i++)
        {
            if (hitColliders[i].transform.CompareTag("Player") && hitColliders[i].GetComponent<PlayerMovement>().m_hasCompass)
            {
                gameObject.SetActive(false);
                //m_transform_target = hitColliders[i].transform;
                //m_bool_playerPresent = true;
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(gameObject.transform.position, m_detectionSize * 2);
    }
}
