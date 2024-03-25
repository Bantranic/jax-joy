using System;
using UnityEngine;
using UnityEngine.Audio;

public class Level_01_Audio_Manager : MonoBehaviour
{
    //public AudioSource sound;
   // public AudioClip hit;


    public sound[] sounds;
    // Start is called before the first frame update
    void Awake()
    {

        foreach(sound s in sounds) 
        {
           s.source =  gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.Loop;
        
        }
        
    }

    public void Play(string name) 
    {
        // Looks through the sounds Array/list and says if the sound in the sound class
        // matches the sound name given in the Audio manager inspector
        // for example if one sound is called "Hit_Sound" then that name in the inspector
        // should also be "Hit_Sound". it will then find the matching sound and play that said sound
        sound s = Array.Find(sounds, sound => sound.name == name);

        if(s != null)
        {
            s.source.Play();
        }
        else 
        {
            Debug.LogWarning("Sound: " + name + "Missing");
        }
       
     
    }

    public void PitchDown(string name, float pitchValue) 
    {

        sound s = Array.Find(sounds, sound => sound.name == name);

        if (s != null) 
        {
            s.source.pitch = pitchValue;
        }
        else 
        {
            Debug.LogWarning("Sound: " + name + "Missing for Pitch");
        }

        
    }


}

   

