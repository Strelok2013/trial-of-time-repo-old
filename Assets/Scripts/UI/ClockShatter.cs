using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClockShatter : MonoBehaviour
{

    public static bool s_destroyClock;

    // Start is called before the first frame update


    // Update is called once per frame
    void Update()
    {
        if(s_destroyClock)
        {
            gameObject.SetActive(true);
        }
    }
}
