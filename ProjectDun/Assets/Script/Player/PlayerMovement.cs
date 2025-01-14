using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;


#if UNITY_EDITOR
using UnityEditor;
#endif

[RequireComponent(typeof(Damagable))]
[RequireComponent(typeof(AttackAble))]
[RequireComponent(typeof(Stat))]
[RequireComponent(typeof(NavMeshAgent))]
public class PlayerMovement : MonoBehaviour
{
    public enum STATE
    {
        Idle,
        Move,
        Chase,
        Attack,
        Dead,
    }
    NavMeshAgent agent;
    Vector3 movePoint;

    Camera cam;
    Rigidbody rigid;
    Animator anim;


    AttackAble attackable;
    RaycastHit hit;                      // 마우스 동작으로 리턴된 정보가 저장 될 변수
    Damagable target;               // 공격 시 대상.
    PlayerStat stat;

    int playerLayerMask;
    float playerY => transform.position.y;
    Coroutine attackCoroutine;
    [SerializeField] STATE state;
    [SerializeField] LayerMask enemyLayer;
    [SerializeField] float attackRange;
    private void Start()
    {
        playerLayerMask = (-1) - (1 << LayerMask.NameToLayer("Player"));
        state = STATE.Idle;
        cam = Camera.main;
        rigid = GetComponent<Rigidbody>();
        agent = GetComponent<NavMeshAgent>();
        stat = GetComponent<PlayerStat>();
        anim = GetComponentInChildren<Animator>();
        attackable = GetComponent<AttackAble>();
    }
    private void Update()
    {
        StateChanger();
        switch (state)
        {
            case STATE.Idle:
                Idle();
                break;
            case STATE.Move:
                Move();
                break;
            case STATE.Chase:
                Chase();
                break;
            case STATE.Attack:
                Attack();
                break;
            case STATE.Dead:
                break;
        }
    }

    private void StateChanger()
    {
        if (Input.GetMouseButtonUp(0))
        {
            if (!EventSystem.current.IsPointerOverGameObject())
            {
                Ray point = cam.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(point, out hit, float.MaxValue, playerLayerMask))
                {
                    target = hit.collider.GetComponent<Damagable>();
                    if (hit.collider.gameObject.CompareTag("Ground"))
                        state = STATE.Move;
                    else if (target != null)
                        state = STATE.Chase;
                }
            }
        }
    }

    private void Idle()
    {
        anim.SetBool("isMove", false);
        TargetUI.Instance.UpdateTargetUI(target);
    }

    private void Chase()
    {
        if (target == null)
            return;

        TargetUI.Instance.UpdateTargetUI(target);
        attackable.Targetting(target);
        agent.SetDestination(target.transform.position);
        float distance = Vector3.Distance(transform.position, target.transform.position);
        bool canAttack = distance < attackRange;
        anim.SetBool("isMove", true);
        if (canAttack)
            state = STATE.Attack;
    }

    private void Move()
    {
        movePoint = hit.point;
        movePoint.y = transform.position.y;
        agent.SetDestination(movePoint);
        anim.SetBool("isMove", true);
        if (agent.hasPath && agent.remainingDistance <= 0.1f)
            state = STATE.Idle;
        TargetUI.Instance.UpdateTargetUI(target);

    }

    private void Attack()
    {
        if (target == null)
        {
            anim.SetBool("isAttack", false);
            state = STATE.Idle;
            return;
        }
        agent.SetDestination(transform.position);
        transform.LookAt(target.transform);
        if (attackCoroutine != null)
            return;

        attackCoroutine = StartCoroutine(AttackCycle());
    }

    IEnumerator AttackCycle()
    {
        WaitForSeconds wait = new WaitForSeconds(stat.basic.attackRate);
        while (state == STATE.Attack)
        {
            anim.SetBool("isMove", false);
            anim.SetBool("isAttack", true);

            TargetUI.Instance.UpdateTargetUI(target);
            yield return wait;
        }
        StopCoroutine(attackCoroutine);
        anim.SetBool("isAttack", false);

        attackCoroutine = null;
        target = null;
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Handles.color = Color.green;
        GUIStyle style = new GUIStyle() { fontSize = 30 };
        Handles.Label(transform.position, state.ToString());
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }

#endif
}




