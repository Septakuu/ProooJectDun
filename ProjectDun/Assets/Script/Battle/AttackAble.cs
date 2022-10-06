using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class AttackAble : MonoBehaviour
{
    [SerializeField] Stat stat;      // 공격자 본인의 스탯.
    Damagable target;               // 피격 대상

    public void Attack(Damagable target)
	{
        this.target = target;
		if (this.target == null)
			return;
		transform.LookAt(target.transform);
		Debug.Log($"{name} : {this.target.name}을 공격!");
		target.Damaged(stat);
	}
}
