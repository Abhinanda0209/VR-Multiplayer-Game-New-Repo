using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class AssignInteractionManager : MonoBehaviour
{
    void Start()
    {
        XRDirectInteractor interactor = GetComponent<XRDirectInteractor>();
        if (interactor != null)
        {
            interactor.interactionManager = FindObjectOfType<XRInteractionManager>();
        }
    }
}
