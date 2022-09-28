using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


[RequireComponent(typeof(NavMeshAgent))]
public class AttackAble : MonoBehaviour
{
    [SerializeField] Stat stat;      // ������ ������ ����.
    Damagable target;               // �ǰ� ���
    NavMeshAgent agent; 
    Camera cam;
    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        cam = Camera.main;
    }
    private void Update()
    {
        Attack();
    }
    public void Attack()
    {
        // ȭ�鿡�� ����� �� ���̿� enemy �� hit���� ���ϵǾ��� ��,
        // ����� ���� AttackRange ���̶�� Attack.
        // ���̶�� ��󿡰Է� �̵�.
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if(Physics.Raycast(ray,out hit))
            {
                target = hit.collider.GetComponent<Damagable>();
                if (target == null)
                    return;
                

            }
        }
    }


}
