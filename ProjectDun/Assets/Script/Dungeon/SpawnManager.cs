using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] EnemyController prefab;
    [SerializeField] Transform[] spawnPoints;

	public void Spawn()
	{
		foreach(Transform t in spawnPoints)
		{
			Instantiate(prefab, t);
		}
	}
}
