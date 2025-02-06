using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectDamage : MonoBehaviour
{
    public float health = 20f;
    public float modifier = 5f;



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ObjectHitDamage(float amount)
    {
        health -= amount;
        if (health <= 0f)
        {
            Die();
        }
    }
    void Die()
    {
        Destroy(gameObject);
    }

}
