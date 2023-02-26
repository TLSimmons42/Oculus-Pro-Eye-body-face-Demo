using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyeTrackingRay : MonoBehaviour
{

    [SerializeField]
    private float rayDistance = 1.0f;

    [SerializeField]
    private float rayWidth = .01f;

    [SerializeField]
    private LayerMask layersToInclude;

    [SerializeField]
    private Color rayColorDefaultState = Color.yellow;

    [SerializeField]
    private Color rayColorHoverState = Color.red;

    private LineRenderer LR;

    private List<EyeInteractable> eyeInteractables = new List<EyeInteractable>();

    // Start is called before the first frame update
    void Start()
    {
        LR = GetComponent<LineRenderer>();
        SetupRay();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void SetupRay()
    {
        LR.useWorldSpace = false;
        LR.positionCount = 2;
        LR.startWidth = rayWidth;
        LR.endWidth = rayWidth;
        LR.startColor = rayColorDefaultState;
        LR.endColor = rayColorDefaultState;
        LR.SetPosition(0, transform.position);
        LR.SetPosition(1, new Vector3(transform.position.x, transform.position.y,
            transform.position.z + rayDistance));
    }

    private void FixedUpdate()
    {
        RaycastHit hit;
        Vector3 rayCastDirection = transform.TransformDirection(Vector3.forward) * rayDistance;

        if (Physics.Raycast(transform.position, rayCastDirection, out hit, Mathf.Infinity, layersToInclude))
        {
            Debug.Log(hit.transform.gameObject.name);
            UnSelect();
            LR.startColor = rayColorHoverState;
            LR.endColor = rayColorHoverState;
            var eyeInteractable = hit.transform.GetComponent<EyeInteractable>();
            eyeInteractables.Add(eyeInteractable);
            eyeInteractable.IsHovered = true;
        }
        else
        {
            LR.startColor = rayColorDefaultState;
            LR.endColor = rayColorDefaultState;
            UnSelect(true);
        }
    }


    void UnSelect(bool clear = false)
    {
        foreach (var interactable in eyeInteractables)
        {
            interactable.IsHovered = false;
        }
        if (clear)
        {
            eyeInteractables.Clear();
        }
    }
}
