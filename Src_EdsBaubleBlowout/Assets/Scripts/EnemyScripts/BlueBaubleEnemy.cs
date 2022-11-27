using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class IdleStateBB : BaseState
{
    BlueBaubleEnemy enemySM;
    public IdleStateBB (BlueBaubleEnemy _machine) : base ("Idle", _machine)
    {
        enemySM = _machine;
    }

    public override void Enter()
    {
        base.Enter();

        enemySM.anim.SetBool("Running", false);
        enemySM.anim.SetBool("Attacking", false);
    }

    public override void UpdateLogic()
    {
        base.UpdateLogic();
        if (enemySM.PlayerCheck())
        {
            stateMachine.ChangeState(enemySM.movementState);
        }
    }
}

public class MovementStateBB : BaseState
{
    BlueBaubleEnemy enemySM;

    Vector2 positionToMoveTo;
    Vector2 refVel;

    public MovementStateBB(BlueBaubleEnemy _machine) : base("Movement", _machine)
    {
        enemySM = _machine;
    }

    public override void Enter()
    {
        base.Enter();

        Transform player = enemySM.PlayerCheck();
        positionToMoveTo = player.position - (player.position - enemySM.transform.position).normalized * enemySM.ClosestToPlayer;

        enemySM.anim.SetBool("Running", true);
    }

    public override void UpdatePhysics()
    {
        base.UpdatePhysics();

        Vector2 dir = (positionToMoveTo - (Vector2)enemySM.transform.position).normalized;
        enemySM.rb.velocity = Vector2.SmoothDamp(enemySM.rb.velocity, dir * enemySM.speed, ref refVel, enemySM.movementSmoothing);
    }

    public override void UpdateLogic()
    {
        base.UpdateLogic();

        if(Vector2.Distance(enemySM.transform.position, positionToMoveTo) < 0.2f)
        {
            stateMachine.ChangeState(enemySM.attackState);
        }
    }

    public override void Exit()
    {
        enemySM.anim.SetBool("Running", false);

        base.Exit();
    }
}

public class AttackStateBB : BaseState
{
    BlueBaubleEnemy enemySM;

    float startTime;
    
    public AttackStateBB(BlueBaubleEnemy _machine) : base("Attacking", _machine)
    {
        enemySM = _machine;
    }

    public override void Enter()
    {
        base.Enter();

        startTime = Time.time;
        enemySM.anim.SetBool("Attacking", true);

        enemySM.rb.velocity = Vector2.zero;
    }

    public override void UpdateLogic()
    {
        base.UpdateLogic();

        if(Time.time - startTime >= enemySM.attackStateDelay)
        {
            for(int i = 0; i < Random.Range(1, enemySM.maxSpawnCount); i++)
            {
                GameObject yellowCretinInst = GameObject.Instantiate(enemySM.YellowCretins);
                Vector2 point = Random.insideUnitCircle * 3;
                yellowCretinInst.transform.position = (Vector2)enemySM.transform.position + point;
            }

            stateMachine.ChangeState(enemySM.attackProcessState);
        }
    }

    public override void Exit()
    {
        enemySM.anim.SetBool("Attacking", false);

        base.Exit();
    }
}

public class AttackProcessStateBB : BaseState
{
    BlueBaubleEnemy enemySM;

    public float startTime;

    public AttackProcessStateBB(BlueBaubleEnemy _machine) : base("Attack Process", _machine)
    {
        enemySM = _machine;
    }

    public override void Enter()
    {
        base.Enter();

        startTime = Time.time;
    }

    public override void UpdateLogic()
    {
        base.UpdateLogic();

        if(Time.time - startTime >= enemySM.attackRefreshDelay)
        {
            stateMachine.ChangeState(enemySM.idleState);
        }
    }
}


public class BlueBaubleEnemy : StateMachine
{
    public float speed;
    public float movementSmoothing;

    public float PlayerScanRange;
    public float ClosestToPlayer;

    public IdleStateBB idleState;
    public MovementStateBB movementState;
    public AttackStateBB attackState;
    public AttackProcessStateBB attackProcessState;

    public float attackStateDelay;
    public float attackRefreshDelay;

    [HideInInspector]
    public Animator anim;
    [HideInInspector]
    public Rigidbody2D rb;

    public int Health;
    public GameObject DeathPrefab;

    public GameObject YellowCretins;
    public int maxSpawnCount;

    protected override BaseState GetInitialState()
    {
        return idleState;
    }

    private void Awake()
    {
        idleState = new IdleStateBB(this);
        movementState = new MovementStateBB(this);
        attackState = new AttackStateBB(this);
        attackProcessState = new AttackProcessStateBB(this);

        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }
    public Transform PlayerCheck()
    {
        Collider2D[] colls = Physics2D.OverlapCircleAll(transform.position, PlayerScanRange);

        for (int i = 0; i < colls.Length; i++)
        {
            if (colls[i].tag == "Player")
            {
                return colls[i].transform;
            }
        }

        return null;
    }

    public new void Update()
    {
        base.Update();

        if (Health <= 0)
            Die();
    }

    private void Die()
    {
        if (DeathPrefab != null)
        {
            GameObject inst = Instantiate(DeathPrefab);
            inst.transform.position = transform.position;
        }

        Destroy(gameObject);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, PlayerScanRange);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, ClosestToPlayer);
    }
}
