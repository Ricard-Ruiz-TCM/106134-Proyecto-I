using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mCameraMovement : MonoBehaviour
{
    public Transform target;
    private Rigidbody2D targetRigidbody;
    public float positionZ = -10;
    public float foreCastFactor = 2;

    [Range(0, 1)]
    public float deathZoneSizeX = 0.5f;

    [Range(0, 1)]
    public float deathZoneSizeY = 0.5f;

    [Range(0, 1)]
    public float smoothing = 0.1f;

    Vector3 targetPosition;
    Vector3 velocity;

    Camera theCamera;

    public int minX, maxX;

    void Start()
    {
        targetRigidbody = target.GetComponent<Rigidbody2D>();
        theCamera = GetComponent<Camera>();
    }

    private void Update()
    {
        //transform.position = new Vector3(Mathf.Clamp(target.position.x, -5f, 5f), Mathf.Clamp(target.position.x, -5f, 5f), transform.position.z);
    }
    void FixedUpdate()
    {
        if (!InDeathZone()) FollowTarget();

        //Mathf.Clamp()
    }

    private bool InDeathZone()
    {
        var viewPortCoodenates = theCamera.WorldToViewportPoint(target.position);
        if (viewPortCoodenates.x < (1 - deathZoneSizeX) / 2)
            return false;
        if (viewPortCoodenates.x > 1 - ((1 - deathZoneSizeX) / 2))
            return false;

        if (viewPortCoodenates.y < (1 - deathZoneSizeY) / 2)
            return false;
        if (viewPortCoodenates.y > 1 - ((1 - deathZoneSizeY) / 2))
            return false;

        return true;
    }

    void FollowTarget()
    {
        targetPosition = target.position + (Vector3)(targetRigidbody.velocity * foreCastFactor);
        targetPosition.z = positionZ;
        Vector3 pos = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothing);
        pos.z = positionZ;
        transform.position = pos;

    }
}
