using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class AttackAble : MonoBehaviour
{
    [SerializeField] Stat stat;      // ������ ������ ����.
    Damagable target;               // �ǰ� ���

    public void Attack(Damagable target)
	{
        this.target = target;
		if (this.target == null)
			return;
		transform.LookAt(target.transform);
		Debug.Log($"{name} : {this.target.name}�� ����!");
		target.Damaged(stat);
	}
}
