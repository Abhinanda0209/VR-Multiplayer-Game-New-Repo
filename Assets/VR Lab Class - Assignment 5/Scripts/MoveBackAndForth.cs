using UnityEngine;
using Unity.Netcode;
using TMPro;

[RequireComponent(typeof(NetworkObject))]
public class MoveBackAndForth : NetworkBehaviour
{
    // Speed of movement
    public float speed = 3f;
    // How long (in seconds) to move in one direction before reversing
    public float moveDuration = 2f;

    // Internal timer to track how long we've moved in one direction
    private float timer = 0f;
    // Whether we’re currently moving right
    private bool movingRight = true;
    [SerializeField] private TextMeshProUGUI scoreText;
    //public static int Score;
    // Networked score variable, only the server can write, everyone can read
    private NetworkVariable<int> Score = new NetworkVariable<int>(
        0,
        NetworkVariableReadPermission.Everyone,
        NetworkVariableWritePermission.Server
    );

    private void Start()
    {
        // Initialize if needed
        timer = 0f;
        //Score = 0;
        movingRight = true;
    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        // If we are a client or the host, subscribe to score changes
        if (IsClient)
        {
            Score.OnValueChanged += OnScoreChanged;
            // Set the initial text
            OnScoreChanged(0, Score.Value);
        }
    }
    private void OnScoreChanged(int oldScore, int newScore)
    {
        if (scoreText != null)
        {
            scoreText.text = $"Score > {newScore}";
        }
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

    // Only the server increments the score on collision
    private void OnCollisionEnter(Collision collision)
    {
        if (!IsServer) return;

        // For example, if an arrow or projectile hits us
        // increment the networked score
        Score.Value++;
        Debug.Log($"Target was hit, new Score={Score.Value}");
    }


}



/*using System.Collections;
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


}*/