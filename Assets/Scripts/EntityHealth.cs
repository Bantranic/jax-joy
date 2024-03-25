using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

// Damage types
public enum EDamageType
{
    StrongFist,
    WeakFist,
    StrongLeg,
    WeakLeg,
}

// Damage States
public enum EDamageState
{
    Neutral,
    Stun,
    KnockBack,
    KnockDown,
    Death,
    Block,
    Roll,
    attack,
}

public class EntityHealth : MonoBehaviour
{

    // Entity health
    public float maxHealth = 100;

    public float currentHealth;


    public float damageResetTime = 1;
    private bool isDamageable = true;
    // Stun 
    public float resetTime = 0;

    // Death
    public float deathTime = 0;
    
    //Player death cout
    private int deathCount = 0;
    // Sets damage state to neutral
    public EDamageState state = EDamageState.Neutral;
    
    // Checking if attack has been done
    public int lastAttackID = 0;

    //How strong the knockback is
    public float knockBackForce;

    public PlayerUI playerUI;


    private void Start()
    {
        currentHealth = maxHealth;

        if(playerUI != null) 
        {
            playerUI.SetMaxHealth();
        }
        
    }

    // Tells player how much damage something took
    public virtual float ApplyDamage(float damage, GameObject causer, EDamageType type)
    {
        //Vector3 currentposition = transform.position;
       
     

        if(isDamageable == true) 
        {

            currentHealth -= damage;
            Debug.Log("HIT");
            isDamageable = false;

        }
        
        if (currentHealth <= 0 && gameObject.tag != "Player")
        {
            Debug.Log("DEATHS");
            Death();
        }
        return damage;
    }

    public void Stun()
    {
        // Checks if health is greater than 0, then sets stun time and applies to entity
        if (currentHealth <= 0 || state > EDamageState.Stun)
            return;

        //Set the Players Health in The UI Script settings
        if (gameObject.CompareTag("Player")) 
        {
            playerUI.SetHealth();

        }

        Debug.Log("stun" + currentHealth);
        state = EDamageState.Stun;

        resetTime = Time.time + 1;

    }

    void Update()
    {

        if (Input.GetKeyDown(KeyCode.T))
        {
            takesdamage(1);
            //health.SetHealth();
        }

        if(isDamageable == false) 
        {
            damageResetTime -= 0.8f * Time.deltaTime;
        
        }

        if(damageResetTime <= 0f) 
        {
            isDamageable = true;
            damageResetTime = 1f;
        }

        // Timer for when stun state reverts back to neutral state
        if (Time.time > resetTime && state == EDamageState.Stun)
            state = EDamageState.Neutral;

        if (state == EDamageState.Death && gameObject.tag != "Player")
        {
            deathTime += 1f * Time.deltaTime;

            if (deathTime >= 4f)
            {
                Destroy(gameObject);
            }

        }
            //Destroy(gameObject);


        if (currentHealth < 0)
        {
            currentHealth = 0;
        }


        if (state == EDamageState.Stun && gameObject.tag == "Player") 
        {
            PlayerController controller = gameObject.GetComponent<PlayerController>();
            Animator animator = gameObject.GetComponent<Animator>();

                animator.SetTrigger("Stun");
                controller.enabled = false;
               state = EDamageState.Neutral;

      
            
            
        
        }

        //Only happens to the player gameobjects
        if(currentHealth == 0 && gameObject.tag == "Player") 
        {

            playerUI.SetHealth();
            PlayerDeath();

        }
        else if(gameObject.tag == "Player")
        {
            PlayerController controller = gameObject.GetComponent<PlayerController>();
            Animator animator = gameObject.GetComponent<Animator>();
            controller.enabled = true;
        }
        
        
    }

    // Death state
    public virtual void Death()
    {

        state = EDamageState.Death;

        deathTime += 1f * Time.deltaTime;

        if(deathTime >= 4f) 
        {
            Destroy(gameObject);
        }
    }

    void PlayerDeath()
    {
        state = EDamageState.Death;

        PlayerController controller = gameObject.GetComponent<PlayerController>();
        var Input = gameObject.GetComponent<PlayerInput>();
        Animator animator = gameObject.GetComponent<Animator>();


        deathTime += 1 * Time.deltaTime;

        if (state == EDamageState.Death)
        {
            controller.isDead = true;
            if(deathCount == 0) 
            { animator.SetTrigger("Death"); }
            

            //Debug.Log(gameObject.name + " is Death");

            deathCount = 1;
        }

        if (deathTime >= 5) 
        {
            animator.SetTrigger("UnDeath");
            controller.isDead = false;

            currentHealth = maxHealth;
            playerUI.SetMaxHealth();
            deathCount = 0;
            deathTime = 0;

        }
        
    }



    public void takesdamage(int damageAmount) 
    {
        currentHealth -= damageAmount;
        Debug.Log("HP = " + currentHealth);
        playerUI.SetHealth();


    }
}