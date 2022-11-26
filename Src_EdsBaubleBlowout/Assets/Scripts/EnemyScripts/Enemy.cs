using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float speed;
    [SerializeField]
    private float smoothing;

    [SerializeField]
    private float playerCheckRange;

    private Transform player;

    private Animator anim;
    private Rigidbody2D rb;

    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        player = null;

        Collider2D[] collidersInArea = Physics2D.OverlapCircleAll(transform.position, playerCheckRange);
        for(int i = 0; i < collidersInArea.Length; i++)
        {
            if (collidersInArea[i].tag == "Player")
            {
                player = collidersInArea[i].transform;
            }
        }
    }

    void Update()
    {
        
    }
}
