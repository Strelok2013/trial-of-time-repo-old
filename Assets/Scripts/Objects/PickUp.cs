using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : MonoBehaviour
{
    public  PickUpScriptable m_pickUpScriptable;

    GameObject m_playerRootObject;

    Vector3 m_oldRotation;
    // Start is called before the first frame update
    void Start()
    {
        m_oldRotation = transform.eulerAngles;
    }



    public void PickUpObject(GameObject targetGameObject)
    {
        if(gameObject.tag == "Weighted")
        {
            gameObject.GetComponent<BoxCollider>().enabled = false; 
        }
        m_playerRootObject = targetGameObject;
        transform.parent = targetGameObject.GetComponent<PlayerPickUp>().m_playerHand;
        transform.localEulerAngles = new Vector3(0, 0, -90);
        transform.localPosition = new Vector3(0, 0, 0);
    }

    public void Drop()
    {
        if (transform.parent != null)
        {
            if (gameObject.tag == "Weighted")
            {
                gameObject.GetComponent<BoxCollider>().enabled = true;
            }
            transform.eulerAngles = m_oldRotation;
            transform.position = m_playerRootObject.GetComponent<PlayerPickUp>().GetDropPos();
            transform.parent = null;
            m_playerRootObject = null;
        }
    }

    //private void OnCollisionEnter(Collision collision)
    //{
    //    if(m_pickUpScriptable.m_type == 0)
    //    {
    //        if(collision.gameObject.GetComponent<Altar>() != null)
    //        {
    //            collision.gameObject.GetComponent<Altar>().RegainTime();
    //            transform.parent.GetComponent<PlayerPickUp>().RemoveCurrentObject();
    //            Destroy(gameObject);
    //        }
    //    }
    //    if(m_pickUpScriptable.m_type == 1)
    //    {
    //        if(collision.gameObject.GetComponent<Lock>() != null)
    //        {
    //            collision.gameObject.GetComponent<Lock>().UnlockDoor(m_pickUpScriptable.m_keyType);
    //            transform.parent.GetComponent<PlayerPickUp>().RemoveCurrentObject();
    //            Destroy(gameObject);
    //        }
    //    }
    //}

    private void OnTriggerStay(Collider collider)
    {
        if (m_pickUpScriptable.m_type == 0)
        {
            if (collider.gameObject.GetComponent<Altar>() != null)
            {
                collider.gameObject.GetComponent<Altar>().RegainTime();
                m_playerRootObject.GetComponent<PlayerPickUp>().RemoveCurrentObject();
                Destroy(gameObject);
                gameObject.SetActive(false);   
            }
        }
        if (m_pickUpScriptable.m_type == 1)
        {
            if (collider.gameObject.GetComponent<Lock>() != null)
            {
                collider.gameObject.GetComponent<Lock>().UnlockDoor(m_pickUpScriptable.m_keyType);
                m_playerRootObject.GetComponent<PlayerPickUp>().RemoveCurrentObject();
                Destroy(gameObject);
                gameObject.SetActive(false);
            }
        }
    }


    public GameObject GetPlayerRootObject()
    {
        return m_playerRootObject;
    }
}
