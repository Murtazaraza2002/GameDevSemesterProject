using System.Runtime.CompilerServices;
using UnityEditor.Build;
using UnityEngine;

public class Zombie : MonoBehaviour
{
    public Transform player;
    private Rigidbody rb;
    public Camera AttackingRayCastArea;
    public Animator anim;

    private float ZombieHealth = 100f;
    private float ZombieHP;
    public float zombieDamage = 5f;

    public float ZombieSpeed = 3.1f; 
    public float attackRange = 1.6f; 
    public int attackDamage = 10; 
    public float attackCooldown = 1f; 
    private float lastAttackTime;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        ZombieHP = ZombieHealth;
    }

    void Update()
    {
        if(ZombieHP<=0)
        {
            return;
        }
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer > attackRange)
        {
            ChasePlayer();
        }
        // If the player is within attack range, attack them
        else
        {
            AttackPlayer();
        }
    }

    void ChasePlayer()
    {
        transform.LookAt(player);
        anim.SetBool("Walking", true);
        anim.SetBool("Attacking", false);
        anim.SetBool("Died", false);

        Vector3 moveDirection = transform.forward; // Use the zombie's forward direction after rotation
        rb.velocity = new Vector3(moveDirection.x * ZombieSpeed, rb.velocity.y, moveDirection.z * ZombieSpeed); // Preserve Y velocity (for gravity)
    }

    void AttackPlayer()
    {
        transform.LookAt(player);
        // Check if enough time has passed since the last attack
        if (Time.time - lastAttackTime >= attackCooldown)
        {
            anim.SetBool("Walking", false);
            anim.SetBool("Attacking", true);
            anim.SetBool("Died", false);
            rb.velocity = new Vector3(0, rb.velocity.y, 0);
            RaycastHit hitInfo;
            if(Physics.Raycast(AttackingRayCastArea.transform.position,AttackingRayCastArea.transform.forward,out hitInfo,attackRange))
            {
                // Perform attack logic (e.g., reduce player health)
                Debug.Log("Attacking" + hitInfo.transform.name);
                PlayerMovement playerBody = hitInfo.transform.GetComponent<PlayerMovement>();
                if(playerBody!=null)
                {
                    playerBody.PlayerHitDamage(zombieDamage);
                }
            }

            // Example: player.GetComponent<PlayerHealth>().TakeDamage(attackDamage);

            // Update the last attack time
            lastAttackTime = Time.time;
        }
    }
    public void ZombieHitDamage(float amount)
    {
        ZombieHP -= amount;
        if(ZombieHP<=0)
        {
            anim.SetBool("Walking", false);
            anim.SetBool("Attacking", false);
            anim.SetBool("Died", true);
            ZombieDie();
        }
    }
    void ZombieDie()
    {
        ZombieSpeed = 0f;
        zombieDamage = 0f;
        attackRange=0f;
        Object.Destroy(gameObject, 3f);
    }

}