using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System.Linq;
#if UNITY_EDITOR
using UnityEditor;
#endif

[RequireComponent(typeof(Damagable))]
[RequireComponent(typeof(AttackAble))]
[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Stat))]
public class EnemyController : MonoBehaviour
{
    public Monster enemyInfo;
    [SerializeField] Stat enemyStat;
    [SerializeField] ParticleSystem deadFx;
    [System.Serializable]
    private enum STATE
    {
        Stay,       // ��� : ���ڸ����� ��ٸ�.
        Patrol,     // ���� : �ֺ��� ���ƴٴѴ�.
        Chase,      // ���� : ����� ���ݹ����� �������� ���󰣴�.
        Attack,     // ���� : ����� �����Ѵ�.
        Dead,
    }

   [SerializeField] Damagable target;      // �÷��̾�(=Ÿ��)
    [SerializeField] float stayTime;        // ��� �ð�.

    [Header("Range")]
    [SerializeField] float patrolRange;     // ���� ����.
    [SerializeField] float detectionRange;  // Ž�� ����.
    [SerializeField] float attackRange;     // ���� ����.

    protected Stat status;

    private NavMeshAgent agent;
    private AttackAble attackable;
    private Animator anim;

    [SerializeField] STATE state;            // ���� ����.
    private float timer;            // ��� �ð� Ÿ�̸�.
    private float nextAttackTime;   // ���� ���� ���� �ð�.

    private Vector3 birthPoint;     // ź�� ����(=���� ��ġ)
    private Vector3 patrolPoint;    // ���� ����.

    private int groundLayerMask;    // ���� ���̾� ����ũ.
    private int playerLayerMask;    // �÷��̾� ���̾� ����ũ.

    private bool isSetPatrolPoint;  // ���� ������ �غ� �Ǿ��°�?
    private bool isInDetectRange;   // Ž�� ������ �÷��̾ ���Դ°�?
    private bool isInAttackRange;   // ���� ������ �÷��̾ ���Դ°�?

    public bool isDead => status.basic.hp <= 0;

    void Start()
    {
        state = STATE.Stay;
        timer = 0f;

        birthPoint = transform.position;

        agent = GetComponent<NavMeshAgent>();
        status = GetComponent<Stat>();
        attackable = GetComponent<AttackAble>();    // �ٰŸ� or ���Ÿ�.
        anim = GetComponent<Animator>();

        // �������ͽ��� �ִ� ���� �����Ѵ�.
        attackRange = status.basic.attackRange;
        agent.speed = status.basic.moveSpeed;

        // ��Ʈ �÷����̱� ������ ����Ʈ �������� ����Ѵ�.
        playerLayerMask = 1 << LayerMask.NameToLayer("Player");
        groundLayerMask = 1 << LayerMask.NameToLayer("Ground");
    }
    void CheckTarget()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, detectionRange);
        if (colliders.Length <= 0)
            return;

        foreach (Collider a in colliders)
        {
            if (a.gameObject.CompareTag("Player"))
            {
                target = a.GetComponent<Damagable>();
                
				if (Vector3.Distance(transform.position, target.transform.position) <= attackRange)
                    state = STATE.Attack;
                else
                    state = STATE.Chase;
            }
        }
    }
	private void Update()
	{
        CheckTarget();
        }
	void LateUpdate()
    {
		if (isDead)
		{
            state = STATE.Dead;
		}
		// Ž��, ���� ������ �÷��̾ ���Դ��� üũ.
		switch (state)
		{
			case STATE.Stay:
                OnStay();
				break;
			case STATE.Patrol:
                OnPatrol();
				break;
			case STATE.Chase:
                OnChase();
				break;
			case STATE.Attack:
                OnAttack();
                break;
            case STATE.Dead:
                OnDead();
				break;
		}

		#region �������
		/*
		// ����. (Ž�� �������� ���Դµ� ���� ������ ������ �ʾҴ�.
		if (isInDetectRange && !isInAttackRange)
        {
            OnChase();
        }
        // ����. (Ž�� �������� ���Ӱ� ���� �������� ���Դ�.)
        else if (isInDetectRange && isInAttackRange)
        {
            OnAttack();
        }
        // ����.
        else if (isSetPatrolPoint)
        {
            OnPatrol();
        }
        else
        {
            OnStay();
        }
        */
		#endregion

		anim.SetBool("isMove", state == STATE.Patrol || state == STATE.Chase);
    }
    void OnDead()
	{
        ParticleSystem newFx = Instantiate(deadFx,transform.position,transform.rotation);
        newFx.Play();
        EffectSoundManager.Instance.SkeletonDeath();
        PlayerStat.Instance.TakeEXP(enemyInfo.givenExp);
        ItemManager.Instance.DropItem(this);
		Destroy(gameObject);
	}

    private void OnStay()
    {
        CheckTarget();
        timer += Time.deltaTime;
        if (timer >= stayTime)
        {
            // stayTime��ŭ ��⸦ �Ϸ��ߴ�. Ž���� �����Ѵ�.
            timer = 0f;

            // ���� ���� ���� �ȿ��� ������ ���.
            Vector2 insideUnit = Random.insideUnitCircle;
            Vector3 point = birthPoint + (new Vector3(insideUnit.x, 0f, insideUnit.y) * patrolRange);
            point += Vector3.up * 10f;

            RaycastHit hit;
            if (Physics.Raycast(point, Vector3.down, out hit, float.MaxValue, groundLayerMask))
            {
                patrolPoint = hit.point;        // ���� ����Ʈ ����.
                isSetPatrolPoint = true;        // ���� ������ �����ߴ�.
                state = STATE.Patrol;
            }
        }
    }
    private void OnPatrol()
    {
        state = STATE.Patrol;
		if (target != null)
		{
            state = STATE.Chase;
		}
        agent.SetDestination(patrolPoint);                          // ������ ����.
        if (agent.hasPath && agent.remainingDistance <= 0.1f)       // �������� �ְ�, �����Ÿ��� 0.1�����϶�.
        {
            // �����ߴ�.
            Debug.Log("����");
            state = STATE.Stay;
            isSetPatrolPoint = false;
        }

    }
    private void OnChase()
    {
		if (Vector3.Distance(transform.position,target.transform.position)>=detectionRange)
		{
            target = null;
            state = STATE.Stay;
            return;
		}
        agent.SetDestination(target.transform.position);
        attackable.Targetting(target);
    }
    private void OnAttack()
    {
        agent.SetDestination(transform.position);
        transform.LookAt(target.transform);
        if (nextAttackTime <= Time.time)
        {
            nextAttackTime = Time.time + status.basic.attackRate;
            anim.SetTrigger("onAttack");
            BottomUI.Instance.UpdateBottomUi();
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Handles.color = Color.white;
        GUIStyle style = new GUIStyle() { fontSize = 20 };
        Handles.Label(transform.position, state.ToString());
    }

    private void OnDrawGizmosSelected()
    {
        Handles.color = Color.yellow;
        Handles.DrawWireDisc(birthPoint, Vector3.up, patrolRange);

        // ���ݰ� Ž�� ����.        
        Handles.color = Color.green;
        Handles.DrawWireDisc(transform.position, Vector3.up, detectionRange);

        Handles.color = Color.red;
        Handles.DrawWireDisc(transform.position, Vector3.up, attackRange);

        Gizmos.DrawSphere(patrolPoint, 0.1f);
    }
#endif
}
