using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class PullBow : XRBaseInteractable
{
    public static event Action<float> PullActionReleased;

    public Transform Start;
    public Transform End;

    public GameObject notch;

    public float pullAmount { get; private set; } = 0.0f;

    [SerializeField] private LineRenderer _lineRenderer;
    private IXRSelectInteractor pullingInteractor = null;

    protected override void Awake()
    {
        base.Awake();
    }

    public void SetPullInteractor(SelectEnterEventArgs args)
    {
        Debug.Log("SetPullInteractor");
        pullingInteractor = args.interactorObject;
    }

    public void Release()
    {
        Debug.Log("Release");
        PullActionReleased?.Invoke(pullAmount);
        pullingInteractor = null;
        pullAmount = 0.0f;
        notch.transform.localPosition = new Vector3(notch.transform.localPosition.x, notch.transform.localPosition.y, 0);
        UpdateString();
    }

    public override void ProcessInteractable(XRInteractionUpdateOrder.UpdatePhase updatePhase)
    {
        Debug.Log("ProcessInteractable");
        base.ProcessInteractable(updatePhase);
        if (updatePhase == XRInteractionUpdateOrder.UpdatePhase.Dynamic)
        {
            if(isSelected)
            {
                Vector3 pullPosition = pullingInteractor.transform.position;
                pullAmount = CalculatePull(pullPosition);

                UpdateString();
            }
        }
    }

    private float CalculatePull(Vector3 pullPosition)
    {
        Debug.Log("CalculatePull");
        Vector3 pullDirection = pullPosition - Start.position;
        Vector3 targetDirection = End.position - Start.position;
        float maxLength = targetDirection.magnitude;

        targetDirection.Normalize();
        float pullValue = Vector3.Dot(pullDirection, targetDirection) / maxLength;
        return Mathf.Clamp(pullValue, 0.0f, 1.0f);

    }

    private void UpdateString()
    {
        Debug.Log("UpdateString");
        Vector3 linePosition = Vector3.forward * Mathf.Lerp(Start.transform.localPosition.z, End.transform.localPosition.z, pullAmount);
        notch.transform.localPosition = new Vector3(notch.transform.localPosition.x, notch.transform.localPosition.y, linePosition.z + 0.2f);
        _lineRenderer.SetPosition(1, linePosition);
    }
}
