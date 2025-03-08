using UnityEngine;
using Unity.Netcode;

public class ArrowControllerNetwork : NetworkBehaviour
{
    [SerializeField] private GameObject midPointVisual, arrowPrefab, arrowSpawnPoint;
    [SerializeField] private GameObject arrowDebugger;
    [SerializeField] private float arrowMaxSpeed = 50f;

    // Called by the local owner when they prepare the bow
    public void PrepareArrow()
    {
        // Only the owner (local player who “owns” this bow) should do these actions
        if (!IsOwner) return;
               // This could be visual-only for the local user, 
        // or you could do a ClientRpc if everyone needs to see it.
        midPointVisual.SetActive(true);
    }

    // Called by the local owner when they release the bowstring
    public void ReleaseArrow(float strength)
    {
        // Again, only the owner can actually initiate firing
        if (!IsOwner) return;

        // Ask the server to spawn and launch the arrow
        ReleaseArrowServerRpc(strength);
    }

    // Runs on the server/host. Spawns the arrow as a networked object.
    [ServerRpc]
    private void ReleaseArrowServerRpc(float strength)
    {
        // Optionally set this false so it disappears on the host’s side
        // (If you want it to disappear for all players, you could call a ClientRpc)
        midPointVisual.SetActive(false);

        Debug.Log($"Bow strength is {strength}");

        // 1) Instantiate arrow on the server
        GameObject arrowInstance = Instantiate(
            arrowPrefab,
            arrowSpawnPoint.transform.position,
            arrowDebugger.transform.rotation
        );

        // 2) Make sure the arrow prefab has a NetworkObject component
        //    Then spawn it so all clients can see/sync it
        NetworkObject arrowNetObj = arrowInstance.GetComponent<NetworkObject>();
        if (arrowNetObj != null)
        {
            arrowNetObj.Spawn();
        }

        // 3) Apply force on the arrow’s Rigidbody to make it fly
        Rigidbody rb = arrowInstance.GetComponent<Rigidbody>();
        rb.AddForce(arrowDebugger.transform.forward * strength * arrowMaxSpeed, ForceMode.Impulse);
    }
}
