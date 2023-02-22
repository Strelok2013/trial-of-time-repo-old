using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPickUp : MonoBehaviour
{
    public float m_playerPickUpRange = 3.0f;
    public float m_detectionWidth = 2.0f;
    public Animator m_playerAnimatorRef;
    public Transform m_playerHand;

    bool m_isHoldingObject; // This might work to prevent insta pickup and drop?

    Vector3 m_dropPos;
    Vector3 m_detectionPos;

    GameObject m_currentObject;

    float m_pickupTimer;

    public Sound[] m_pickUpSounds;
    public Sound[] m_dropSounds;

    private void Awake()
    {
        foreach (Sound sound in m_pickUpSounds)
        {
            sound.m_source = gameObject.AddComponent<AudioSource>();
            sound.m_source.clip = sound.m_clip;
            sound.m_source.volume = sound.m_volume;
            sound.m_source.pitch = sound.m_pitch;
            sound.m_source.loop = sound.m_loop;
        }

        foreach (Sound sound in m_dropSounds)
        {
            sound.m_source = gameObject.AddComponent<AudioSource>();
            sound.m_source.clip = sound.m_clip;
            sound.m_source.volume = sound.m_volume;
            sound.m_source.pitch = sound.m_pitch;
            sound.m_source.loop = sound.m_loop;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        m_dropPos = transform.position + transform.forward * m_playerPickUpRange - transform.forward * 0.1f + transform.up * 0.25f;
        m_detectionPos = transform.position + transform.forward * 0.8f;
    }

    // Update is called once per frame
    void Update()
    {
        if(!m_isHoldingObject)
        {
            ObjectDetection();
        }
        else
        {
            DropObject();
        }

        m_dropPos = transform.position + transform.forward * m_playerPickUpRange - transform.forward * 0.1f + transform.up * 0.25f;
        m_detectionPos = transform.position + transform.forward * 0.8f;
    }

    void ObjectDetection()
    {
        LayerMask mask = LayerMask.GetMask("PickUps");
        Collider[] hitColliders = Physics.OverlapSphere(m_detectionPos, m_playerPickUpRange, mask);
        if(hitColliders.Length != 0)
        {
            UIManager.s_showPrompt = true;
            PickUpObject(hitColliders[0].gameObject);
            //foreach(Collider hit in hitColliders)
            //{
            //    // Do magic here
            //    UIManager.s_showPrompt = true;
            //}
        }
        else if (hitColliders.Length > 1)
        {
            RaycastHit sphereHit;
            Vector3 forward = transform.forward;
            Vector3 rayCastOrigin = transform.position - transform.forward * 0.1f;
            Debug.DrawRay(rayCastOrigin, forward * m_playerPickUpRange, Color.red);

            if (Physics.SphereCast(rayCastOrigin, m_detectionWidth, forward, out sphereHit, m_playerPickUpRange))
            {
                if (sphereHit.collider.GetComponent<PickUp>() != null)
                {
                    PickUpObject(sphereHit.collider.gameObject);

                }
            }
            else
            {
                float distanceToPlayer1 = float.PositiveInfinity;
                float distanceToPlayer;
                Collider colliderRef = null;
                foreach(Collider hit in hitColliders)
                {
                    distanceToPlayer = Vector3.Distance(transform.position, hit.transform.position);
                    if(distanceToPlayer1 > distanceToPlayer)
                    {
                        distanceToPlayer1 = distanceToPlayer;
                        colliderRef = hit;
                    }
                    PickUpObject(colliderRef.gameObject);
                }
            }

        }
        else
        {
            UIManager.s_showPrompt = false;
        }

        //RaycastHit sphereHit;
        //Vector3 forward = transform.forward;
        //Vector3 rayCastOrigin = transform.position - transform.forward * 0.1f;
        //Debug.DrawRay(rayCastOrigin, forward * m_playerPickUpRange, Color.red);
        //
        //if (Physics.SphereCast(rayCastOrigin, m_detectionWidth, forward, out sphereHit, m_playerPickUpRange))
        //{
        //    if (sphereHit.collider.GetComponent<PickUp>() != null)
        //    {
        //        UIManager.s_showPrompt = true;
        //        PickUpObject(sphereHit.collider.gameObject);
        //            
        //    }
        //    else
        //    {
        //        UIManager.s_showPrompt = false;
        //    }
        //}
        //else
        //{
        //    UIManager.s_showPrompt = false;
        //}
    }

    public void PlayPickUpSound(int randomNumber)
    {
        Sound s = m_pickUpSounds[randomNumber];
        if (s == null)
        {
            return;
        }
        s.m_source.Play();
    }

    public void PlayDropSound(int randomNumber)
    {
        Sound s = m_dropSounds[randomNumber];
        if (s == null)
        {
            return;
        }
        s.m_source.Play();
    }

    public void RemoveCurrentObject()
    {
        m_playerAnimatorRef.SetTrigger("dropsItem");
        m_currentObject.GetComponent<PickUp>().Drop();
        m_currentObject = null;
        m_isHoldingObject = false;
        UIManager.s_showDropPrompt = false;
    }

    void PickUpObject(GameObject targetObject)
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //if (targetObject.GetComponent<Torch>())
            //{
            //    m_playerAnimatorRef.SetBool("isHoldingTorch", true);
            //}
            int randNum = Random.Range(0, 2);
            PlayPickUpSound(randNum);
            m_playerAnimatorRef.SetTrigger("picksUpItem");
            m_currentObject = targetObject;
            targetObject.GetComponent<PickUp>().PickUpObject(gameObject);
            m_isHoldingObject = true;
            UIManager.s_showPrompt = false;
            UIManager.s_showDropPrompt = true;
        }
    }

    void DropObject()
    {
        
        RaycastHit sphereHit;
        Vector3 forward = transform.forward;
        Vector3 rayCastOrigin = transform.position - transform.forward * 0.1f;
        if (Input.GetKeyDown(KeyCode.Space))
        {
            m_playerAnimatorRef.SetTrigger("dropsItem");
            if (Physics.SphereCast(rayCastOrigin, m_detectionWidth, forward, out sphereHit, m_playerPickUpRange))
            {
                if (sphereHit.collider.GetComponent<PressurePlate>())
                {
                    m_currentObject.GetComponent<PickUp>().Drop();
                    m_currentObject = null;
                    m_isHoldingObject = false;
                    UIManager.s_showDropPrompt = false;
                }
                if (sphereHit.collider.GetComponent<PickUp>() == null)
                {
                    SetDropBehindPlayer();
                }
            }
            int randNum = Random.Range(0, 2);
            PlayDropSound(randNum);
            m_currentObject.GetComponent<PickUp>().Drop();
            m_currentObject = null;
            m_isHoldingObject = false;
            UIManager.s_showDropPrompt = false;
        }
    }

    void SetDropBehindPlayer()
    {
        m_dropPos = transform.position + transform.forward * -1.5f + transform.up * 0.25f;
    }

    public Vector3 GetDropPos()
    {
        return m_dropPos;
    }

    private void OnDrawGizmos()
    {
        Vector3 rayCastOrigin = transform.position - transform.forward * 0.1f;
        Gizmos.DrawWireSphere(rayCastOrigin + transform.forward * m_playerPickUpRange, m_detectionWidth);
        Gizmos.DrawWireSphere(m_detectionPos, m_playerPickUpRange);
        Gizmos.DrawIcon(m_dropPos, "");
    }

    
}
