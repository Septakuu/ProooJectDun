using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orbuculum : MonoBehaviour
{
	[SerializeField] SpawnManager spawnManager;
	[SerializeField] OrbManager orbManager;
	[SerializeField] DoorManager doorManager;
	[SerializeField] Transform UIPivot;
	[SerializeField] float checkRadius;
	[SerializeField] LayerMask playerLayer;
	public bool isAnswer;
	bool isUse;
	public void OnInteract()
	{
		if (isUse)
		{
			// 이미 사용 한 오브젝트입니다.
			return;
		}
		if (orbManager.isClear)
		{
			// 이미 클리어 된 방입니다.
			return;
		}

		if (isAnswer)
		{
			doorManager.OnSwitchDoor(isAnswer);
			EffectSoundManager.Instance.CorrectInteract();
			isUse = true;
			orbManager.isClear = true;
		}
		else
		{
			spawnManager.Spawn();
			doorManager.OnSwitchDoor(isAnswer);

			isUse = true;
		}
	}
	void Start()
	{
		Debug.Log(isAnswer);
	}
	private bool CheckPlayer()
	{
		Collider[] checks = Physics.OverlapSphere(transform.position, checkRadius, playerLayer);
		if (checks.Length <= 0)
			return false;

		return checks[0].tag == "Player";
	}

	void Update()
	{
		bool exsist = CheckPlayer();
		if (exsist && Input.GetKeyDown(KeyCode.G))
		{
			Debug.Log("작동!");
			Debug.Log(isAnswer);
			OnInteract();
		}
	}
	private void OnDrawGizmos()
	{
		Gizmos.color = Color.green;
		Gizmos.DrawSphere(transform.position, checkRadius);

	}
}
