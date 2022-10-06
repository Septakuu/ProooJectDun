using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float xmove;  // X�� ���� �̵���
    public float ymove;  // Y�� ���� �̵���
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
        //  ���콺 ��Ŭ�� �߿��� ī�޶� ���� ����

        if (Input.GetMouseButton(1))
        {
            xmove += Input.GetAxis("Mouse X"); // ���콺�� �¿� �̵����� xmove �� �����մϴ�.
            ymove -= Input.GetAxis("Mouse Y"); // ���콺�� ���� �̵����� ymove �� �����մϴ�.
            ymove = Mathf.Clamp(ymove, 0.0f, 100f);
            cam.transform.rotation = Quaternion.Euler(ymove,  xmove, 0); // �̵����� ���� ī�޶��� �ٶ󺸴� ������ �����մϴ�.
        }
        Vector3 reverseDistance = new Vector3(0.0f, 0.0f, distance); // ī�޶� �ٶ󺸴� �չ����� Z ���Դϴ�. �̵����� ���� Z ������� ���͸� ���մϴ�.
        cam.transform.position = transform.position - cam.transform.rotation * reverseDistance; // �÷��̾��� ��ġ���� ī�޶� �ٶ󺸴� ���⿡ ���Ͱ��� ������ ��� ��ǥ�� �����մϴ�.
    }
    void Zoom()
	{
        float scroll = Input.GetAxis("Mouse ScrollWheel")* zoomSpeed;
        distance = Mathf.Clamp(distance -scroll , 3, 10);
    }

}
