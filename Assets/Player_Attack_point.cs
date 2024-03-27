using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Attack_point : MonoBehaviour
{

    public LayerMask collisionLayer;

    public float radius = 1f;
    public float knockbackDistance;
    private float newKnockbackDistance;
    public float knockbackDuration;
    public float damageLight;
    public float damageHeavy;

    public GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(this.enabled == true) 
        {
            //Debug.Log(this.name + ("is active"));
        }


      // DetectCollision();

    }

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("Hit 1" + other.name);

        DetectCollision();
    }




    void DetectCollision() 
    {
        Collider[] hit = Physics.OverlapSphere(transform.position, radius, collisionLayer);
      
        foreach (Collider obj in hit) 
        {
            if (hit.Length > 0)
            {
                
                var Health = obj.gameObject.GetComponent<EntityHealth>();
                if (Health == null)
                    continue;


                //Apply Damage
                Debug.Log("Hit " + obj.gameObject.name);
                       
                var cAttack = player.GetComponent<PlayerController>().curAttack;
                var enemydirection = new Vector3(obj.transform.position.x, 0, obj.transform.position.z);
                var playerdirection = new Vector3(player.transform.position.x, 0, player.transform.position.z);


                Vector3 knockbackDirection = (enemydirection - playerdirection).normalized;




                if (cAttack == EAttackType.LightA1) 
                {
                    Health.ApplyDamage(damageLight, gameObject, EDamageType.StrongFist);
                    Health.Stun();
                    knockbackDirection = new Vector3(0, 1f, 0);  
                }
                else if (cAttack == EAttackType.LightA2)
                {
                    Health.ApplyDamage(damageLight, gameObject, EDamageType.StrongFist);
                    Health.Stun();
                    knockbackDirection = new Vector3(0, 0.5f, 0);
                }
                else if(cAttack == EAttackType.HeavyA1) 
                {
                    Health.ApplyDamage(damageHeavy, gameObject, EDamageType.StrongFist);
                    Health.Stun();
                    knockbackDirection = (enemydirection - playerdirection).normalized;
                }
                else if(cAttack == EAttackType.HeavyA2) 
                {
                    Health.ApplyDamage(damageHeavy, gameObject, EDamageType.StrongFist);
                    Health.Stun();
                    knockbackDirection = (enemydirection - playerdirection).normalized;
                }


                if (cAttack == EAttackType.HeavyA2)
                {
                    newKnockbackDistance = knockbackDistance * 2;
                }
                else
                {
                    newKnockbackDistance = knockbackDistance;
                }

                //Calculate kncokback


                //Apply kncokback force
                Enemy_Controller_Mantis enemy_Controller = obj.gameObject.GetComponent<Enemy_Controller_Mantis>();
                if (enemy_Controller != null)
                {
                    //calls the appltknockback function located in the Enemy_mantis_controller script
                    enemy_Controller.ApplyKnockback(knockbackDirection, newKnockbackDistance, knockbackDuration);
                }





            }

        }
       
    }
}
