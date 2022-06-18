using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mCorridorEntranceExit : MonoBehaviour
{
    public CapsuleCollider2D capsuleCollider;

    private SpriteRenderer player;

    private void Start()
    {
        player = GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Entrance")
        {
            capsuleCollider.isTrigger = true;
            player.sortingOrder = 6;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Entrance")
        {
            capsuleCollider.isTrigger = false;
            player.sortingOrder = 6;
        }
    }
}
