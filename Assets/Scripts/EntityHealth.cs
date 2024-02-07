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
    KnockDown,
    Death,
    Block,
    Roll,
}

public class EntityHealth : MonoBehaviour
{
    // Entity health
    public float health = 100;

    // Stun 
    public float resetTime = 0;
    
    // Sets damage state to neutral
    public EDamageState state = EDamageState.Neutral;
    
    // Checking if attack has been done
    public int lastAttackID = 0;


    // Tells player how much damage something took
    public virtual float ApplyDamage(float damage, GameObject causer, EDamageType type)
    {
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

        state = EDamageState.Stun;

        resetTime = Time.time + 1;

    }

    void Update()
    {
        if (Time.time > resetTime && state == EDamageState.Stun)
            state = EDamageState.Neutral;
    }

    public virtual void Death()
    {
        Destroy(gameObject);
    }
}