using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


[RequireComponent(typeof(NavMeshAgent))]
public class AttackAble : MonoBehaviour
{
    [SerializeField] Stat stat;      // 공격자 본인의 스탯.
    Damagable target;               // 피격 대상
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
        // 화면에서 월드로 쏜 레이에 enemy 가 hit으로 리턴되었을 때,
        // 대상이 나의 AttackRange 안이라면 Attack.
        // 밖이라면 대상에게로 이동.
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
