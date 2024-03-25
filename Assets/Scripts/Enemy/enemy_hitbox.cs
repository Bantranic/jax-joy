using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemy_hitbox : MonoBehaviour
{
    public LayerMask collisionLayers;
    public float radius;
    public float damage;

    private bool is_Block;

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Collision Enter "+collision.collider.name);
    }
    private void OnTriggerEnter(Collider other)
    {

        //Debug.Log("OVERLAP "+other.name);

        if (other.CompareTag("Block")) 
        {
            Debug.Log("HIT_BLOCK");
            is_Block = true;
            
        }

        //Debug.Log("HIT");
        if (other.CompareTag("Player"))
        {

            if(is_Block == false) 
            {
                var health = other.GetComponent<EntityHealth>();
                health.currentHealth -= damage;
                health.playerUI.SetHealth();
                health.Stun();

                Debug.Log("WE HIT " + other.gameObject.name + "Health is now = " + health.maxHealth);

            }
            else 
            {
                is_Block = false;
            }
            
        }
    }
    void DetectCollision() 
    {
        Collider[] hit = Physics.OverlapSphere(transform.position, radius, collisionLayers);

        if(hit.Length > 0) 
        {
            Debug.Log("WE HIT " + hit[0].gameObject.name);
        }

    }
}

