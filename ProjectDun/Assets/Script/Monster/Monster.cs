using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="Monster",menuName ="DB/Monster")]
public class Monster : ScriptableObject
{
	public GameObject prefab;			// ���� ������
	public string monsterName;			// ���� �̸�
	public int level;							// ���� ����
	public float hp;                          // ���� ���� ü��
	public float maxHp;					// ���� �ִ� ü��
	public float attackPower;             // ���� ���ݷ�
	public float attackRate;               // ���� ���� �ӵ�
	public float givenExp;                 // ���� ����ġ
	public GameObject[] dropItem;	// ��� ������
}
