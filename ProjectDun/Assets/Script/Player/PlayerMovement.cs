using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class PlayerMovement : MonoBehaviour
{
    NavMeshAgent agent;
    Vector3 movePoint;
    Camera cam;
    Rigidbody rigid;


    private void Start()
    {
        cam = Camera.main;
        rigid = GetComponent<Rigidbody>();
        agent = GetComponent<NavMeshAgent>();
    }
    private void Update()
    {
        Move();
    }
    private void Move()
    {
        if (Input.GetMouseButtonUp(0))
        {
            Ray point = cam.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(point, out RaycastHit raycastHit, float.MaxValue))
            {
                if (raycastHit.collider.gameObject.tag == "Ground")
                {
                    movePoint = raycastHit.point;
                    movePoint.y = transform.position.y;
                    agent.SetDestination(movePoint);
                }
            }
        }
    }
}

