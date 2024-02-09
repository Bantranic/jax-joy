using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy_Controller_Mantis : MonoBehaviour
{
    public NavMeshAgent agent;

    public Transform player;

    public LayerMask whatIsGround, whatIsPlayer, whatIsEnemy;

    public EntityHealth health;
    public EDamageState state;


    GameObject[] players;

    //Patrolling 
    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;

    //chasing
    Vector3 playerPosition;
    public float avoidanceDistance;
    //Attacking
    public float timeBetweenAttacks;
    public bool alreadyAttacked;

    public float sightRange, AttackRange;
    public bool playerInSightRange, playerInAttackRange;

    private void Awake()
    {
        players = GameObject.FindGameObjectsWithTag("Player");
        agent = GetComponent<NavMeshAgent>();

    }



    private void Patroling() 
    {

        
        { }
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

        //If players where found with tag and there is at least one player in the level
        if (players != null && players.Length > 0)
        {

            playerPosition = players[0].transform.position;
            Debug.Log("Players in =" + playerPosition);
        }
        else
        {
            Debug.LogError("NO Game Object found with Player tag");
        }

        // direction to target eqaul target position minus current position
        Vector3 directionToPlayer = playerPosition - transform.position;


        //Raycast check to see if certain object block path to player
        RaycastHit hit;
        if(Physics.Raycast(transform.position, directionToPlayer, out hit, sightRange, whatIsEnemy))
        {
            
            
            //If an obstacle is detected, adjust the destination to avoid it
            agent.SetDestination(hit.point - directionToPlayer.normalized * avoidanceDistance);
        }
        else 
        {
            agent.SetDestination(playerPosition);
        }

        

        transform.LookAt(player);

    }
    private void Attacking() 
    {
        // Stops in movement so it does collide into player 
        // adjust the attack range range for stoping distance
        agent.SetDestination(transform.localPosition);
        

        if (!alreadyAttacked)//If already attacked equals false
        {
            //all attack functionallity should go here
            
            //atacking action here
           // Debug.Log("Attacked");

            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);

            
        }
    
    }
    private void ResetAttack()
    {
        Debug.Log("RESET");
        alreadyAttacked = false;

    }
   
    // Update is called once per frame
    void Update()
    {
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, AttackRange, whatIsPlayer);

        //Debug.Log(state);
        //Debug.Log("Destinion = " + playerPosition);
        if (!playerInSightRange && !playerInAttackRange && health.state == EDamageState.Neutral) Patroling();//if player is out of sight range 
        else if (playerInSightRange && !playerInAttackRange && health.state == EDamageState.Neutral) Chasing();// If player is in sight range
        else if (playerInSightRange && playerInAttackRange && health.state == EDamageState.Neutral) Attacking();// If player is in attack range
        else 
        {
            agent.SetDestination(transform.localPosition);
        }
    }
  
}
