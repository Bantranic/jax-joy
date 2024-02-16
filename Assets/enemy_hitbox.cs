using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemy_hitbox : MonoBehaviour
{
    public LayerMask collisionLayers;
    public float radius;
    public float damage;


    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("HIT");
        if (other.CompareTag("Player"))
        {
            var health = other.GetComponent<EntityHealth>();

            health.health -= damage;
            health.Stun();

            //Debug.Log("WE HIT " + other.gameObject.name +  "Health is now = " + health.health);
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

