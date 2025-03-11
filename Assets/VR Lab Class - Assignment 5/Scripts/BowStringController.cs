using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;
using Unity.Netcode;

public class BowStringController : NetworkBehaviour
{
    [SerializeField]
    private BowString bowStringRenderer;

    private XRGrabInteractable interactable;

    [SerializeField]
    private Transform midPointGrabObject, midPointVisualObject, midPointParent;

    private Transform interactor;
    [SerializeField]
    private float bowStringStretchLimit = 0.3f;

    private float strength;

    public UnityEvent OnBowPulled;
    public UnityEvent<float> OnBowReleased;

    private NetworkVariable<Vector3> midPointGrabLocalPosition = new NetworkVariable<Vector3>(
        Vector3.zero, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    private NetworkVariable<Vector3> midPointVisualLocalPosition = new NetworkVariable<Vector3>(
        Vector3.zero, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);


    private void Awake()
    {
        interactable = midPointGrabObject.GetComponent<XRGrabInteractable>();
    }

    public override void OnNetworkSpawn()
    {
        if (!IsOwner)
        {
            midPointVisualLocalPosition.OnValueChanged += OnMidPointVisualPositionChanged;
            midPointGrabLocalPosition.OnValueChanged += OnMidPointGrabPositionChanged;
        }
    }

    private void OnMidPointGrabPositionChanged(Vector3 previous, Vector3 current)
    {
        midPointGrabObject.localPosition = current;
    }

    private void OnMidPointVisualPositionChanged(Vector3 previous, Vector3 current)
    {
        midPointVisualObject.localPosition = current;
        bowStringRenderer.CreateString(midPointVisualObject.position);
    }

    private void Start()
    {
        if (IsOwner)
        {
            interactable.selectEntered.AddListener(PrepareBowString);
            interactable.selectExited.AddListener(ResetBowString);
        }
    }

    private void PrepareBowString(SelectEnterEventArgs arg0)
    {
        interactor = arg0.interactorObject.transform;
        OnBowPulled?.Invoke();

        GetComponent<ArrowController>()?.PrepareArrow();
    }

    private void ResetBowString(SelectExitEventArgs arg0)
    {
        ArrowController arrowController = GetComponent<ArrowController>();

        if (arrowController != null)
        {
            arrowController.ReleaseArrowServerRpc(strength);
        }
        else
        {
            Debug.LogError("ArrowController not found on the Bow!");
        }

        strength = 0;
        interactor = null;

        midPointGrabObject.localPosition = Vector3.zero;
        midPointVisualObject.localPosition = Vector3.zero;

        midPointGrabLocalPosition.Value = Vector3.zero;
        midPointVisualLocalPosition.Value = Vector3.zero;

        bowStringRenderer.CreateString(null);
    }

    private void Update()
    {
        if (IsOwner && interactor != null)
        {
            midPointGrabLocalPosition.Value = midPointGrabObject.localPosition;

            Vector3 midPointLocalSpace = midPointParent.InverseTransformPoint(midPointGrabObject.position);
            float midPointLocalZAbs = Mathf.Abs(midPointLocalSpace.x);

            if (midPointLocalSpace.x < 0 && midPointLocalZAbs < bowStringStretchLimit)
            {
                strength = Remap(midPointLocalZAbs, 0, bowStringStretchLimit, 0, 1);
                UpdateMidPointVisual(new Vector3(midPointLocalSpace.x, 0, 0));
            }
            else if (midPointLocalSpace.x < 0 && midPointLocalZAbs >= bowStringStretchLimit)
            {
                strength = 1;
                UpdateMidPointVisual(new Vector3(-bowStringStretchLimit, 0, 0));
            }
            else
            {
                strength = 0;
                UpdateMidPointVisual(Vector3.zero);
            }

            midPointGrabLocalPosition.Value = midPointGrabObject.localPosition;
            bowStringRenderer.CreateString(midPointVisualObject.position);
        }
    }

    private void UpdateMidPointVisual(Vector3 localPosition)
    {
        midPointVisualObject.localPosition = localPosition;
        midPointVisualLocalPosition.Value = localPosition;
    }

    private float Remap(float value, float fromMin, float fromMax, float toMin, float toMax)
    {
        return (value - fromMin) / (fromMax - fromMin) * (toMax - toMin) + toMin;
    }
}






/* below code is already done by Sanchi for single player, the above code is for networking
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;

public class BowStringController : MonoBehaviour
{

    [SerializeField]
    private BowString bowStringRenderer;

    private XRGrabInteractable interactable;

    [SerializeField]
    private Transform midPointGrabObject, midPointVisualObject, midPointParent;

    private Transform interactor;
    [SerializeField]
    private float bowStringStretchLimit = 0.3f;

    private float strength;

    public UnityEvent OnBowPulled;
    public UnityEvent<float> OnBowReleased;


    private void Awake()
    {
        interactable = midPointGrabObject.GetComponent<XRGrabInteractable>();
    }

    private void Start()
    {
        interactable.selectEntered.AddListener(PrepareBowString);
        interactable.selectExited.AddListener(ResetBowString);
    }

    private void ResetBowString(SelectExitEventArgs arg0)
    {
        OnBowReleased?.Invoke(strength);
        strength = 0;

        interactor = null;
        midPointGrabObject.localPosition = Vector3.zero;
        midPointVisualObject.localPosition = Vector3.zero;
        bowStringRenderer.CreateString(null);
    }

    private void PrepareBowString(SelectEnterEventArgs arg0)
    {
        interactor = arg0.interactorObject.transform;
        OnBowPulled?.Invoke();
    }

    private void Update()
    {
        if (interactor != null)
        {

            //convert bow string mid point position to the local space of the MidPoint
            Vector3 midPointLocalSpace =
                midPointParent.InverseTransformPoint(midPointGrabObject.position); // localPosition

            //get the offset
            float midPointLocalZAbs = Mathf.Abs(midPointLocalSpace.x);

            HandleStringPushedBackToStart(midPointLocalSpace);

            HandleStringPulledBackTolimit(midPointLocalZAbs, midPointLocalSpace);

            HandlePullingString(midPointLocalZAbs, midPointLocalSpace);

            bowStringRenderer.CreateString(midPointVisualObject.position);

            // bowStringRenderer.CreateString(midPointGrabObject.transform.position);
        }
    }

    private void HandlePullingString(float midPointLocalZAbs, Vector3 midPointLocalSpace)
    {
        if (midPointLocalSpace.x < 0 && midPointLocalZAbs < bowStringStretchLimit)
        {
            strength = Remap(midPointLocalZAbs, 0, bowStringStretchLimit, 0, 1);
            midPointVisualObject.localPosition = new Vector3(midPointLocalSpace.x, 0, 0);
        }
    }

    private float Remap(float value, int fromMin, float fromMax, int toMin, int toMax)
    {
        return (value - fromMin) / (fromMax - fromMin) * (toMax - toMin) + toMin;
    }

    private void HandleStringPulledBackTolimit(float midPointLocalZAbs, Vector3 midPointLocalSpace)
    {
        if (midPointLocalSpace.x < 0 && midPointLocalZAbs >= bowStringStretchLimit)
        {
            strength = 1;
            //Vector3 direction = midPointParent.TransformDirection(new Vector3(0, 0, midPointLocalSpace.z));
            midPointVisualObject.localPosition = new Vector3(-bowStringStretchLimit,0,0);
        }
    }

    private void HandleStringPushedBackToStart(Vector3 midPointLocalSpace)
    {
        strength = 0;
        midPointVisualObject.localPosition = Vector3.zero;
    }
}*/
