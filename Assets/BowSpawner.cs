using Unity.Netcode;
using UnityEngine;

public class BowSpawner : NetworkBehaviour
{
    [SerializeField] public GameObject bowPrefab;

    // A simple way: spawn the bow when the server starts
    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        if (IsServer)
        {
            GameObject bowInstance = Instantiate(bowPrefab, Vector3.zero, Quaternion.identity);
            bowInstance.transform.SetParent(null); // <-- THIS LINE CRITICAL FIX

            NetworkObject bowNetObj = bowInstance.GetComponent<NetworkObject>();
            if (bowNetObj != null)
                bowNetObj.Spawn(true);
            else
                Debug.LogError("Prefab missing NetworkObject!");
        }
    }
}
