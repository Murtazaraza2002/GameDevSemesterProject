using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController CC;
    public Animator animator;
    public Transform playerCamera;


    public float PlayerSpeed = 3.3f;
    public float PlayerSprint = 5f;
    public float TurnTime = 0.1f;
    private float turnVelocity;
    private bool isSprinting;

    //To Manage Jump/gravity
    public float gravity = -9.81f;
    public float JumpRange = 1f;
    private Vector3 Velocity;
    public Transform SurfaceCheck;
    bool onSurface;
    public float surfaceDistance = 0.4f;
    public LayerMask surfaceMask;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        onSurface = Physics.CheckSphere(SurfaceCheck.position, surfaceDistance, surfaceMask);
        if(onSurface&&Velocity.y<0)
        {
            Velocity.y = -2f;
        }
        Velocity.y += gravity * Time.deltaTime;

        if (Input.GetKey(KeyCode.LeftShift) && onSurface)
        {
            isSprinting = true;
        }
        else
        {
            isSprinting = false;
        }
        CC.Move(Velocity * Time.deltaTime);

        PlayerMove();
        Jump();
    }

    void PlayerMove()
    {
        float horizontalAxis = Input.GetAxisRaw("Horizontal");
        float VerticalAxis = Input.GetAxisRaw("Vertical");

        Vector3 direction = new Vector3(horizontalAxis, 0f, VerticalAxis).normalized;

        if(direction.magnitude>=0.1f)
        {
            animator.SetBool("RifleWalk", false);
            animator.SetBool("IdleAim", false);

            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg +playerCamera.eulerAngles.y;

            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnVelocity, TurnTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            if (isSprinting)
            {
                animator.SetBool("Idle", false);
                animator.SetBool("Walk", false);
                animator.SetBool("Running", true);
                CC.Move(moveDirection.normalized * PlayerSprint * Time.deltaTime);
            }
            else
            {
                animator.SetBool("Idle", false);
                animator.SetBool("Walk", true);
                animator.SetBool("Running", false);
                CC.Move(moveDirection.normalized * PlayerSpeed * Time.deltaTime);
            }
        }
        else
        {

            animator.SetBool("Idle", true);
            animator.SetBool("Walk", false);
            animator.SetBool("Running", false);
        }
    }

    void Jump()
    {
        if (Input.GetButtonDown("Jump") && onSurface)
        {
            animator.SetBool("Idle", false);
            animator.SetTrigger("Jump");

            Velocity.y = Mathf.Sqrt(JumpRange * -2 * gravity);
        }
        else
        {
            animator.SetBool("Idle", true);
            animator.ResetTrigger("Jump");
        }
    }

}
