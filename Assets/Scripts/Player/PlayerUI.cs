using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    public EntityHealth PlayerHealth;

    public Slider Slider;

    public float HP;
    public float HPmax;

    // Start is called before the first frame update
    private void Awake()
    {
        Slider = gameObject.GetComponent<Slider>();
        HP = PlayerHealth.currentHealth;
        HPmax = PlayerHealth.maxHealth;
    }

    public void SetMaxHealth() 
    {
        float Health = PlayerHealth.currentHealth;
        Slider.maxValue = PlayerHealth.maxHealth;
        Slider.value = Health;
    }

    public void SetHealth() 
    {

        float Health = PlayerHealth.currentHealth;
        if(PlayerHealth != null) 
        {
           // Debug.Log("Health =" + PlayerHealth.currentHealth);
        }
        else 
        {
            Debug.LogError("PLAYER HEALTH IS NULL");
        }

        Slider.value = Health;
    }
}
