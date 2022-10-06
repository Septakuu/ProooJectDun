using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="Monster",menuName ="DB/Monster")]
public class Monster : ScriptableObject
{
	public GameObject prefab;			// 몬스터 프리팹
	public string monsterName;			// 몬스터 이름
	public int level;							// 몬스터 레벨
	public float hp;                          // 몬스터 현재 체력
	public float maxHp;					// 몬스터 최대 체력
	public float attackPower;             // 몬스터 공격력
	public float attackRate;               // 몬스터 공격 속도
	public float givenExp;                 // 몬슨터 경험치
	public GameObject[] dropItem;	// 드랍 아이템
}
