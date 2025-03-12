using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    // Start is called before the first frame update


    private Animator animator;
    public bool isAttacking;
    private float timer = 0f;
    public float speed = 3f;
    public float moveDuration = 3.5f;
    public int moveDirection = 0;



    void Start()
    {
        animator = GetComponent<Animator>();
        isAttacking = false;
        timer = 0f;
        // InvokeRepeating(nameof(MyFunction), 2f, 3f);
    }

    // Update is called once per frame
    void Update()
    {

        timer += Time.deltaTime;

        if (timer >= moveDuration)
        {

            animator.SetBool("isAttacking", isAttacking);
            timer = 0f;
            //isAttacking = !isAttacking;
            moveDirection = 3 - moveDirection;

            if (moveDirection == 0) { 
                moveDirection = Random.Range(1,2);
            }
        }

        moveUpdate();
    }


    private void OnTriggerEnter(Collider other)
    {
        animator.SetBool("isDead", true);
        speed = 0;
    }



    void MyFunction()
    {
        Debug.Log("Function is called every 3 seconds!");
        isAttacking = !isAttacking;
    }


    void moveUpdate() {

        if (moveDirection == 1) {
            transform.Translate(Vector3.right * speed * Time.deltaTime);
            animator.SetInteger("isWalking",1);
        }
        if (moveDirection == 2) {
            animator.SetInteger("isWalking", 2);
            transform.Translate(Vector3.left * speed * Time.deltaTime);
        }

    }
}

/*using UnityEngine;
using Unity.Netcode;
using Unity.Netcode.Components;

[RequireComponent(typeof(NetworkObject))]
[RequireComponent(typeof(NetworkTransform))]
// (Optional) [RequireComponent(typeof(NetworkAnimator))] if you want full animator replication
public class EnemyController : NetworkBehaviour
{
    private Animator animator;

    public bool isAttacking;
    public float speed = 3f;
    public float moveDuration = 3.5f;

    private float timer = 0f;
    private int moveDirection = 0;

    private void Start()
    {
        animator = GetComponent<Animator>();
        isAttacking = false;
        timer = 0f;
    }

    private void Update()
    {
        // Only the server updates logic. Clients see the result via NetworkTransform
        if (!IsServer)
            return;

        timer += Time.deltaTime;
        if (timer >= moveDuration)
        {
            // flip or randomize direction
            moveDirection = 3 - moveDirection;
            if (moveDirection == 0)
            {
                moveDirection = Random.Range(1, 2);
            }

            // Optionally toggle isAttacking or other states
            animator.SetBool("isAttacking", isAttacking);
            timer = 0f;
        }

        MoveUpdate();
    }

    private void MoveUpdate()
    {
        // Move or animate based on your direction
        if (moveDirection == 1)
        {
            // Move right
            transform.Translate(Vector3.right * speed * Time.deltaTime);
            animator.SetInteger("isWalking", 1);
        }
        else if (moveDirection == 2)
        {
            // Move left
            transform.Translate(Vector3.left * speed * Time.deltaTime);
            animator.SetInteger("isWalking", 2);
        }
        else
        {
            // No movement
            animator.SetInteger("isWalking", 0);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Also run collision logic only on the server
        if (!IsServer)
            return;

        // Mark as dead
        animator.SetBool("isDead", true);
        speed = 0f;
    }
}*/




