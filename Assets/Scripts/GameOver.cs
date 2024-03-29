using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    void Start()
    {
        
    }

    void Update()
    {
        if (Input.GetButton("Fire1")) 
        {
            SceneManager.LoadScene("Main Menu");
        }
     
    }
    public void MenuPress()
    {
        SceneManager.LoadScene("Main Menu");
    }
}
