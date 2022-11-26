using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private float speed;

    [SerializeField]
    private float sprintAdditional;

    [SerializeField]
    private float movementSmoothing;

    [SerializeField]
    private float boostSpeed;

    [SerializeField]
    private float boostRefreshTime;

    [SerializeField]
    private float movementResetTime;

    [SerializeField]
    private int boostsLeft;
    [SerializeField]
    private TMPro.TMP_Text boostsLeftText;

    private Rigidbody2D rb;
    private Vector2 currentVel;

    private int maxBoosts;
    private float lastBoost;
    private float lastBoostRefresh;

    Vector2 movementDir;

    bool sprint;
    bool boost;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        maxBoosts = boostsLeft;
        lastBoostRefresh = Time.time;
    }

    private void Update()
    {
        movementDir = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        
        sprint = Input.GetKey(KeyCode.LeftShift);

        if(!boost)
            boost = Input.GetKeyDown(KeyCode.Space);
    }

    void FixedUpdate()
    {

        if (Time.time - lastBoost > movementResetTime) {
            rb.velocity = Vector2.SmoothDamp(rb.velocity, movementDir * (sprint ? speed + sprintAdditional : speed), ref currentVel, movementSmoothing);
        }

        if (boost)
        {
            if (boostsLeft > 0)
            {
                boostsLeft--;
                lastBoost = Time.time;
                lastBoostRefresh = Time.time;
                boost = false;

                rb.AddForce(movementDir * boostSpeed);
            }
        }

        if (boostsLeft < maxBoosts) {
            if (Time.time - lastBoostRefresh > boostRefreshTime)
            {
                boostsLeft++;
                lastBoostRefresh = Time.time;
            }
        }

        boostsLeftText.text = boostsLeft.ToString();
    }
}
