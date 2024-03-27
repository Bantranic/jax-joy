using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class zone_manager : MonoBehaviour
{

   

    public int Clearamount;
    public int enemies_defeated;

    private float active_delay;
    private float active_delay_max = 5f;

    public GameObject walkTrigger;
    public List<GameObject> spawners;

   // public GameObject childObject;

    public bool isactive = false;
    private bool isCleared = false;
    // Start is called before the first frame update
    public GameObject barrier;
    void Start()
    {
        //barrier = gameObject.GetComponentInChildren<BoxCollider>(); //GetComponent<BoxCollider>();
        barrier.SetActive(false);

        foreach (Transform child in this.transform) 
        {

            if(child.GetComponent<enemy_spawn>() != null) 
            {
                spawners.Add(child.gameObject);
            }
            
        
        }

    }

    private void Update()
    {

        if(isactive == true) 
        {
            barrier.SetActive(true);
            walkTrigger.SetActive(false);
            
            foreach(GameObject s in spawners) 
            {
                s.SetActive(true);

                if(s.GetComponent<enemy_spawn>().enabled == false) 
                {
                    spawners.Remove(s.gameObject);
                
                }
            
            }

            if(spawners.Count == 0) 
            {
                isactive = false;
            }
        
        }
        else 
        {

            barrier.SetActive(false);

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
