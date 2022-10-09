using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : EnemyController
{

	private void OnDestroy()
	{
		BossSpawnSystem.Instance.ClearDungeon();
	}
	
}
