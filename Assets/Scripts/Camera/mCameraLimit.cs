using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mCameraLimit : MonoBehaviour
{
    [SerializeField] GameObject player;

    [SerializeField] float timeOffset;

    [SerializeField] Vector2 posOffset;

    [SerializeField] float leftLimit;

    [SerializeField] float rightLimit;

    [SerializeField] float bottomLimit;

    [SerializeField] float topLimit;

    private Vector3 velocity;

    void LateUpdate()
    {
        if (player.gameObject != null)
        {
            Vector3 startPos = transform.position;

            Vector3 endPos = player.transform.position;

            endPos.x += posOffset.x;
            endPos.y += posOffset.y;
            endPos.z = -10;

            transform.position =  Vector3.SmoothDamp(startPos, endPos, ref velocity, timeOffset);

            transform.position = new Vector3(Mathf.Clamp(transform.position.x, leftLimit, rightLimit), Mathf.Clamp(transform.position.y, bottomLimit, topLimit), transform.position.z);
        }
        
    }
}
