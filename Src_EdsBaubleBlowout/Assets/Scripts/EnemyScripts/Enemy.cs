using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : BaseState
{
    Enemy enemySM;
    public IdleState(Enemy stateMachine) : base("Idle", stateMachine) {
        enemySM = stateMachine;
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

public class MovementState : BaseState
{
    Enemy enemySM;

    Vector2 refVel;

    public MovementState(Enemy stateMachine) : base("Movement", stateMachine) {
        enemySM = stateMachine;
    }

    public override void Enter()
    {
        base.Enter();

        enemySM.anim.SetBool("Running", true);
    }

    public override void UpdatePhysics()
    {
        base.UpdatePhysics();

        Transform player = enemySM.PlayerCheck();
        if (player)
        {
            if(Vector2.Distance(player.position, enemySM.transform.position) <= enemySM.PlayerAttackRange)
            {
                stateMachine.ChangeState(enemySM.attackState);
            }

            Vector2 dir = (player.position - enemySM.transform.position);
            enemySM.rb.velocity = Vector2.SmoothDamp(enemySM.rb.velocity, dir * enemySM.speed, ref refVel, enemySM.movementSmoothing);
        } else
        {
            stateMachine.ChangeState(enemySM.idleState);
        }
    }

    public override void Exit()
    {
        enemySM.anim.SetBool("Running", false);

        base.Exit();
    }
}

public class AttackState : BaseState
{
    Enemy enemySM;

    float startTime;

    Vector2 playerPositionOnStart;

    public AttackState(Enemy stateMachine) : base("Attack", stateMachine) {
        enemySM = stateMachine;
    }

    public override void Enter()
    {
        base.Enter();

        enemySM.anim.SetBool("Attacking", true);
        enemySM.rb.velocity = Vector2.zero;
        
        startTime = Time.time;
        playerPositionOnStart = enemySM.PlayerCheck().position;
    }

    public override void UpdateLogic()
    {
        base.UpdateLogic();

        if(Time.time - startTime > enemySM.attackDelay)
        {
            enemySM.rb.AddForce((playerPositionOnStart - (Vector2)enemySM.transform.position).normalized * enemySM.attackForce);
            stateMachine.ChangeState(enemySM.attackProcessState);
        }
    }

    public override void Exit()
    {
        enemySM.anim.SetBool("Attacking", false);

        base.Exit();
    }
}

public class AttackProcessState : BaseState
{
    Enemy enemySM;

    float startTime;
    Vector2 refVel;

    public AttackProcessState(Enemy _machine) : base("Attack Process", _machine)
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

        enemySM.rb.velocity = Vector2.SmoothDamp(enemySM.rb.velocity, Vector2.zero, ref refVel, 0.65f);

        if(Time.time - startTime >= enemySM.afterAttackDelay)
        {
            stateMachine.ChangeState(enemySM.idleState);
        }
    }
}

public class Enemy : StateMachine
{
    public float speed;
    public float movementSmoothing;

    public float PlayerScanRange;
    public float PlayerAttackRange;

    public float attackDelay;
    public float afterAttackDelay;
    public float attackForce;
    public float attackKnockBackForce;

    public int attackDamage;

    [HideInInspector]
    public Animator anim;

    public IdleState idleState;
    public MovementState movementState;
    public AttackState attackState;
    public AttackProcessState attackProcessState;

    [HideInInspector]
    public Rigidbody2D rb;

    public int Health;
    public bool killOnDamage;
    public GameObject DeathPrefab;

    protected override BaseState GetInitialState()
    {
        return idleState;
    }

    private void Awake()
    {
        idleState = new IdleState(this);
        movementState = new MovementState(this);
        attackState= new AttackState(this);
        attackProcessState = new AttackProcessState(this);

        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    public new void Update()
    {
        base.Update();

        if (Health <= 0)
            Die();
    }

    public Transform PlayerCheck()
    {
        Collider2D[] colls = Physics2D.OverlapCircleAll(transform.position, PlayerScanRange);

        for(int i = 0; i < colls.Length; i++)
        {
            if (colls[i].tag == "Player")
            {
                return colls[i].transform;
            }
        }

        return null;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.transform.tag == "Player")
        {
            collision.transform.GetComponent<Rigidbody2D>().AddForce(
                (collision.transform.position - transform.position).normalized * attackKnockBackForce);
            collision.transform.GetComponent<PlayerMovement>().lastBoost = Time.time;
            collision.transform.GetComponent<PlayerStats>().health -= attackDamage;

            if (killOnDamage)
            {
                Die();
            }
        }
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
        Gizmos.DrawWireSphere(transform.position, PlayerAttackRange);
    }
}
