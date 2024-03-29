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
     
    }
    public void MenuPress()
    {
        SceneManager.LoadScene("Main Menu");
    }
}
