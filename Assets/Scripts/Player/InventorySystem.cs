using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventorySystem : MonoBehaviour
{
    //public Queue<GameObject> pickedUpItems = new Queue<GameObject>();
    List<GameObject> m_pickedUpItems = new List<GameObject>(2); // Should really intialize this to size of 3, and should really be initialized inside start in case we want 
                                                                // to increase the size of the players inventory
    public float m_KeyHoldLength = 0.5f;

    float m_dropTimerLimit = 2;
    float m_pickUpTimerLimit = 2;

    int m_currentlySelected = 0;
    float m_timer = 0.0f;
    float m_pickUpTimer = 0.0f;
    float m_dropTimer = 0.0f;
    // Update is called once per frame
    void Update()
    {

        CycleSelection();

        m_pickUpTimer += Time.deltaTime;
        m_dropTimer += Time.deltaTime;


        if(Input.GetKey(KeyCode.Space))
        {
            m_timer += Time.deltaTime;
            if(m_timer > m_KeyHoldLength)
            {
                if(m_pickedUpItems[m_currentlySelected])
                {
                    DropObject();
                    m_timer = 0.0f;
                }
            }
        }
        else
        {
            m_timer = 0.0f;
        }

    }

    /// <summary>
    /// Welp, looks like I have to make this myself...
    /// bummer
    /// </summary>
    public void InventoryUpdate()
    {
        for (int i = 0; i < m_pickedUpItems.Capacity; i++)
        {
            if (!m_pickedUpItems[i].transform.parent)
            {
                m_pickedUpItems[i].transform.parent = transform;
                m_pickedUpItems[i].transform.localPosition = new Vector3(0, 0.5f, 1);
            }
            if (i == m_currentlySelected)
            {
                m_pickedUpItems[i].SetActive(true);
            }
            else
            {
                m_pickedUpItems[i].SetActive(false);
            }
        }
    }

    void CycleSelection()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            m_currentlySelected--;
            if (m_currentlySelected < 0)
            {
                m_currentlySelected = 1;
            }
                InventoryUpdate();
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            m_currentlySelected++;
            if (m_currentlySelected > 1)
            {
                m_currentlySelected = 0;
            }
                InventoryUpdate();
        }
    }


    public void PickObject(GameObject targetObject)
    {
       
        if (m_pickedUpItems.Count < 2) // If the player has less than two objects in the inventory
        {
            if (m_pickUpTimer > m_pickUpTimerLimit)
            {
                if (!m_pickedUpItems.Contains(gameObject)) // If we have absolutely nothing
                {
                    m_pickedUpItems.Add(targetObject);
                    m_pickUpTimer = 0.0f;
                    m_dropTimer = 0.0f;
                    InventoryUpdate();
                }
            }
        }
    }

    public void DropObject()
    {
        if (m_dropTimer > m_dropTimerLimit)
        {
            m_pickedUpItems[m_currentlySelected].gameObject.transform.localPosition = new Vector3(0, 0.5f, 1);
            m_pickedUpItems[m_currentlySelected].gameObject.transform.parent = null;
            m_pickedUpItems[m_currentlySelected].gameObject.SetActive(true);
            m_pickedUpItems.Remove(m_pickedUpItems[m_currentlySelected]);
            m_dropTimer = 0.0f;
            m_pickUpTimer = 0.0f;
            InventoryUpdate();
        }
    }

    public float CalcMoveSpeedModifier()
    {
        float moveSpeedModifier = 0.0f;

        

        return moveSpeedModifier;
    }

    //private void OnDrawGizmos()
    //{
    //    Gizmos.DrawWireSphere(gameObject.transform.position, m_detectionRadius);
    //}
}
