using System.Collections;
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

    // Update is called once per frame
    void Update()
    {
        
        if (checkVisibility)
        {
         //   activateBullsEye();
         //   dctivateEnemy();
        }

        else {
           // activateEnemy();
           // dactivateBullsEye();
        }

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
}
