using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CompleteLevel : MonoBehaviour
{
    void OnTriggerEnter(Collider YeWhoEnters)
    {
        if (YeWhoEnters.gameObject.tag == "Player")
        {
            SceneManager.LoadScene("Main Menu");
        }
    }

}
