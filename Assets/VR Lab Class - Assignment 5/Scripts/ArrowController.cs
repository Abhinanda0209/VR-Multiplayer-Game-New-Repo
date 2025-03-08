using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowController : MonoBehaviour
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
}
