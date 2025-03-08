using Unity.Netcode;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class BowOwnershipHandler : MonoBehaviour
{
    private XRGrabInteractable grabInteractable;
    private NetworkObject netObj;

    private void Awake()
    {
        grabInteractable = GetComponent<XRGrabInteractable>();
        netObj = GetComponent<NetworkObject>();
    }

    private void OnEnable()
    {
        // Subscribe to XR Interaction events
        grabInteractable.selectEntered.AddListener(OnSelectEntered);
    }

    private void OnDisable()
    {
        grabInteractable.selectEntered.RemoveListener(OnSelectEntered);
    }

    private void OnSelectEntered(SelectEnterEventArgs args)
    {
        // Only do this on the client that actually grabbed the bow
        if (!NetworkManager.Singleton.IsClient) return;

        // Attempt to change ownership via a ServerRpc
        RequestOwnershipServerRpc(NetworkManager.Singleton.LocalClientId);
    }

    [ServerRpc]
    private void RequestOwnershipServerRpc(ulong clientId)
    {
        // The server changes ownership to the grabbing client
        netObj.ChangeOwnership(clientId);
    }
}
