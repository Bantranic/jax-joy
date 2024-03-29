using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mantis_Attack : MonoBehaviour
{

    public GameObject hitbox;

   

    // Update is called once per frame
    public void ActiveHitbox() 
    {
        hitbox.SetActive(true);
        
    }

    public void DeactiveHitbox()
    {
        hitbox.SetActive(false);

    }
}
