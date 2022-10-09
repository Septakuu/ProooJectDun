using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
	public static Door Instance { get; private set; }
	Vector3 openPos;
	Vector3 closePos;
	public bool isOpen;
	[SerializeField] float moveSpeed;
	private void Awake()
	{
		Instance = this;
	}
	private void Start()
	{
		openPos = transform.position;
		Vector3 cPos = new(transform.position.x, 0, transform.position.z);
		closePos = cPos;
	}

	public void OnSwitchDoor(bool isOpen)
	{
		Debug.Log($"Door를 {isOpen} 한다!");
		StartCoroutine(SwitchDoor(isOpen));
	}
	IEnumerator SwitchDoor(bool isOpen)
	{
		while (Vector3.Distance(transform.position, isOpen ?openPos : closePos) > 0.01f)
		{
			transform.position = Vector3.MoveTowards(transform.position, isOpen ? openPos : closePos, moveSpeed * Time.deltaTime);
			yield return null;
		}
		Debug.Log("끝!");
	}
}
