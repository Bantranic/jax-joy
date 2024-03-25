using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{

    public AudioSource sound;
    public AudioClip start, exit;
    public Image white;
    private Color c;
    private bool isWhite = false;

    private void Awake()
    {
        c = white.color;
        c.a = 0;

    }
    public void StartGame()
    {

        isWhite = true;
        sound.clip = start;
        sound.Play();
        
    }


    private void Update()
    {
        if(isWhite == true) 
        {
            c.a += 0.1f;

        }

        if(c.a >= 1f) 
        {
            SceneManager.LoadScene("SampleScene");
        }
    }
    public void OnQuit()
    {
        sound.clip = exit;
        sound.Play();
        Application.Quit();
    }
}
