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
}
