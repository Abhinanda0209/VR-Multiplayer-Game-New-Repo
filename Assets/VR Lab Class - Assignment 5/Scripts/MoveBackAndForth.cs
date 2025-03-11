using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MoveBackAndForth : MonoBehaviour
{
    // Speed of movement
    public float speed = 3f;
    // How long (in seconds) to move in one direction before reversing
    public float moveDuration = 2f;

    // Internal timer to track how long we've moved in one direction
    private float timer = 0f;
    // Whether we’re currently moving right
    private bool movingRight = true;
    public TextMeshProUGUI scoreText;
    public static int Score;

    private void Start()
    {
        // Initialize if needed
        timer = 0f;
        Score = 0;
        movingRight = true;
    }



    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Shoot");
    }

    private void OnCollisionEnter(Collision collision)
    {
        Score++;
        scoreText.text = "Score > " + Score.ToString();
        Debug.Log("Shoot");
    }
    private void Update()
    {
        // Increment timer each frame
        timer += Time.deltaTime;

        // Check if we’ve moved in one direction long enough
        if (timer >= moveDuration)
        {
            // Reverse direction and reset timer
            movingRight = !movingRight;
            timer = 0f;
        }

        // Move the object according to current direction
        if (movingRight)
        {
            transform.Translate(Vector3.right * speed * Time.deltaTime);
        }
        else
        {
            transform.Translate(Vector3.left * speed * Time.deltaTime);
        }
    }


}