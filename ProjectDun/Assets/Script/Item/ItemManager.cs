using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
	public static ItemManager Instance { get; private set; }
	[SerializeField] DropUIManager dropUIManager;
	// 플레이어 인포 값을.. inven?
	[SerializeField] PlayerMovement player;
	[SerializeField] Inventory inven;
	[SerializeField] Item[] dropItems;
	[SerializeField] DropItem dropitem;
	private void Awake()
	{
		Instance = this;
	}
	// 몬스터의 Death 타이밍에 해당함수 호출
	public void DropItem(EnemyController enemy)
	{
		Debug.Log("ItemDrop 로직 실행");
		dropItems = enemy.enemyInfo.dropItem;
		int num = Random.Range(1, 100);
		if (num < 20)
		{
			// 골드 드랍? ( 안 나올 수도 있잖아.)
			// 아이템 드랍 x 
			Debug.Log("아이템 미 드랍");
		}
		else if (num >= 20)
		{
			// 골드 드랍
			// 아이템 드랍
			inven.gold+=DecideGold(enemy);

			Vector3 enemyPos = new Vector3(enemy.transform.position.x, enemy.transform.position.y+3f, enemy.transform.position.z);

			DropItem newDropItem = Instantiate(dropitem, enemyPos, enemy.transform.rotation);
			newDropItem.player = player;
			newDropItem.SetUp(DecideItem(dropItems));
			
		}
	}
	public Item DecideItem(Item[] items)
	{
		int num = Random.Range(1, 100);
		Item newItem = null;
		try
		{
			if (num <= 20)
			{
				newItem= items[0];
			}
			if (20 < num && num <= 40)
			{
				newItem= items[1];
			}
			if (40 < num && num <= 60)
			{
				newItem= items[2];
			}
			if (60 < num && num <= 80)
			{
				newItem= items[3];
			}
			if (80 < num && num <= 100)
			{
				newItem= items[4];
			}
		}
		catch
		{
			Debug.Log("인덱스범위 탈출");
			return null;
		}
		return newItem;
	}
	public int DecideGold(EnemyController enemy)
	{
		int gold = Mathf.RoundToInt((float)(enemy.enemyInfo.level * 1.5));
		Debug.Log($"골드를 획득했습니다. {gold} 골드");
		return gold;
	}
}
