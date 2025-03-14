using UnityEngine;
using Unity.Netcode;
using TMPro;

public class TimerScript : NetworkBehaviour
{
    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI timerText;  // Assign in Inspector
    [SerializeField] private GameObject endMenu;         // Assign a UI panel or menu

    [Header("Timer Settings")]
    [SerializeField] public float initialTime = 90f;

    public NetworkVariable<float> timeRemaining = new NetworkVariable<float>(
        0f,
        NetworkVariableReadPermission.Everyone,
        NetworkVariableWritePermission.Server
    );

    // Whether the game has ended
    private NetworkVariable<bool> isGameOver = new NetworkVariable<bool>(
        false,
        NetworkVariableReadPermission.Everyone,
        NetworkVariableWritePermission.Server
    );

    private void Start()
    {
        // Only the server sets the starting timer 
        //if (IsServer)
        //{
            timeRemaining.Value = initialTime;
            isGameOver.Value = false;  
        //}

        // Hide end-menu on all clients initially
        if (endMenu != null)
            endMenu.SetActive(false);
    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        // When timeRemaining changes on the server, 
        // all clients automatically see the new value
        // We'll update the local timer text from any client
        Debug.Log("Inside OnNetwork");
        timeRemaining.OnValueChanged += OnTimeChanged;

        // If the game ends, show the end menu 
        isGameOver.OnValueChanged += OnGameOverChanged;

        // Initialize the timer display if a new client joined 
        OnTimeChanged(0f, timeRemaining.Value);
        OnGameOverChanged(false, isGameOver.Value);
    }

    private void Update()
    {
        // Only the Server updates the timer 
        //if (IsServer && !isGameOver.Value)
        //{
            if (timeRemaining.Value > 0f)
            {
                Debug.Log("Timer getting reduced");
                timeRemaining.Value -= Time.deltaTime;

                if (timeRemaining.Value <= 0f)
                {
                    timeRemaining.Value = 0f;
                    isGameOver.Value = true;
                }
            }
        //}
    }

    // This callback runs on any client (including host) 
    // whenever timeRemaining changes
    private void OnTimeChanged(float oldValue, float newValue)
    {
        // Convert to minutes:seconds 
        int minutes = Mathf.FloorToInt(newValue / 60f);
        int seconds = Mathf.FloorToInt(newValue % 60f);

        if (timerText != null)
        {
            timerText.text = $"{minutes:00}:{seconds:00}";
        }
    }

    // This callback runs on all clients if isGameOver changes
    private void OnGameOverChanged(bool oldValue, bool newValue)
    {
        if (newValue == true)
        {
            // Game is over: show end menu, or 
            // do other end-game logic
            ShowEndMenuClientRpc();
        }
    }

    // This runs on all clients to show the end menu 
    [ClientRpc]
    private void ShowEndMenuClientRpc()
    {
        if (endMenu != null)
        {
            endMenu.SetActive(true);
        }
        // Optionally lock out further actions
        // e.g., disable certain scripts or inputs
    }

    // If you want to restart the game from this script, e.g.:
    [ServerRpc(RequireOwnership = false)]
    public void RestartGameServerRpc()
    {
        timeRemaining.Value = initialTime;
        isGameOver.Value = false;

        // Optionally hide end menu for all 
        HideEndMenuClientRpc();
    }

    [ClientRpc]
    private void HideEndMenuClientRpc()
    {
        if (endMenu != null)
            endMenu.SetActive(false);
    }


}










/*using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;

public class TimerScript : NetworkBehaviour
{
    // Start is called before the first frame update

    [SerializeField] public TextMeshProUGUI timerText;
    [SerializeField] float remaining=90;
    void Start()
    {
      
    }

    // Update is called once per frame
    void Update()
    {

        if (remaining > 0)
        {
            remaining -= Time.deltaTime;
        }
        else if (remaining < 0) {
            remaining = 0;
        }

        remaining += Time.deltaTime;
        int minutes = Mathf.FloorToInt(remaining / 60);
        int seconds = Mathf.FloorToInt(remaining % 60);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}*/
