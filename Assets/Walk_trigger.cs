using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Walk_trigger : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Player")) 
        {
            transform.GetComponentInParent<zone_manager>().isactive = true;
   
        }
        
    }
}
