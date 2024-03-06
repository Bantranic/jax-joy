using System.Collections.Specialized;
using System.Diagnostics;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.InputSystem;

public enum EAttackType
{
    None,
    Punch,
    Kick,
    LPunch,
    RPunch,
    LKick,
    RKick,
    Charge
}

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    // Keeps track of players
    public static PlayerController Player1=null;
    public static PlayerController Player2=null;

    // What moves the player
    private CharacterController controller;
    private Vector3 playerVelocity;

    private bool groundedPlayer;
    [SerializeField]
    private float defaultPlayerSpeed;
    private float playerSpeed;
    [SerializeField]
    private float jumpHeight = 1.0f;
    [SerializeField]
    private float gravityValue = -9.81f;
    private Vector2 movementInput = Vector2.zero;
 
    // BEHOLD the Unspam-inator
    private bool jumped = false;

    // What attack is currently active
    private EAttackType curAttack = EAttackType.None;

    // Amount of attacks that have been performed
    private int attackCounter = 0;


    public bool isDead = false;

    //Knockback Variables
    private float knockbackdistance;
    public float defualtKnockbackDistance;

    //Combo variables
    private int LCount = 0;
    private int HCount = 0;
    private float baseComboCount = 0;
    private float ComboCount = 0;
    private float comboTimeSet = 2;
    private float comboTime = 0;
    public float comboDuration = 2;

    //CHARGE ATTACK VARIABLES
    private float chargeCount = 0;
    private float chargeLow = 10;
    private float chargeMid = 20;
    private float chargeHigh = 40;
    private bool isCharging = false; 


    private Animator animator;
    //Blocking hitbox object
    public GameObject blockHitBox;
    // Hitbox positions array (use Blender coords)
    static Vector3[] HitBoxPositions = new Vector3[] {
        (Vector3.forward * 0.89f) + (Vector3.up * 1.37f), // Punch
        (Vector3.forward * 0.89f) + (Vector3.up * 1.37f), // Kick
        (Vector3.forward * 0.89f) + (Vector3.up * 1.37f), // Left Punch
        (Vector3.forward * 0.89f) + (Vector3.up * 1.37f), // Right Punch
        (Vector3.forward * 0.89f) + (Vector3.up * 1.37f), // Left Kick
        (Vector3.forward * 0.89f) + (Vector3.up * 1.37f), // Right Kick
    };

    // Hitbox sizes array (use Blender size /2)
    static Vector3[] HitBoxSizes = new Vector3[] {
        new Vector3 (0.21f, 0.21f, 0.37f), // Punch
        new Vector3 (0.21f, 0.21f, 0.37f), // Kick
        new Vector3 (0.21f, 0.21f, 0.37f), // Left Punch
        new Vector3 (0.21f, 0.21f, 0.37f), // Right Punch
        new Vector3 (0.21f, 0.21f, 0.37f), // Left Kick
        new Vector3 (0.21f, 0.21f, 0.37f), // Right Kick
    };


    private void Start()
    {
        controller = gameObject.GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        blockHitBox.SetActive(false);
        if (Player1 == null)
            Player1 = this;
        else
            Player2 = this;
    }

    // Animator
    public void OnMove(InputAction.CallbackContext context)
    {
        movementInput = context.ReadValue<Vector2>();
    }
    public void OnJump(InputAction.CallbackContext context)
    {
        if (!groundedPlayer)
            return;

        jumped = context.action.triggered;

        if (context.action.triggered)
        {
            animator.SetTrigger("jumped");
        }
    }

    public void LightAttack(InputAction.CallbackContext context)
    {
        knockbackdistance = defualtKnockbackDistance; //sets knockback distance for punches


        if (context.action.triggered)
        {
            ComboCount += 1;//adds to overall combo count
            LCount += 1;// holds number of punches in combo

            blockHitBox.SetActive(false);// set block to false


            //JAX ATTACKS
            if (LCount == 1 && HCount == 0) //Trigger punch 1
            {
                comboTime = comboTimeSet;// reset combo time
                animator.SetTrigger("LightA1");// calls animation condition
               
                
            }
            else if(LCount == 2 && HCount == 0 && gameObject.name == ("Player 1")) //Trigger punch 2
            {
                comboTime = comboTimeSet;// reset combo time
                animator.SetTrigger("LightA2"); // calls animation condition


            }
            else if(LCount == 3 && HCount == 0 && gameObject.name == ("Player 1")) //Trigger punch 3
            {
                comboTime = comboTimeSet;// reset combo time
                animator.SetTrigger("LightA3");// calls animation condition



            }


            //JOY LightATTACKS
            if (LCount == 1 && HCount == 0 && gameObject.name == ("Player 2"))
            {
                comboTime = comboTimeSet;// reset combo time
                animator.SetTrigger("Punch1");// calls animation condition
                UnityEngine.Debug.Log("Punch3");

            }
            else if(LCount == 2 && HCount == 0 && gameObject.name == ("Player 2")) 
            {
                comboTime = comboTimeSet;// reset combo time
                animator.SetTrigger("LightA2");// calls animation condition
                UnityEngine.Debug.Log("LightA2");

            }
            else if(LCount == 3 && HCount == 0 && gameObject.name == ("Player 2")) 
            {
                comboTime = comboTimeSet;// reset combo time
                animator.SetTrigger("LightA3");// calls animation condition
                UnityEngine.Debug.Log("LightA3");

            }
        }

    }

   

    public void HeavyAttack(InputAction.CallbackContext context)
    {
        knockbackdistance = defualtKnockbackDistance; //sets knockback distance for Kickes

        if (context.action.triggered)
        {
            ComboCount += 1;
            HCount += 1;
            blockHitBox.SetActive(false);


            //JAX HEAVYATTACKS and Joys for the moment can be seperated in the future 
            UnityEngine.Debug.Log("Kicking");
            if (LCount == 0 && HCount == 1 || LCount == 2 && HCount == 1)
            {
                comboTime = comboTimeSet;// reset combo time
                animator.SetTrigger("HeavyA1");// calls animation condition
                //UnityEngine.Debug.Log("HeavyA1");

            }

            //JOY HEAVYATTACKS Go here in future


            //UnityEngine.Debug.Log("Punches = " + pCount + " Kickes =" + kCount);

            
        }

    }

    public void Charge(InputAction.CallbackContext context)
    {
        switch (context.phase)
        {

            case InputActionPhase.Performed:

                // Trigger block action when input is performed
                isCharging = true;
                break;
            case InputActionPhase.Canceled:
                // Release block action when input is released
                // Add exit logical stufff here 

                
                UnityEngine.Debug.Log("Holding off = " + chargeCount);
                if (chargeCount >= chargeHigh)
                {
                    knockbackdistance = 40; //sets knockback distance for Kickes
                    UnityEngine.Debug.Log("CHARGE ATTACK High" + chargeCount);


                }
                else if (chargeCount >= chargeMid)
                {
                    knockbackdistance = 30; //sets knockback distance for Kickes
                    UnityEngine.Debug.Log("CHARGE ATTACK Mid" + chargeCount);

                }
              
                else if(chargeCount >= chargeLow)
                {
                    knockbackdistance = 20; //sets knockback distance for Kickes
                    UnityEngine.Debug.Log("CHARGE ATTACK Low = " + chargeCount);



                }

                //Have an effects for charging attack deactive here

                isCharging = false;//Stops charge count going up


                break;

        }
        

    }

    public void Block(InputAction.CallbackContext context) 
    {
        

        switch (context.phase)
        {

            case InputActionPhase.Performed:
               
                // Trigger block action when input is performed
              UnityEngine.Debug.Log("Switch BLOCK");
              animator.SetTrigger("Block");
               blockHitBox.SetActive(true);
                break;
           case InputActionPhase.Canceled:
                // Release block action when input is released
                // Add exit logical stufff here 
               blockHitBox.SetActive(false);
               animator.SetTrigger("UnBlock");
               UnityEngine.Debug.Log("OFF");
               break;

        }


        
        


    }

    // Enables and disables attacks
    public void StartPunch() { curAttack = EAttackType.Punch; attackCounter++; }
    public void StartKick() { curAttack = EAttackType.Kick; attackCounter++; }
    public void StartLPunch() { curAttack = EAttackType.LPunch; attackCounter++; }
    public void StartRPunch() { curAttack = EAttackType.RPunch; attackCounter++; }
    public void StartLKick() { curAttack = EAttackType.LKick; attackCounter++; }
    public void StartRKick() { curAttack = EAttackType.RKick; attackCounter++; }
    public void StopAttack() { curAttack = EAttackType.None; }

    // Hit detection for attacks
    void UpdateAttacks ()
    {
        //if combo time is greater than 0 then
        // take away comboduration from combo time
        if (comboTime > 0) 
        {
            comboTime -= comboDuration * Time.deltaTime;
        }//else if combo time is less than or equal to 0 reset combo count
        else if (comboTime <= 0)   
        {
            LCount = 0;
            HCount = 0;
            ComboCount = 0;
        }


        //statement holds charging time for charge attack
        // Longer the button is held the greater the attack damage and knockback
        if (isCharging == true)
        {
            chargeCount += 1;
            //UnityEngine.Debug.Log("IS_CHARGING" + chargeCount);
        }
        else
        {
            chargeCount = 0;
        }

        if (chargeCount >= chargeHigh)
        {
            chargeCount = chargeHigh;
        }

        // Makes right punch first attack
        //if (Time.time > LastAttackTime + 0.5f)
        //bLeftPunch = false;

        //UnityEngine.Debug.Log("PUNCH" + curAttack);

        // Hit detecion for when attacking
        if (curAttack != EAttackType.None)
        {
            
            //Creates hit box (uses pos and size from array)
            Vector3 center = HitBoxPositions[(int)curAttack - 1];
            Vector3 size = HitBoxSizes[(int)curAttack - 1];

            //Checks if there are colliders every frame
            var colliders = Physics.OverlapBox(transform.position + (transform.rotation * center), size, transform.rotation);
            if (colliders.Length > 0)
            {
                // Checks if intercepting collider has health component
                foreach (var collider in colliders)
                {
                    var Health = collider.gameObject.GetComponent<EntityHealth>();
                    if (Health == null)
                        continue;

                    //Check if the collider of the object has the enemy tags
                    // if yes, then inflict damage and knockback to said enemy
                    if (Health.CompareTag("Enemy")) 
                    {
                        


                        //Apply Damage
                        Health.ApplyDamage(20, gameObject, EDamageType.StrongFist);
                        Health.Stun();

                        //Calculate kncokback
                        Vector3 knockbackDirection = (collider.transform.position - transform.position).normalized;

                        //Apply kncokback force
                        Enemy_Controller_Mantis enemy_Controller = collider.gameObject.GetComponent<Enemy_Controller_Mantis>();
                        if (enemy_Controller != null) 
                        {
                            //calls the appltknockback function located in the Enemy_mantis_controller script
                            enemy_Controller.ApplyKnockback(knockbackDirection, knockbackdistance, 0.1f);
                        }
                    
                    }
                    // Applies damage and stuns, unless damage has already been dealt
                    if (Health.lastAttackID != attackCounter)
                    {
                        Vector3 selfPosition = transform.localPosition;

                        Health.lastAttackID = attackCounter;
                        Health.ApplyDamage(20, gameObject, EDamageType.StrongFist);
                        Health.Stun();
                    }
                }
            }

            //Creates visual for hit box
            DebugExtension.DebugLocalCube(transform, size * 2, Color.red, center);
        }
    }

    void UpdateMovement() 
    {


        groundedPlayer = controller.isGrounded;
        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }

        // Checks if player is grounded for animator
        animator.SetBool("grounded", groundedPlayer);


        if(ComboCount!= 0) 
        {
            UnityEngine.Debug.Log("SLOW");
            playerSpeed = 0.3f;
        }
        else 
        {
            UnityEngine.Debug.Log("NotSLOW");
            playerSpeed = defaultPlayerSpeed;
        }

        // Turns player speed into m/s
        Vector3 move = new Vector3(movementInput.x, 0, movementInput.y);
        controller.Move(move * playerSpeed * Time.deltaTime);

        // If we move, we move...
        if (move != Vector3.zero)
        {
            gameObject.transform.forward = move;
        }

        // Applies vertical velocity to player
        if (jumped && groundedPlayer)
        {
            playerVelocity.y = Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
            jumped = false;
        }

        playerVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);

        animator.SetFloat("MoveSpeed", (new Vector2(move.x, move.z)).magnitude);



    }

    void Update()
    {

        if(isDead != true) 
        {
            UpdateAttacks();
            UpdateMovement();

        }
      
 

    }



    // When player dies, they are no longer referenced
    void OnDelete()
    {
       /* if (this == Player1)
            Player1 = null;
        if (this == Player2)
            Player2 = null;*/
    }

}