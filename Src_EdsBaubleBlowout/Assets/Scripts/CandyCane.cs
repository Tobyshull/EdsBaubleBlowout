using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CandyCane : MonoBehaviour
{
    public float speed;
    public float spinSpeed;

    public Vector2 direction;

    Rigidbody2D rb;

    float startTime;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        transform.Rotate(new Vector3(0, 0, Random.Range(0, 360)));

        startTime = Time.time;
    }

    void Update()
    {
        rb.velocity = direction * speed;

        transform.Rotate(new Vector3(0, 0, spinSpeed));

        if(Time.time - startTime > 30)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag != "Player" && !collision.GetComponent<CandyCane>())
        {
            Enemy enemyCheck = collision.GetComponent<Enemy>();
            if (enemyCheck)
            {
                enemyCheck.Health--;
            }
            Destroy(gameObject);
        }
    }
}
