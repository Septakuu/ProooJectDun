using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


#if UNITY_EDITOR
using UnityEditor;
#endif

[RequireComponent(typeof(Stat))]
[RequireComponent(typeof(NavMeshAgent))]
public class PlayerController : MonoBehaviour
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

    RaycastHit hit;                      // 마우스 동작으로 리턴된 정보가 저장 될 변수
    Damagable target;               // 공격 시 대상.
    Stat stat;
    float playerY => transform.position.y;
    Coroutine attackCoroutine;

    [SerializeField] STATE state;
    [SerializeField] LayerMask enemyLayer;
    [SerializeField] float attackRange;
    private void Start()
    {
        state = STATE.Idle;
        cam = Camera.main;
        rigid = GetComponent<Rigidbody>();
        agent = GetComponent<NavMeshAgent>();
        stat = GetComponent<PlayerStat>();
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
            Ray point = cam.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(point, out hit, float.MaxValue))
            {
                target = hit.collider.GetComponent<Damagable>();
                if (hit.collider.gameObject.tag == "Ground")
                    state = STATE.Move;

                if(target != null)              // target 이  null이 아니고,
				{
                    state = STATE.Chase;
                    // attackRange안의 적의 collider[]을 받아와서,
                    // target의 instanceid와 대조시켜 올바른 대상을 공격
                    // 또는 target과 나 사이의 distance가 attackrange보다 작거나 같다면!

                }
            }
        }
    }
    private void Idle()
	{

	}
    private void Chase()
	{
        agent.SetDestination(target.transform.position);
        float distance = Vector3.Distance(transform.position, target.transform.position);
        bool attackable = distance < attackRange;
        if (attackable)
            state = STATE.Attack;
    }
    private void Move()
    {
        movePoint = hit.point;
        movePoint.y = transform.position.y;
        agent.SetDestination(movePoint);
        if (agent.hasPath && agent.remainingDistance <= 0.1f)
            state = STATE.Idle;
    }
    private void Attack()
	{
        agent.SetDestination(transform.position);

        if (attackCoroutine != null)
            return;

        attackCoroutine = StartCoroutine(AttackCycle());
    }
    IEnumerator AttackCycle()
	{
        WaitForSeconds wait = new WaitForSeconds(stat.attackRate);
		while (state == STATE.Attack)
		{
            Debug.Log("공격!");
            yield return wait;
		}
        StopCoroutine(attackCoroutine);
        attackCoroutine = null;
	}

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Handles.color = Color.green;
        GUIStyle style = new GUIStyle() { fontSize = 30 };
        Handles.Label(transform.position, state.ToString());
    }
#endif
}




