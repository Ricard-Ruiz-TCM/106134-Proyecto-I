using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mLayerChange : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private bool isAtTheBack;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        isAtTheBack = false;
    }
    
    void Update()
    {
        if (isAtTheBack == false)
        {
            spriteRenderer.sortingOrder = 4;
        }  
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "BackLayer")
        {
            spriteRenderer.sortingOrder = 1;
            isAtTheBack = true;
        }

        if (collision.tag == "FrontLayer")
        {
            spriteRenderer.sortingOrder = 4;
            isAtTheBack = false;
        }
    }
}
