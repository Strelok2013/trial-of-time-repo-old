using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{

    public InventorySystem m_inventory;

    public GameObject m_inventorySlot1, m_inventorySlot2, m_inventorySlot3;

    // Start is called before the first frame update
    void Start()
    {
        m_inventory = gameObject.GetComponent<InventorySystem>();
    }

 
}
