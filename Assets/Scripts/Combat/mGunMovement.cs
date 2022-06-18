using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mGunMovement : MonoBehaviour
{
    public Rigidbody2D theRigidbody;
    public Camera theCamera;
    Vector2 mousePos;

    Vector3 gunOffset = new Vector3(0.0f, 0.6f, 0.0f);

    private void Update()
    {
        transform.position = gameObject.transform.parent.position + gunOffset;
        mousePos = theCamera.ScreenToWorldPoint(Input.mousePosition);
    }

    void FixedUpdate()
    {
        Vector2 lookDir = mousePos - theRigidbody.position;
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 90f;
        theRigidbody.rotation = angle;
    }
}
