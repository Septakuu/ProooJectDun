using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BossSpawnSystem : MonoBehaviour
{
	public static BossSpawnSystem Instance { get; private set; }
    public bool isClear;
	bool isSpawn=false;
	[SerializeField] Transform SpawnPoint;
	[SerializeField] BossController spawnBoss;
	[SerializeField] DoorManager doorManager;
	private void Awake()
	{
		Instance = this;
	}
	private void OnTriggerEnter(Collider other)
	{
		Debug.Log(other.gameObject.name);

		if (isSpawn)
			return;
		Instantiate(spawnBoss, SpawnPoint);
		isSpawn = true;
		doorManager.OnSwitchDoor(false);
	}
	public void ClearDungeon()
	{
		doorManager.OnSwitchDoor(true);
	}

}
