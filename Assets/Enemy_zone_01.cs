using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_zone : MonoBehaviour
{

    public int Clearamount;
    public int enemies_defeated;

    private float active_delay;
    private float active_delay_max = 5f;


    public bool isactive = false;
    private bool isCleared = false;
    // Start is called before the first frame update
    private BoxCollider boxCollider;
    void Start()
    {
        boxCollider = gameObject.GetComponent<BoxCollider>();

        
    }

    // Update is called once per frame
    void Update()
    {


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
        
       }


        
         
         */

    }
}
