using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float xmove;  // X축 누적 이동량
    public float ymove;  // Y축 누적 이동량
    public float distance = 3;
    public float zoomSpeed=2;

    Camera cam;
	private void Start()
	{
        cam = Camera.main;
        cam.transform.rotation = Quaternion.Euler(45, 0, 0);
	}
	void Update()
    {
        Zoom();
        CamRotate();
    }
    void CamRotate()
	{
        //  마우스 우클릭 중에만 카메라 무빙 적용

        if (Input.GetMouseButton(1))
        {
            xmove += Input.GetAxis("Mouse X"); // 마우스의 좌우 이동량을 xmove 에 누적합니다.
            ymove -= Input.GetAxis("Mouse Y"); // 마우스의 상하 이동량을 ymove 에 누적합니다.
            ymove = Mathf.Clamp(ymove, 0.0f, 100f);
            cam.transform.rotation = Quaternion.Euler(ymove,  xmove, 0); // 이동량에 따라 카메라의 바라보는 방향을 조정합니다.
        }
        Vector3 reverseDistance = new Vector3(0.0f, 0.0f, distance); // 카메라가 바라보는 앞방향은 Z 축입니다. 이동량에 따른 Z 축방향의 벡터를 구합니다.
        cam.transform.position = transform.position - cam.transform.rotation * reverseDistance; // 플레이어의 위치에서 카메라가 바라보는 방향에 벡터값을 적용한 상대 좌표를 차감합니다.
    }
    void Zoom()
	{
        float scroll = Input.GetAxis("Mouse ScrollWheel")* zoomSpeed;
        distance = Mathf.Clamp(distance -scroll , 3, 10);
    }

}
