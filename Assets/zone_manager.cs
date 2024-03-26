using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class zone_manager : MonoBehaviour
{

    public Enemy_zone enemyZone;

    public int Clearamount;
    public int enemies_defeated;

    private float active_delay;
    private float active_delay_max = 5f;

    public GameObject walkTrigger;
    public GameObject[] spawners;

    public bool isactive = false;
    private bool isCleared = false;
    // Start is called before the first frame update
    private BoxCollider boxCollider;
    void Start()
    {
        boxCollider = gameObject.GetComponentInChildren<BoxCollider>(); //GetComponent<BoxCollider>();
        boxCollider.enabled = false;

    }

    private void Update()
    {

        if(isactive == true) 
        {
            boxCollider.enabled = true;
            walkTrigger.SetActive(false);
            
            foreach(GameObject s in spawners) 
            {
                s.SetActive(true);
            
            }
        
        }
        else 
        {

            boxCollider.enabled = false;

        }



        /*
         
        if (isactive == true)
        {

         if enemies beat == clearAmount
        {
            iscleared = ture
        }
         
        if(iscleared == true)
        {
          activeDelay += 1 * time.deltatime

          If(active Delay <= Max Delay){
             collider.active = false;
             matireal.active = false;
        }
        else
        {
          activeDelay = 0;
        
        }
        
       }*/

    }
}
