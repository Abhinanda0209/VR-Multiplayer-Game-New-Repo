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
