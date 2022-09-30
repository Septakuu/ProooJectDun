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

    RaycastHit hit;                      // ���콺 �������� ���ϵ� ������ ���� �� ����
    Damagable target;               // ���� �� ���.
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

                if(target != null)              // target ��  null�� �ƴϰ�,
				{
                    state = STATE.Chase;
                    // attackRange���� ���� collider[]�� �޾ƿͼ�,
                    // target�� instanceid�� �������� �ùٸ� ����� ����
                    // �Ǵ� target�� �� ������ distance�� attackrange���� �۰ų� ���ٸ�!

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
            Debug.Log("����!");
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




