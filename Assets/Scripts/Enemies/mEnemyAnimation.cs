using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mEnemyAnimation : MonoBehaviour
{
    Vector2 movement;

    private Animator animator;

    Vector3 previousLocation;
    Vector3 difference;

    void Start()
    {
        animator = GetComponent<Animator>();
    }


    void Update()
    {
        difference = transform.position - previousLocation;
        previousLocation = transform.position;

        movement.x = difference.x;
        movement.y = difference.y;

        animator.SetFloat("Horizontal", movement.x);
        animator.SetFloat("Vertical", movement.y);
        animator.SetFloat("Magnitude", movement.magnitude);
    }
}
