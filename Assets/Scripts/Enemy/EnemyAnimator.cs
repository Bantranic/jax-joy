using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimator : MonoBehaviour
{

    Animator animator;

    EntityHealth EnemyHealth;

    void Start()
    {
        animator = GetComponent<Animator>();

        EnemyHealth = GetComponent<EntityHealth>();
    }

    void Update()
    {
        if (animator != null && EnemyHealth != null)
        {
            animator.SetInteger("EntityState", (int)EnemyHealth.state);
        }
    }
}
