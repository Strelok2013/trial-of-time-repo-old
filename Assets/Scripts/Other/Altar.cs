using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Altar : MonoBehaviour
{
    public float m_float_timeRegained = 1f;
    public GameManager m_gameManagerRef;
    public GameObject m_relicEmber;
    public Transform m_emberSpawnPos;
    public Transform m_clockWorldpos;





    public void RegainTime()
    {
        m_gameManagerRef.AddTime(m_float_timeRegained);
        ShootRelicEmbers();
    }

    public void ShootRelicEmbers()
    {
        GameObject relicEmber = m_relicEmber;

        relicEmber.GetComponent<RelicEmber>().SetTargetPos(m_clockWorldpos.position - transform.position);


        Instantiate(relicEmber, m_emberSpawnPos.position, transform.rotation);
    }
    
}
