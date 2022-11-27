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
    [HideInInspector]
    public float lastBoost;
    private float lastBoostRefresh;

    private Vector2 movementDir;
    private Vector2 lastMovementDir = new Vector2(0, -1);
    private Vector2 lastLastMovementDir = Vector2.zero;
    private bool goingLeftRight = false;

    [SerializeField]
    private GameObject[] playerParts;
    [SerializeField]
    private GameObject[] forwardPlayerParts;
    [SerializeField]
    private GameObject[] backwardPlayerParts;
    [SerializeField]
    private GameObject[] leftPlayerParts;
    [SerializeField]
    private GameObject[] rightPlayerParts;

    [SerializeField]
    private Animator UpDownAnimator;
    [SerializeField]
    private Animator LeftRightAnimator;

    [SerializeField]
    private Camera cameraObj;
    [SerializeField]
    private float shootCoolDown;
    [SerializeField]
    private GameObject candyCaneProjectile;
    private float lastShot = 0;


    bool sprint;
    bool boost;

    void ChangeAllPlayerParts(GameObject[] parts, bool activity)
    {
        for (int i = 0; i < parts.Length; i++)
        {
            parts[i].SetActive(activity);
        }
    }

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

        if (movementDir != Vector2.zero)
        {
            lastMovementDir = movementDir;
        }

        if (lastLastMovementDir != lastMovementDir)
        {
            ChangeAllPlayerParts(playerParts, false);
            if (Mathf.Abs(movementDir.x) > Mathf.Abs(movementDir.y))
            {
                if (lastMovementDir.x < 0)
                {
                    ChangeAllPlayerParts(rightPlayerParts, true);
                    goingLeftRight = true;
                }
                else if (lastMovementDir.x > 0)
                {
                    ChangeAllPlayerParts(leftPlayerParts, true);
                    goingLeftRight = true;
                }
                else
                {
                    ChangeAllPlayerParts(forwardPlayerParts, true);
                    goingLeftRight = false;
                }
            }
            else
            {
                if (lastMovementDir.y > 0)
                {
                    ChangeAllPlayerParts(backwardPlayerParts, true);
                    goingLeftRight = false;
                }
                else if (lastMovementDir.y < 0)
                {
                    ChangeAllPlayerParts(forwardPlayerParts, true);
                    goingLeftRight = false;
                }
                else
                {
                    ChangeAllPlayerParts(forwardPlayerParts, true);
                    goingLeftRight = false;
                }
            }
            lastLastMovementDir = lastMovementDir;
        }

        if(rb.velocity.magnitude > 0.1f)
        {
            if (goingLeftRight)
            {
                LeftRightAnimator.SetTrigger("Running");
            }
            else
            {
                UpDownAnimator.SetTrigger("Running");
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            Vector2 worldPosition = cameraObj.ScreenToWorldPoint(Input.mousePosition);
            Vector2 dir = (worldPosition - (Vector2)transform.position).normalized;

            GameObject inst = Instantiate(candyCaneProjectile);
            inst.transform.position = transform.position;
            inst.GetComponent<CandyCane>().direction = dir;
        }
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
