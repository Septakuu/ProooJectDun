using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    // 카메라의 위치 Car와 동기화

    [SerializeField] Transform thingToFollow;
    void LateUpdate()
    {
        transform.position = thingToFollow.position + new Vector3(0, 0, -10);
    }
}
