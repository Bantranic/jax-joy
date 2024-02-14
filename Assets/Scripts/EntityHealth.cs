using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        if (health <= 0)
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

        //Knockback and 


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
            Destroy(gameObject);
    }

    // Death state
    public virtual void Death()
    {

        state = EDamageState.Death;

        deathTime = Time.time + 2;
    }
}