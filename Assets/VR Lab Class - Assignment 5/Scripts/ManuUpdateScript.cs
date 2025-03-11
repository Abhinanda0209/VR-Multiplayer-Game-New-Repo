using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
public class ManuUpdateScript : MonoBehaviour
{

    public TextMeshProUGUI score;
    // Start is called before the first frame update
    int counter = 0;
    public GameObject menu;
    public bool setmenuActive;


    public TMP_Dropdown dropDownMenu;


    public void buttonPressed() {
       
        counter++;
        if (counter % 3 == 0) {
            Debug.Log("pressed " + setmenuActive);
            setmenuActive = !setmenuActive;
            menu.SetActive(setmenuActive);
            if (setmenuActive == false)
            {
                Time.timeScale = 1;
            }

            else {

                Time.timeScale = 0;
            }
            Debug.Log("After pressed " + menu.activeSelf);
        }
    }


    public void RestartGame() {
     //   SceneManager.LoadScene("VR Lab Class - Assignment 5");
    }


    public void getDropDownMenu() {
        int val = dropDownMenu.value;
        Debug.Log("drop down value "+" "+val);
    }

    
    void Start()
    {
        setmenuActive = true;
        Time.timeScale = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
