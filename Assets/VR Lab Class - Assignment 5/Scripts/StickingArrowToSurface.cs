using UnityEngine;
using Unity.Netcode;

public class StickingArrowToSurface : NetworkBehaviour
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] private SphereCollider myCollider;
    [SerializeField] private GameObject stickingArrow;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Target"))
        {
            rb.isKinematic = true;
            myCollider.isTrigger = true;

            if (IsServer)
            {
                SpawnStickingArrow(collision.transform.position, collision.transform.rotation, collision.gameObject);
                NetworkObject.Despawn(); // safely despawn the network arrow on server
            }
            else
            {
                RequestArrowStickServerRpc(collision.transform.position, collision.transform.rotation, collision.gameObject.GetComponent<NetworkObject>().NetworkObjectId);
            }
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void RequestArrowStickServerRpc(Vector3 position, Quaternion rotation, ulong targetObjectId)
    {
        NetworkObject targetNetworkObject = NetworkManager.SpawnManager.SpawnedObjects[targetObjectId];
        SpawnStickingArrow(position, rotation, targetNetworkObject.gameObject);
        NetworkObject.Despawn(true);
    }

    private void SpawnStickingArrow(Vector3 position, Quaternion rotation, GameObject target)
    {
        GameObject arrow = Instantiate(stickingArrow, position, rotation);
        arrow.transform.SetParent(target.transform, true);
        var arrowNetObj = arrow.GetComponent<NetworkObject>();
        arrowNetObj.Spawn(true);

        // Inform score manager explicitly
        ScoreManager.Instance.AddScore();
    }
}


//public static NetworkVariable<int> Score = new NetworkVariable<int>();

//public TextMeshProUGUI scoreText;


/*private void OnCollisionEnter(Collision collision)
{
    if (!IsServer || !collision.gameObject.CompareTag("Target")) return;

    rb.isKinematic = true;
    myCollider.isTrigger = true;

    GameObject arrow = Instantiate(stickingArrow, transform.position, Quaternion.LookRotation(transform.forward));
    arrow.transform.SetParent(collision.transform, true);
    arrow.GetComponent<NetworkObject>().Spawn();

    Score.Value++;
    UpdateScoreClientRpc(Score.Value);

    Destroy(gameObject);
}

[ClientRpc]
private void UpdateScoreClientRpc(int newScore)
{
    if (scoreText != null)
        scoreText.text = $"Score > {newScore}";
    else
        Debug.LogWarning("ScoreText is not assigned on client.");
}

}*/

/* previously working script without networking
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StickingArrowToSurface : MonoBehaviour
{
    [SerializeField]
    private Rigidbody rb;
    [SerializeField]
    private SphereCollider myCollider;

    [SerializeField]
    private GameObject stickingArrow;

    public TextMeshProUGUI scoreText;

    public static int Score;


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Target")
        {
            rb.isKinematic = true;
            myCollider.isTrigger = true;

            GameObject arrow = Instantiate(stickingArrow);
            arrow.transform.position = transform.position;
            arrow.transform.forward = transform.forward;
            arrow.transform.SetParent(collision.gameObject.transform);


            Score++;
            scoreText.text = "Score > " + Score.ToString();
            Debug.Log("Shoot");

            if (collision.collider.attachedRigidbody != null)
            {
                arrow.transform.parent = collision.collider.attachedRigidbody.transform;
            }

            //   collision.collider.GetComponent<IHittable>()?.GetHit(); // Todo reduce the health 

            Destroy(gameObject);
        }

    }
}*/
