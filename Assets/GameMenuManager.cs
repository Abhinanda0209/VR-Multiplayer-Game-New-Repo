using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class GameMenuManager : MonoBehaviour
{

    public GameObject menu;
    public float spawnDistance = 2;
    public Transform head;
    public InputActionProperty showbutton;
    public bool setmenuActive;
    // Start is called before the first frame update
    void Start()
    {
        setmenuActive = true;
       // menu.transform.position = head.position+ new Vector3(head.forward.x,0,head.forward.z).normalized*spawnDistance;
    }

    // Update is called once per frame
    void Update()
    {
        
        //if (showbutton.action.WasPressedThisFrame()) {
        //    menu.SetActive(!menu.activeSelf);
       // }
       // menu.transform.LookAt(new Vector3(head.forward.x, menu.transform.position.y, head.forward.z));
       // menu.transform.forward *= -1;
    }


    public void menuVisualizer() {
        Debug.Log("pressed "+setmenuActive);
        setmenuActive = !setmenuActive;
        menu.SetActive(setmenuActive);
        Debug.Log("After pressed " + menu.activeSelf);
    }
}
