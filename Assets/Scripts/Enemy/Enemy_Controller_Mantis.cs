using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy_Controller_Mantis : MonoBehaviour
{
    public GameObject LAPosition;

    public NavMeshAgent agent;
    public float speed;
    public Transform player;

    public LayerMask whatIsGround, whatIsPlayer, whatIsEnemy;

    public EntityHealth health;
    public EDamageState state;

    private bool isStunned = false; 

    private Animator animator;
    private Rigidbody rb;

    public GameObject hitbox;

    GameObject[] players;
    private GameObject targetPlayer;

    private float stunDuration;

    //Patrolling 
    private Vector3 walkPoint;
    bool walkPointSet;
    private float walkPointRange;

    //chasing
    Vector3 playerPosition;
    //public float avoidanceDistance;
    //Attacking
    public float timeBetweenAttacks;
    public bool alreadyAttacked;

    public float sightRange, AttackRange;
    public bool playerInSightRange, playerInAttackRange;

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        rb = gameObject.GetComponent<Rigidbody>();
        players = GameObject.FindGameObjectsWithTag("Player");
        agent = GetComponent<NavMeshAgent>();
        targetPlayer = players[Random.Range(0, players.Length)];
    }



    private void Patroling() 
    {

        if (!walkPointSet) SearchWalkPoint();

        if (walkPointSet) agent.SetDestination(walkPoint);

        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        //if walkpoint is reached get new point
        if(distanceToWalkPoint.magnitude < 1) 
        {
            walkPointSet = false;
        }
    
    }
    
    private void SearchWalkPoint() 
    {
        // More or less for testing purposes but if it can't see the player
        // enemy will walk randomly

        float randomz = Random.Range(-walkPointRange, walkPointRange);
        float randomx = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomx, transform.position.y, transform.position.z + randomz);

        if(Physics.Raycast(walkPoint,-transform.up, 2f, whatIsGround)) 
        {
            walkPointSet = true;
        }

    }
    private void Chasing()
    {
        agent.speed = speed;
        //If players where found with tag and there is at least one player in the level
        if (players != null && players.Length > 0)
        {

            playerPosition = targetPlayer.transform.position;
           // Debug.Log("Players in =" + playerPosition);
        }
        else
        {
            Debug.LogError("NO Game Object found with Player tag");
        }

        
        animator.SetTrigger("Run");
        agent.SetDestination(playerPosition);


        Vector3 directionToPlayer = playerPosition - transform.position;
        directionToPlayer.y = 0f; // Ensure the enemy doesn't tilt up or down

        transform.rotation = Quaternion.LookRotation(directionToPlayer);
        transform.LookAt(player);

    }
    private void Attacking() 
    {
        // Stops in movement so it does collide into player 
        // adjust the attack range range for stoping distance
        agent.SetDestination(transform.localPosition);
        agent.speed = 0;

        if (!alreadyAttacked)//If already attacked equals false
        {
            //all attack functionallity should go here

            //atacking action here
            animator.SetTrigger("Attack");
            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);

            
        }

        // Calculate direction to the player
        Vector3 directionToPlayer = playerPosition - transform.position;
        directionToPlayer.y = 0f; // Ensure the enemy doesn't tilt up or down

        transform.rotation = Quaternion.LookRotation(directionToPlayer);
        transform.LookAt(player);
     

    }
    private void ResetAttack()
    {
        //Debug.Log("RESET");
        alreadyAttacked = false;

    }

    // Called to activate the hitbox at the start of the attack animation
    public void ActivateHitbox()
    {
        hitbox.SetActive(true);
    }

    // Called to deactivate the hitbox at the end of the attack animation
    public void DeactivateHitbox()
    {
        hitbox.SetActive(false);
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
           

            Debug.Log("Touching " + collision.gameObject.name);
            if (gameObject.GetComponent<NavMeshAgent>().enabled == false)
            {
                gameObject.GetComponent<NavMeshAgent>().enabled = true;
            }

            gameObject.GetComponent<Rigidbody>().isKinematic = true;
        }
    }


    // Update is called once per frame
    void Update()
    {

        /*if (Input.GetKeyDown("t")) 
        {
            transform.position = LAPosition.transform.position;
            gameObject.GetComponent<NavMeshAgent>().enabled = false;
            isStunned = true;
            health.state = EDamageState.Stun;
 
        }
        if (Input.GetKeyDown("e"))
        {
            //transform.position = LAPosition;
            gameObject.GetComponent<NavMeshAgent>().enabled = true;
            isStunned = false;
            health.state = EDamageState.Neutral;
        }*/

        

        //Stops hitbox being active after death
        if(health.state == EDamageState.Death)
        {
            DeactivateHitbox();
            agent.speed = 0;
        
        }

        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, AttackRange, whatIsPlayer);

        //Debug.Log(state);
        //Debug.Log("Destinion = " + playerPosition);

        if(isStunned == false)
        {
            if (gameObject.GetComponent<NavMeshAgent>().enabled == true)
            {
                if (!playerInSightRange && !playerInAttackRange && health.state == EDamageState.Neutral) Patroling();//if player is out of sight range 
                else if (playerInSightRange && !playerInAttackRange && health.state == EDamageState.Neutral) Chasing();// If player is in sight range
                else if (playerInSightRange && playerInAttackRange && health.state == EDamageState.Neutral) Attacking();// If player is in attack range
                else
                {
                    agent.SetDestination(transform.localPosition);
                }
            }

        }
        else
        {
            stunDuration += 1.7f * Time.deltaTime;
            rb.drag = 5;
        }

        if (stunDuration >= 2)
        {
            isStunned = false;
            stunDuration = 0f;
            rb.drag = 0f;
        }



        //Checks if animation is playing
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Attack")) 
        {
            // Determine when to activate/deactivate the hitbox based on animation time
            float animationtime = animator.GetCurrentAnimatorStateInfo(0).normalizedTime;


            if(animationtime >= 0.1f && animationtime <= 0.8f) 
            {
               // Debug.Log("hitbox on");
                ActivateHitbox();
            
            }
            else 
            {
                //Debug.Log("Hitbox off");
                DeactivateHitbox();
            }

        
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
       // Debug.Log("HIT");
    }

    public void ApplyKnockback(Vector3 KnockbackDirection, float knockbackdistance, float duration) 
    {
        if(gameObject.GetComponent<NavMeshAgent>().enabled == true) 
        {
            gameObject.GetComponent<NavMeshAgent>().enabled = false;
        }
        
        isStunned = true;
        //stunDuration = 0f;
        gameObject.GetComponent<Rigidbody>().isKinematic = false;
        //health.state = EDamageState.Stun;
        //UnityEngine.Debug.Log("Knockback");
        // moves the enemy to knockback position by starting a corouttine
        // The Coroutine loops until the elasped time set equal the duration set in the fuction,
        // To increase the speed increase the knockback distance and to decrease the duration...decrease the duration.
        StartCoroutine(Knockbackaction(KnockbackDirection, knockbackdistance, duration));
    
    }

    private IEnumerator Knockbackaction(Vector3 knockbackDirection, float knockbackDistance, float duration) 
    {
        //Save intial postion of the the enemy
        Vector3 initialPosition = transform.position;

        //Calculate target position, so target position = current positon + the direction times by the distance
        Vector3 targetPosition = initialPosition + knockbackDirection * knockbackDistance;

        // 
        float elapsedTime = 0f;


        //loop until elapsed time equal the duration
        while(elapsedTime < duration) 
        {

            // Calculate the interpolation factor (0 to 1)
            float time = elapsedTime / duration;


            // alternate between initial and target positions
            transform.position = Vector3.Lerp(initialPosition, targetPosition, time);


            elapsedTime += Time.deltaTime;

            // until the end of the frame
            yield return null;
        }

        // Ensure the enemy reaches the exact target position
        transform.position = targetPosition;

    }

}
