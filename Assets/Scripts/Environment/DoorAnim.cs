using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorAnim : MonoBehaviour
{
    Animator m_doorAnimator;
    bool m_doorLocked = false;

    bool m_doorClockwise;
    bool m_doorCounterClockwise;
    // Start is called before the first frame update
    void Start()
    {

        m_doorAnimator = transform.GetChild(0).GetComponent<Animator>();
    }



    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerMovement>())
        {
            if (!m_doorLocked)
            {
                if (DeterminePlayerPosRelative(other.transform.position) < 0)
                {
                    SwingDoorClockWise();
                }
                if (DeterminePlayerPosRelative(other.transform.position) > 0)
                {
                    SwingDoorAntiClockwise();
                }
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.GetComponent<PlayerMovement>())
        {
            if(!m_doorLocked)
            {
                if(m_doorClockwise)
                {
                    KeepDoorOpenClockwise();
                }
                if(m_doorCounterClockwise)
                {
                    keepDoorOpenCounterClockwise();
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<PlayerMovement>())
        {
            StopDoor();
        }
    }

    void SwingDoorClockWise()
    {
        m_doorCounterClockwise = false; 
        m_doorClockwise = true;
    }

    void SwingDoorAntiClockwise()
    {
        m_doorClockwise = false;
        m_doorCounterClockwise = true;
    }

    void KeepDoorOpenClockwise()
    {
        m_doorAnimator.SetBool("PlayerPresentClockwise", true);
    }

    void keepDoorOpenCounterClockwise()
    {
        m_doorAnimator.SetBool("PlayerPresentCounterclockwise", true);
    }

    void StopDoor()
    {
        m_doorAnimator.SetBool("Counterclockwise", false);
        m_doorAnimator.SetBool("Clockwise", false);
        m_doorAnimator.SetBool("PlayerPresentClockwise", false);
        m_doorAnimator.SetBool("PlayerPresentCounterclockwise", false);
    }

    public void UnlockDoor()
    {
        m_doorLocked = false;  
    }

    public void LockDoor()
    {
        m_doorLocked = true;
    }

    float DeterminePlayerPosRelative(Vector3 playerPos) // So like whether the player is in front or behind the door
    {
        Vector3 playerDirectionFromDoor = playerPos - transform.position;
        return Vector3.Dot(playerDirectionFromDoor, transform.right);
    }

}
