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
    private float playerSpeed = 2.0f;
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

    // Sets up alternating attacks
    public bool bLeftPunch = false;
    public bool bLeftKick = true;
    float LastAttackTime = 0;


    public float knockbackdistance;

    //Combo varaibles
    private int pCount = 0;
    private int kCount = 0;
    private float baseComboCount = 0;
    private float ComboCount = 0;
    private float comboTimeSet = 2;
    private float comboTime = 0;
    public float comboDuration = 2;

    private Animator animator;

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

    public void Punch(InputAction.CallbackContext context)
    {
        /* if (context.action.triggered)
         {
            UnityEngine.Debug.Log("Punch1");
            if (bLeftPunch)
                 animator.CrossFadeInFixedTime("Punch1", 0);
             else
                 animator.CrossFadeInFixedTime("Punch2", 0);

             bLeftPunch = !bLeftPunch;

             LastAttackTime = Time.time;
         }*/

        
        if (context.action.triggered)
        {
            ComboCount += 1;
            pCount += 1;


            if (pCount == 1 && kCount == 0) 
            {
                comboTime = comboTimeSet;// reset combo time
                animator.SetTrigger("Punch1");// calls animation condition
                UnityEngine.Debug.Log("PUnch1");
                
            }
            else if(pCount == 2 && kCount == 0) ////Trigger punch 3
            {
                comboTime = comboTimeSet;
                animator.SetTrigger("Punch2");
                UnityEngine.Debug.Log("PUnch2");
                

            }
            else if(pCount == 3 && kCount == 0) //Trigger punch 3
            {
                comboTime = comboTimeSet;
                animator.SetTrigger("Punch3");
                UnityEngine.Debug.Log("PUnch2");
                

            }
            //UnityEngine.Debug.Log("Punches = " + pCount + " Kickes =" + kCount);


            LastAttackTime = Time.time;


        }

    }

    public void Kick(InputAction.CallbackContext context)
    {
        ComboCount += 1;
        kCount += 1;

        if (context.action.triggered)
        {
            if (pCount == 0 && kCount == 2)
            {
                comboTime = comboTimeSet;// reset combo time
                animator.SetTrigger("Kick1");// calls animation condition
                UnityEngine.Debug.Log("Kick1");

            }
            else if(pCount == 2 && kCount == 2) 
            {
                comboTime = comboTimeSet;// reset combo time
                animator.SetTrigger("Kick2");// calls animation condition
                UnityEngine.Debug.Log("Kick2");
            }
            

            LastAttackTime = Time.time;
        }

    }
   
    public void Charge(InputAction.CallbackContext context) 
    {
        if (context.action.triggered) 
        {
            UnityEngine.Debug.Log("Charge");
        
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


        //COUNT IF STATEMENTS ADD CONTEXT TO-DO
        if(comboTime > 0) 
        {
            comboTime -= comboDuration * Time.deltaTime;
        }
        else if (comboTime <= 0) 
        {
            pCount = 0;
            kCount = 0;
        }

        //UnityEngine.Debug.Log("COMBO TIME = " + comboTime);

        // Makes right punch first attack
        if (Time.time > LastAttackTime + 0.5f)
            bLeftPunch = false;

       

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
                        UnityEngine.Debug.Log("HIT");


                        //Apply Damage
                        Health.ApplyDamage(20, gameObject, EDamageType.StrongFist);
                        Health.Stun();

                        //Calculate kncokback
                        Vector3 knockbackDirection = (collider.transform.position - transform.position).normalized;

                        //Apply kncokback force
                        Enemy_Controller_Mantis enemy_Controller = collider.gameObject.GetComponent<Enemy_Controller_Mantis>();
                        if (enemy_Controller != null) 
                        {
                            enemy_Controller.ApplyKnockback(knockbackDirection, knockbackdistance);
                        }
                    
                    }
                    // Applies damage and stuns, unless damage has already been dealt
                   /* if (Health.lastAttackID != attackCounter)
                    {
                        Vector3 selfPosition = transform.localPosition;

                        Health.lastAttackID = attackCounter;
                        Health.ApplyDamage(20, gameObject,selfPosition, EDamageType.StrongFist);
                        Health.Stun();
                    }*/
                }
            }

            //Creates visual for hit box
            DebugExtension.DebugLocalCube(transform, size * 2, Color.red, center);
        }
    }


    void Update()
    {
        UpdateAttacks ();

        groundedPlayer = controller.isGrounded;
        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }

        // Checks if player is grounded for animator
        animator.SetBool("grounded", groundedPlayer);

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

    // When player dies, they are no longer referenced
    void OnDelete()
    {
        if (this == Player1)
            Player1 = null;
        if (this == Player2)
            Player2 = null;
    }

}