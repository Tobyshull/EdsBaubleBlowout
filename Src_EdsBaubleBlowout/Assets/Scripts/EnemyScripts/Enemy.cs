using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float speed;
    [SerializeField]
    private float obstacleAvoidanceForce;
    [SerializeField]
    private float smoothing;

    [SerializeField]
    private float playerCheckRange;

    [SerializeField]
    private Sprite[] sprites;
    [SerializeField]
    SpriteRenderer spriteRenderer;

    private Transform player;
    private List<Transform> obstacles = new List<Transform>();

    private Animator anim;
    private Rigidbody2D rb;

    private Vector2 currentVel;

    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        player = null;
        obstacles.Clear();

        Collider2D[] collidersInArea = Physics2D.OverlapCircleAll(transform.position, playerCheckRange);
        for(int i = 0; i < collidersInArea.Length; i++)
        {
            if (collidersInArea[i].tag == "Player")
            {
                player = collidersInArea[i].transform;
            } else if (collidersInArea[i].GetComponent<Enemy>())
            {
                obstacles.Add(collidersInArea[i].transform);
            }
        }
    }

    void Update()
    {
        if (player)
        {
            Vector2 dir = player.position - transform.position;
            dir = dir.normalized;

            rb.velocity = Vector2.SmoothDamp(rb.velocity, dir * speed, ref currentVel, smoothing);
        }

        anim.SetBool("Running", (rb.velocity.magnitude > 0.1f));

        if(Mathf.Abs(rb.velocity.x) > Mathf.Abs(rb.velocity.y))
        {
            spriteRenderer.sprite = sprites[1];
            spriteRenderer.flipX = (rb.velocity.x > 0);
        } else
        {
            spriteRenderer.flipX = false;
            if(rb.velocity.y < 0)
            {
                spriteRenderer.sprite = sprites[0];
            } else
            {
                spriteRenderer.sprite = sprites[2];
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, playerCheckRange);
    }
}
