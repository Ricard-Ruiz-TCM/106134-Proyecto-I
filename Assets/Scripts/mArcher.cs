using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mArcher : MonoBehaviour
{
    public float speed;
    public float stopDistance;
    public float backDistance;

    private float timeBetweenShots;
    public float startTimeBetweenShots;

    public GameObject bullet;

    private Transform player;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;

        timeBetweenShots = startTimeBetweenShots;
    }

    void Update()
    {
        if (Vector2.Distance(transform.position, player.position) > stopDistance)
        {
            transform.position = Vector2.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
        }
        else if (Vector2.Distance(transform.position, player.position) < stopDistance && Vector2.Distance(transform.position, player.position) > backDistance)
        {
            transform.position = this.transform.position;
        }
        else if (Vector2.Distance(transform.position, player.position) < backDistance)
        {
            transform.position = Vector2.MoveTowards(transform.position, player.position, -speed * Time.deltaTime);
        }

        if (timeBetweenShots <= 0)
        {
            Instantiate(bullet, transform.position, transform.rotation);
            timeBetweenShots = startTimeBetweenShots;
        }
        else
        {
            timeBetweenShots -= Time.deltaTime;
        }

        GetComponent<Animator>().SetFloat("Magnitude", transform.position.magnitude);
        GetComponent<Animator>().SetFloat("Horizontal", transform.position.x);
        GetComponent<Animator>().SetFloat("Vertical", transform.position.y);
    }
}
