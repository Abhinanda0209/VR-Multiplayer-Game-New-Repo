using UnityEngine;
using Unity.Netcode;

public class ArrowController : NetworkBehaviour
{
    [SerializeField]
    private GameObject midPointVisual, arrowPrefab, arrowSpawnPoint, arrowDebugger;

    [SerializeField]
    private float arrowMaxSpeed = 50;

    private NetworkVariable<bool> arrowVisualActive = new NetworkVariable<bool>(
        false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    public override void OnNetworkSpawn()
    {
        arrowVisualActive.OnValueChanged += OnArrowVisualActiveChanged;
    }

    private void OnArrowVisualActiveChanged(bool previous, bool current)
    {
        midPointVisual.SetActive(current);
    }

    public void PrepareArrow()
    {
        if (IsOwner)
        {
            SetArrowVisualServerRpc(true);
        }
    }

    [ServerRpc]
    private void SetArrowVisualServerRpc(bool isActive)
    {
        arrowVisualActive.Value = isActive;
    }

    [ServerRpc]
    public void ReleaseArrowServerRpc(float strength)
    {
        arrowVisualActive.Value = false;

        GameObject arrow = Instantiate(arrowPrefab, arrowSpawnPoint.transform.position, arrowDebugger.transform.rotation);
        NetworkObject arrowNetObj = arrow.GetComponent<NetworkObject>();
        arrowNetObj.Spawn(true);

        Rigidbody rb = arrow.GetComponent<Rigidbody>();
        rb.AddForce(arrowDebugger.transform.forward * strength * arrowMaxSpeed, ForceMode.Impulse);
    }
}







/* Previous working version
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class ArrowController : NetworkBehaviour
{
 
    [SerializeField]
    private GameObject midPointVisual, arrowPrefab, arrowSpawnPoint;
    [SerializeField]
    private GameObject arrowDebugger;

    [SerializeField]
    private float arrowMaxSpeed = 50;

    public void PrepareArrow()
    {
        midPointVisual.SetActive(true);
    }

   // public void ReleaseArrow(float strength)
   // {
     //   midPointVisual.SetActive(false);
    //    Debug.Log($"Bow strength is {strength}");

        // Spawning arrow locally, but for networking:
        // 1) We want to do this on the Server so all clients see it.
        // 2) We'll do so via a ServerRpc call, e.g.:

      //  ReleaseArrowServerRpc(strength);
    //}

    [ServerRpc]
    private void ReleaseArrowServerRpc(float strength)
    {
        // 1) Instantiate on the server
        GameObject arrow = Instantiate(arrowPrefab, arrowSpawnPoint.transform.position, arrowDebugger.transform.rotation);

        // 2) Network spawn
        NetworkObject arrowNetObj = arrow.GetComponent<NetworkObject>();
        arrowNetObj.Spawn();

        // 3) Apply force
        Rigidbody rb = arrow.GetComponent<Rigidbody>();
        rb.AddForce(arrowDebugger.transform.forward * strength * arrowMaxSpeed, ForceMode.Impulse);
    }

    public void ReleaseArrow(float strength)
    {
        midPointVisual.SetActive(false);
        Debug.Log($"Bow strength is {strength}");
        GameObject arrow = Instantiate(arrowPrefab);
        arrow.transform.position = arrowSpawnPoint.transform.position;
        arrow.transform.rotation = arrowDebugger.transform.rotation;
        Rigidbody rb = arrow.GetComponent<Rigidbody>();
        rb.AddForce(arrowDebugger.transform.forward * strength * arrowMaxSpeed, ForceMode.Impulse);
    }
}*/
