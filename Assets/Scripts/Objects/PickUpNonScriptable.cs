using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpNonScriptable : MonoBehaviour
{

    // Start is called before the first frame update

    public void PickUp(GameObject targetGameObject)
    {
        transform.parent = targetGameObject.transform;
    }

    public void Drop()
    {
        if(transform.parent != null)
        {
            transform.parent = null;
        }
    }

}
