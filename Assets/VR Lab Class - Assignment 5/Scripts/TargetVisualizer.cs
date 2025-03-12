using UnityEngine;
using Unity.Netcode;

public class TargetVisualizer : NetworkBehaviour
{
    public GameObject[] bullseye = new GameObject[4];

    public GameObject[] enemy = new GameObject[4];

    public bool checkVisibility;

    void Start()
    {
        checkVisibility = true;
    }

    public void ActivateMenuSelection(int selection)
    {
        if (selection == 0)
        {
            RequestActivateBullsEyeServerRpc();
        }
        else if (selection == 1)
        {
            RequestActivateEnemyServerRpc();
        }
    }

    public void ActivateSpeedSelection(int selection)  
    {
        Debug.Log("Inside ActivateSpeedSelection");
        if (selection == 0)
        {
            RequestFreezeTargetsServerRpc();
        }
        else if (selection == 1)
        {
            Debug.Log("Inside ActivateSpeedSelection selection 1 - to speed up");
            RequestSpeedUpTargetsServerRpc();
        }
    }

    [ServerRpc(RequireOwnership = false)]
    public void RequestActivateBullsEyeServerRpc()
    {
        ActivateBullsEyeClientRpc();
    }

    [ServerRpc(RequireOwnership = false)]
    public void RequestActivateEnemyServerRpc()
    {
        ActivateEnemyClientRpc();
    }

    [ServerRpc(RequireOwnership = false)]
    public void RequestFreezeTargetsServerRpc()
    {
        FreezeEnemyClientRpc();
        FreezeBullsEyeClientRpc();
    }

    [ServerRpc(RequireOwnership = false)]
    public void RequestSpeedUpTargetsServerRpc()
    {
        SpeedUpEnemyClientRpc();
        SpeedUpBullsEyeClientRpc();
    }

    [ClientRpc]
    private void ActivateBullsEyeClientRpc()
    {
        for (int i = 0; i < bullseye.Length; i++)
        {
            bullseye[i].SetActive(true);
            enemy[i].SetActive(false);
        }
    }

    [ClientRpc]
    private void ActivateEnemyClientRpc()
    {
        for (int i = 0; i < enemy.Length; i++)
        {
            enemy[i].SetActive(true);
            bullseye[i].SetActive(false);
        }
    }

    [ClientRpc]
    private void FreezeEnemyClientRpc() 
    {
        for (int i = 0; i < 4; i++)
        {
            enemy[i].GetComponent<EnemyController>().speed = 0;
        }
    }
    [ClientRpc]
    private void FreezeBullsEyeClientRpc()
    {
        for (int i = 0; i < 4; i++)
        {
            bullseye[i].GetComponent<MoveBackAndForth>().speed = 0;
        }
    }

    [ClientRpc]
    private void SpeedUpEnemyClientRpc()
    {
        for (int i = 0; i < 4; i++)
        {
            enemy[i].GetComponent<EnemyController>().speed = 3;
        }
    }
    [ClientRpc]
    private void SpeedUpBullsEyeClientRpc()
    {
        for (int i = 0; i < 4; i++)
        {
            bullseye[i].GetComponent<MoveBackAndForth>().speed = 3;
        }
    }

    /*[ClientRpc]
    private void FreezeTargetsClientRpc()
    {
        for (int i = 0; i < enemy.Length; i++)
        {
            if (enemy[i].TryGetComponent(out EnemyController enemyController))
                enemyController.speed = 0;

            if (bullseye[i].TryGetComponent(out MoveBackAndForth moveController))
                moveController.speed = 0;
        }
    }

    [ClientRpc]
    private void SpeedUpTargetsClientRpc()
    {
        for (int i = 0; i < enemy.Length; i++)
        {
            if (enemy[i].TryGetComponent(out EnemyController enemyController))
                enemyController.speed = 3;

            if (bullseye[i].TryGetComponent(out MoveBackAndForth moveController))
                moveController.speed = 3;
        }
    }*/
}








/*
 * previous working code for 1 player
 * using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetVisualizer : MonoBehaviour
{
    // Start is called before the first frame update


    public GameObject[] bullseye = new GameObject[4];
    public GameObject[] enemy = new GameObject[4];

    public bool checkVisibility;

    void Start()
    {
        checkVisibility = true;
    }

    public void activateBullsEye() {
        for (int i = 0; i < 4; i++) {
            bullseye[i].SetActive(true);
        }
    }


    public void dactivateBullsEye()
    {
        for (int i = 0; i < 4; i++)
        {
            bullseye[i].SetActive(false);
        }
    }

    public void activateEnemy()
    {
        for (int i = 0; i < 4; i++)
        {
            enemy[i].SetActive(true);
        }
    }

    public void dctivateEnemy()
    {
        for (int i = 0; i < 4; i++)
        {
            enemy[i].SetActive(false);
        }
    }


    public void makeEnemyFrezz() {
        for (int i = 0; i<4 ; i++) {
            enemy[i].GetComponent<EnemyController>().speed = 0;
        }
    }

    public void addSpeedToEnemy() {

        for (int i = 0; i < 4; i++)
        {
            enemy[i].GetComponent<EnemyController>().speed = 3;
        }
    }


    public void makeTargetFrezz() {
        for (int i = 0; i < 4; i++)
        {
            bullseye[i].GetComponent<MoveBackAndForth>().speed = 0;
        }
    }


    public void addSpeedToTarget() {
        for (int i = 0; i < 4; i++)
        {
            bullseye[i].GetComponent<MoveBackAndForth>().speed = 3;
        }
    }
}*/
