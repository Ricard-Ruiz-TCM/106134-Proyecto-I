using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mPoisonousArrow : MonoBehaviour
{
    public int damage;
    public float impactArea;
    public float force;

    public GameObject explosion;
    public GameObject venomTrap;

    public LayerMask hitLayer;

    private void Explode()
    {
        Collider2D[] objects = Physics2D.OverlapCircleAll(transform.position, impactArea, hitLayer);

        foreach (Collider2D obj in objects)
        {
            Vector2 direction = obj.transform.position - transform.position;

            obj.GetComponent<Rigidbody2D>().AddForce(direction * force);

            if (obj.tag == "Enemy")
            {
                obj.GetComponent<mEnemy>().hitMe(damage);
            }

            if (obj.tag == "Player")
            {
                obj.GetComponent<mPlayer>().hitMe(damage);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, impactArea);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        mEnemy enemy = collision.GetComponent<mEnemy>();
        if (enemy != null)
        {
            Explode();
        }

        if (collision.tag == "Enemy" || collision.tag == "Walls")
        {
            Explode();
            mScreenShake.instance.StartShake(0.2f, 0.1f);
            Destroy(gameObject);
            Instantiate(venomTrap, transform.position, Quaternion.identity);
            GameObject explosionEffect = Instantiate(explosion, transform.position, transform.rotation);
            Destroy(explosionEffect, 0.4f);
        }
    }
}
