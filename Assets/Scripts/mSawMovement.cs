using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mSawMovement : MonoBehaviour
{
    public float movingSpeed = 4f;

    public float limitRight, limitLeft;

    private bool moveRight = true;

    void FixedUpdate()
    {
        if (transform.position.x > limitRight)
        {
            moveRight = false;
        }

        if (transform.position.x < limitLeft)
        {
            moveRight = true;
        }


        if (moveRight)
        {
            transform.position = new Vector2(transform.position.x + movingSpeed + Time.deltaTime, transform.position.y);
        }
        else
        {
            transform.position = new Vector2(transform.position.x - movingSpeed + Time.deltaTime, transform.position.y);
        }
    }
}
