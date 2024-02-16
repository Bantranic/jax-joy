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
    public float health = 100;

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


    // Tells player how much damage something took
    public virtual float ApplyDamage(float damage, GameObject causer, EDamageType type)
    {
        //Vector3 currentposition = transform.position;
        Debug.Log("HTI");
       // currentposition += currentposition +  new Vector3(50, 50, 50);

        //Vector3 direction = (position - transform.position).normalized;

       // Vector3 knockback = direction * knockBackForce;

       // transform.localPosition += knockback; 

        health -= damage;
        if (health <= 0 && gameObject.tag != "Player")
        {
            Death();
        }
        return damage;
    }

    public void Stun()
    {
        // Checks if health is greater than 0, then sets stun time and applies to entity
        if (health <= 0 || state > EDamageState.Stun)
            return;


        Debug.Log("stun" + health);
        state = EDamageState.Stun;

        resetTime = Time.time + 1;

    }

    void Update()
    {
        // Timer for when stun state reverts back to neutral state
        if (Time.time > resetTime && state == EDamageState.Stun)
            state = EDamageState.Neutral;

        if (Time.time > deathTime && state == EDamageState.Death)
           // Destroy(gameObject);


        if (health < 0)
        {
            health = 0;
        }


        if (state == EDamageState.Stun && gameObject.tag == "Player") 
        {
            PlayerController controller = gameObject.GetComponent<PlayerController>();
            Animator animator = gameObject.GetComponent<Animator>();

                animator.SetTrigger("Stun");
                controller.enabled = false;
               state = EDamageState.Neutral;

      
            
            
        
        }


        if(health == 0 && gameObject.tag == "Player") 
        {
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

        deathTime = Time.time + 2;
    }

    void PlayerDeath()
    {
        state = EDamageState.Death;

        PlayerController controller = gameObject.GetComponent<PlayerController>();
        Animator animator = gameObject.GetComponent<Animator>();

        if (deathCount == 0)
        {
            controller.enabled = false;
            animator.SetTrigger("Death");

            Debug.Log(gameObject.name + " is Death");

            deathCount += 1;
        }

        deathTime += 1 * Time.deltaTime;
        if (deathTime >= 5) 
        {
            animator.SetTrigger("UnDeath");
            deathCount = 0;
            deathTime = 0;
            controller.enabled = true;
            health = 100;

        }
        
    }
}