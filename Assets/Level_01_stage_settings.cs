using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level_01_stage_settings : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        FindAnyObjectByType<Level_01_Audio_Manager>().Play("levelTheme");
    }

    private void Update()
    {



        //Test code to change the pitch of a sound with a siimple button Press
        //IT WORKS!!!!
        /*if (Input.GetKeyDown("e")) 
        {

            FindAnyObjectByType<Level_01_Audio_Manager>().PitchDown("levelTheme", 0.3f);
        
        }*/
    }

}
