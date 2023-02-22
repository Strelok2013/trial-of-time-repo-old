using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lock : MonoBehaviour
{
    public byte m_lockType = 0;

    GameObject m_door;
    GameObject m_lock;
    GameObject m_lock2;

    Animator m_lockAnim;
    Animator m_lockAnim2;

    BoxCollider m_blockingBox;

    bool m_lockOpen = false;

    float m_animDuration = 4.4f;
    float m_lockDropDuration = 1.9f;
    float m_animTimer = 0.0f;

    public AudioSource m_audioSource;
    // Basically the lock should play its animaton then disappear

    // Start is called before the first frame update
    void Start()
    {
        m_blockingBox = gameObject.GetComponent<BoxCollider>();
        m_lock = transform.GetChild(0).gameObject;
        m_lock2 = transform.GetChild(2).gameObject;
        m_lockAnim = transform.GetChild(0).GetComponent<Animator>();
        m_lockAnim2 = transform.GetChild(2).GetComponent<Animator>();
        m_door = transform.Find("Door 1").gameObject;
        m_door.GetComponent<DoorAnim>().LockDoor();
    }

    // Update is called once per frame
    void Update()
    {
        if(m_lockOpen)
        {
            m_animTimer += Time.deltaTime;
            if(m_animTimer > m_lockDropDuration)
            {
                Destroy(m_blockingBox);
            }
            if(m_animTimer > m_animDuration)
            {
                DestroyLock();
            }
        }
    }

    public void RattleDoor()
    {
        if(!m_lockOpen)
        {
            m_audioSource.Play();
        }
    }

    public void UnlockDoor(byte keyType)
    {
        if(keyType == m_lockType)
        {
            m_lockAnim.SetBool("Unlock", true);
            m_lockAnim2.SetBool("Unlock", true);
            m_animTimer += Time.deltaTime;
            m_door.GetComponent<DoorAnim>().UnlockDoor();
            m_door.GetComponent<BoxCollider>().isTrigger = true;
            m_lockOpen = true;
        }
    }

    public void DestroyLock()
    {
        Destroy(m_lock);
        Destroy(m_lock2);
    }

    private void OnCollisionEnter(Collision collision)
    {
        RattleDoor();
    }

    
}
