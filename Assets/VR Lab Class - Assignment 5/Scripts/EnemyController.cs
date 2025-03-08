using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    // Start is called before the first frame update


    private Animator animator;
    public bool isAttacking;


    void Start()
    {
        animator = GetComponent<Animator>();
        isAttacking = true;
    }

    // Update is called once per frame
    void Update()
    {

        if (isAttacking)
        {
            animator.SetBool("isAttacking", true);
        }
        else {
            animator.SetBool("isAttacking", false);
        }
    }
}
