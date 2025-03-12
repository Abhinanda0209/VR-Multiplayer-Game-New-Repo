using UnityEngine;
using Unity.Netcode;
using TMPro;

public class MenuUpdateScript : NetworkBehaviour
{
    public GameObject menu;
    public bool setmenuActive;
    private TargetVisualizer targetVisualizer;

    public int counter = 0;
    public TMP_Dropdown dropDownMenu;
    public TMP_Dropdown dropDownMenuSpeed;

    private NetworkVariable<int> menuSelection = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    private NetworkVariable<int> speedSelection = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    private NetworkVariable<bool> menuActiveState = new NetworkVariable<bool>(true, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    void Start()
    {
        setmenuActive = true;
        Time.timeScale = 0;
        FindTargetVisualizer(); //  Ensures TargetVisualizer is found on startup
    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        FindTargetVisualizer(); // Ensuring it's found when spawned

        if (IsClient)
        {
            menuSelection.OnValueChanged += OnMenuSelectionChanged;
            speedSelection.OnValueChanged += OnSpeedSelectionChanged;
            menuActiveState.OnValueChanged += OnMenuActiveStateChanged;

            // Apply current values for newly joined clients
            dropDownMenu.value = menuSelection.Value;
            dropDownMenuSpeed.value = speedSelection.Value;
            menu.SetActive(menuActiveState.Value);

            ApplyMenuSelection();
            ApplySpeedSelection();
        }
    }

    private void FindTargetVisualizer()
    {
        if (targetVisualizer == null)
        {
            targetVisualizer = FindObjectOfType<TargetVisualizer>();

            if (targetVisualizer == null)
            {
                Debug.LogError("TargetVisualizer NOT FOUND in the scene!");
            }
            else
            {
                Debug.Log("TargetVisualizer FOUND!");
            }
        }
    }

    private void OnMenuSelectionChanged(int previousValue, int newValue) 
    {
        dropDownMenu.value = newValue;
        ApplyMenuSelection();
    }

    private void OnSpeedSelectionChanged(int previousValue, int newValue)
    {
        dropDownMenuSpeed.value = newValue;
        ApplySpeedSelection();
    }

    private void OnMenuActiveStateChanged(bool previousValue, bool newValue)
    {
        menu.SetActive(newValue);
        SyncTimeScaleClientRpc(newValue);
    }

    [ClientRpc]
    private void SyncTimeScaleClientRpc(bool isPaused)
    {
        Time.timeScale = isPaused ? 0 : 1;
    }

    public void buttonPressed()
    {
        if (!IsOwner) return;

        counter++;
        if (counter % 3 == 0)
        {
            setmenuActive = !setmenuActive;
            menuActiveState.Value = setmenuActive;
        }
    }

    public void GetDropDownMenu()
    {
        //Debug.Log("Inside GetDropdown menu before checking owner");
        if (!IsOwner) return;
        //Debug.Log("Inside GetDropdown menu AFTER checking owner  ");
        menuSelection.Value = dropDownMenu.value;
        RequestMenuSelectionServerRpc(menuSelection.Value);
    }

    public void GetDropDownMenuSpeed()
    {
        //Debug.Log("GetDropdownSPEED menu before checking owner");
        if (!IsOwner) return;
        //Debug.Log("GetDropdownSPEED menu AFTER checking owner");

        speedSelection.Value = dropDownMenuSpeed.value;
        RequestSpeedSelectionServerRpc(speedSelection.Value);
    }

    [ServerRpc(RequireOwnership = false)]
    private void RequestMenuSelectionServerRpc(int selection)
    {
        targetVisualizer.ActivateMenuSelection(selection);
    }

    [ServerRpc(RequireOwnership = false)]
    private void RequestSpeedSelectionServerRpc(int selection)
    {
        //Debug.Log("RequestSpeedSelectionServerRpc");
        targetVisualizer.ActivateSpeedSelection(selection);
    }

    private void ApplyMenuSelection()
    {
        if (targetVisualizer == null) return;

        targetVisualizer.ActivateMenuSelection(menuSelection.Value);
    }

    private void ApplySpeedSelection()
    {
        if (targetVisualizer == null) return;

        targetVisualizer.ActivateSpeedSelection(speedSelection.Value);
    }
}








/* Already working code with single player
 * 
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
    public GameObject targetVisualizer;

    public TMP_Dropdown dropDownMenu;
    public TMP_Dropdown dropDownMenuSpeed;


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
        TargetVisualizer tv = targetVisualizer.GetComponent<TargetVisualizer>();
        Debug.Log("drop down value "+" "+val);
        if (val == 0) {
            tv.activateBullsEye();
            tv.dctivateEnemy();
        }

        if (val == 1) {
            tv.activateEnemy();
            tv.dactivateBullsEye();
        }
    }

    public void getDropDownMenuSpeed() {
        int val = dropDownMenuSpeed.value;
        Debug.Log("drop down value speed" + " " + val);
        TargetVisualizer tv = targetVisualizer.GetComponent<TargetVisualizer>();
        if (val == 0) {
            tv.makeEnemyFrezz();
            tv.makeTargetFrezz();
        }

        if (val == 1) {
            tv.addSpeedToEnemy();
            tv.addSpeedToTarget();
        }
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
*/