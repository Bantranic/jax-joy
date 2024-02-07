using System.Collections.Specialized;
using System.Diagnostics;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.InputSystem;

public enum EAttackType
{
    None,
    LPunch,
    RPunch,
    LKick,
    RKick
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

    private Animator animator;

    //Hitbox positions array (use Blender coords)
    static Vector3[] HitBoxPositions = new Vector3[] {
        (Vector3.forward * 0.89f) + (Vector3.up * 1.37f), // Left Punch
        (Vector3.forward * 0.89f) + (Vector3.up * 1.37f), // Right Punch
        (Vector3.forward * 0.89f) + (Vector3.up * 1.37f), // Left Kick
        (Vector3.forward * 0.89f) + (Vector3.up * 1.37f), // Right Kick
    };

    //Hitbox sizes array (use Blender size /2)
    static Vector3[] HitBoxSizes = new Vector3[] {
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
    public void LPunch(InputAction.CallbackContext context)
    {
        if (context.action.triggered)
        {
           animator.CrossFadeInFixedTime("Left Punch",0);
        }

    }
    public void RPunch(InputAction.CallbackContext context)
    {
        if (context.action.triggered)
        {
            animator.CrossFadeInFixedTime("Right Punch", 0);
        }

    }
    public void LKick(InputAction.CallbackContext context)
    {
        if (context.action.triggered)
        {
            animator.CrossFadeInFixedTime("Left Kick", 0);
        }
    }
    public void RKick(InputAction.CallbackContext context)
    {
        if (context.action.triggered)
        {
            animator.CrossFadeInFixedTime("Right Kick", 0);
        }

    }

    // Enables and disables attacks
    public void StartLPunch() { curAttack = EAttackType.LPunch; attackCounter++; }
    public void StartRPunch() { curAttack = EAttackType.RPunch; attackCounter++; }
    public void StartLKick() { curAttack = EAttackType.LKick; attackCounter++; }
    public void StartRKick() { curAttack = EAttackType.RKick; attackCounter++; }
    public void StopAttack() { curAttack = EAttackType.None; }

    // Hit detection for attacks
    void UpdateAttacks ()
    {
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

                    // Applies damage and stuns, unless damage has already been dealt
                    if (Health.lastAttackID != attackCounter)
                    {
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
}