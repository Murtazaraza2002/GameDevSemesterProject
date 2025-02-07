using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    public TextMeshProUGUI HealthText;
    public CharacterController CC;
    public Animator animator;
    public Transform playerCamera;
    public GameObject playerDamage;
    private float damageDisplayTimer;
    private bool damaged = false;
    


    public float PlayerSpeed = 3.3f;
    public float PlayerSprint = 5f;
    public float TurnTime = 0.1f;
    private float turnVelocity;
    private bool isSprinting;

    private float playerHealth = 100f;
    public float currentHP;


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
        Cursor.lockState = CursorLockMode.Locked;
        currentHP = playerHealth;
        SetHealthText();
    }

    // Update is called once per frame
    void Update()
    {
        //return to main menu:

        if (Input.GetKey(KeyCode.Q) || Input.GetKey(KeyCode.Escape))
        {
            Debug.Log("Returning to Main Menu...");
            Invoke("DelayedAction", 2f);
        }
        if (damaged)
        {
            if(Time.time>=damageDisplayTimer)
            {
                damaged = false;
                playerDamage.SetActive(false);
            }
        }
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
    private void DelayedAction()
    {
        Cursor.lockState = CursorLockMode.None;
        SceneManager.LoadScene("MainMenu");
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

    public void PlayerHitDamage(float amount)
    {
        SetHealthText();
        playerDamage.SetActive(true);
        damaged = true;
        damageDisplayTimer = Time.time + 1f;
        currentHP -= amount;
        if(currentHP<=0)
        {
            PlayerDie();
        }
    }
    private void PlayerDie()
    {
        Cursor.lockState = CursorLockMode.None;
        Object.Destroy(gameObject,1.0f);//destroys after 1f time
    }
    void SetHealthText()
    {
        HealthText.text = "Health:" + currentHP.ToString();
    }
}
