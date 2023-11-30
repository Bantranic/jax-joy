using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    public static PlayerController Player1=null;
    public static PlayerController Player2=null;

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
    private bool jumped = false;
    private bool isLpunch = false;
    private bool isRpunch = false;
    private bool isLkick = false;
    private bool isRkick = false;

    private Animator animator;

    private void Start()
    {
        controller = gameObject.GetComponent<CharacterController>();
        animator = GetComponent<Animator>();

        if (Player1 == null)
            Player1 = this;
        else
            Player2 = this;
    }

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
            animator.SetTrigger("isLkick");
        }

    }
    public void RKick(InputAction.CallbackContext context)
    {
        if (context.action.triggered)
        {
            animator.SetTrigger("isRkick");
        }

    }

    void Update()
    {
        groundedPlayer = controller.isGrounded;
        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }

        animator.SetBool("grounded", groundedPlayer);

        Vector3 move = new Vector3(movementInput.x, 0, movementInput.y);
        controller.Move(move * Time.deltaTime * playerSpeed);

        if (move != Vector3.zero)
        {
            gameObject.transform.forward = move;
        }

        // Changes the height position of the player..
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