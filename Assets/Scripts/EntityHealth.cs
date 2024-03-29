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

    public GameObject Respawn_position;
    public List<GameObject> Mats;
    private List<Color> names;
    private Color Savedcolour;
    private bool isRed = false;

    private void Start()
    {
        currentHealth = maxHealth;

       // Mats = new List<GameObject>();
        if(playerUI != null) 
        {
            playerUI.SetMaxHealth();
        }

        if(this.gameObject.name == "Player 1")
        {
            Respawn_position = GameObject.Find("Player 2");


        }
        else if (this.gameObject.name == "Player 2")
        {
            Respawn_position = GameObject.Find("Player 1");

        }
        else 
        {
            Respawn_position = null;
        }

        foreach (Transform child in this.transform)
        {
           

           Mats.Add(child.gameObject);
              
            
            if(child.GetComponent<SkinnedMeshRenderer>() == null) 
            {

                Mats.Remove(child.gameObject);
            }
            else 
            {
                child.GetComponent<SkinnedMeshRenderer>().enabled = true;

            }

            

        }

    }

    

    public void Stun()
    {
        Debug.Log("CHECK1");
        // Checks if health is greater than 0, then sets stun time and applies to entity
        if (currentHealth <= 0 || state > EDamageState.Stun)
            return;
        
        if(state != EDamageState.Death) 
        {
            //Set the Players Health in The UI Script settings
            if (gameObject.CompareTag("Player"))
            {
                playerUI.SetHealth();
                this.GetComponent<Animator>().SetTrigger("Stun");

            }
            if (isRed == false)
            {
                
                StartCoroutine(FlashRed());
                isRed = true;
            }

            

            state = EDamageState.Stun;

            resetTime = Time.time + 1;



        }
        

    }

    // Tells player how much damage something took
    public virtual float ApplyDamage(float damage, GameObject causer, EDamageType type)
    {
        //Vector3 currentposition = transform.position;



        if (isDamageable == true)
        {

            currentHealth -= damage;
            Debug.Log("ENEMY_Health = " + currentHealth);
            isDamageable = false;

        }

        if (currentHealth <= 0 && gameObject.tag != "Player")//if player health is 0 then player death 
        {
            Death();
        }
        return damage;
    }

    void Update()
    {

        if (Input.GetKeyDown("e")) 
        {

            foreach (GameObject mat in Mats)
            {
                Debug.Log("Working");
                StartCoroutine(FlashRed());
                //mat.GetComponent<SkinnedMeshRenderer>().enabled = true;


            }

            

        }

        if (Input.GetKeyDown("t"))
        {

            foreach (GameObject mat in Mats)
            {


                //mat.GetComponent<SkinnedMeshRenderer>().enabled = false;

                mat.GetComponent<SkinnedMeshRenderer>().enabled = true;


            }



        }


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

               // animator.SetTrigger("Stun");
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
            state = EDamageState.Neutral;
            animator.SetTrigger("UnDeath");
            controller.isDead = false;

            currentHealth = maxHealth;
            playerUI.SetMaxHealth();
            deathCount = 0;
            deathTime = 0;

            if(Respawn_position != null && Respawn_position.gameObject.GetComponent<PlayerController>().isDead == false) 
            {
                this.transform.GetComponent<CharacterController>().enabled = false;
                this.transform.position = Respawn_position.transform.position + new Vector3(-1, 1, 0);
                this.transform.GetComponent<CharacterController>().enabled = true;
            }
            else 
            {
                Debug.LogError("Not RESPAWN OBJECT");
                GameObject.Find("Stage_settings").GetComponent<Level_01_stage_settings>().Retry();
            }
            

        }
        
    }


    public IEnumerator FlashRed()
    {
        //int Countdown = 3;
        float timeVisible = 0.1f;
        float timeInvisible = 0.1f;
        float blinkFor = 0.5f;


        var whenAreWeDone = Time.time + blinkFor;

        while(Time.time < whenAreWeDone) 
        {
            foreach (Transform child in this.transform)
            {
                Debug.Log("Check1");

                if (child.GetComponent<SkinnedMeshRenderer>() != null)
                {
                    Debug.Log("Check2");
                    child.GetComponent<SkinnedMeshRenderer>().enabled = false;


                }




            }
            yield return new WaitForSeconds(timeInvisible);
            foreach (Transform child in this.transform)
            {
                Debug.Log("Check1");

                if (child.GetComponent<SkinnedMeshRenderer>() != null)
                {
                    Debug.Log("Check2");
                    child.GetComponent<SkinnedMeshRenderer>().enabled = true;


                }




            }
            yield return new WaitForSeconds(timeVisible);

            isRed = false;
            Debug.Log("RED = False");

        }
        

        //yield return null;

        foreach (GameObject mat in Mats)
        {
            

            //mat.GetComponent<SkinnedMeshRenderer>().enabled = false;
           // yield return null;
            //yield return new WaitForSeconds(0.6f);
            //mat.GetComponent<SkinnedMeshRenderer>().enabled = true;
            /* Debug.Log("Check2");
                var savedC = mat.color;
                 mat.color = Color.red;
                 yield return new WaitForSeconds(0.2f);
            
             mat.color = savedC;
             isRed = false;
             Debug.Log("Check3");*/

        }





    }


    public void takesdamage(int damageAmount) 
    {
        currentHealth -= damageAmount;
        Debug.Log("HP = " + currentHealth);
        playerUI.SetHealth();


    }

    
}