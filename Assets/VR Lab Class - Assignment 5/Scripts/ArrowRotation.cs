using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowRotation : MonoBehaviour
{

    [SerializeField]
    private Rigidbody rb;
    private void FixedUpdate()
    {
        if (rb.velocity.sqrMagnitude > 0.01f) // Ensuring velocity isn't near-zero
        {
            transform.forward =
                Vector3.Slerp(transform.forward, rb.velocity.normalized, Time.fixedDeltaTime * 10f);
        }
    }


}
