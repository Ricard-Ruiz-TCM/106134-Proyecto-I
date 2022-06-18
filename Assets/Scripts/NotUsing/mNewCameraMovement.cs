using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mNewCameraMovement : MonoBehaviour
{
    public Transform target;

    public float smoothing;

    private Vector3 cameraOffset = new Vector3(0, 0, -10);

    private void FixedUpdate()
    {
        Vector3 wantedPosition = target.position + cameraOffset;
        Vector3 afterSmoothPosition = Vector3.Lerp(transform.position, wantedPosition, smoothing);
        transform.position = afterSmoothPosition;
        transform.LookAt(target);
    }
}
