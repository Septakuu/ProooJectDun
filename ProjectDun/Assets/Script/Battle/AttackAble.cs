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

	public void Targetting(Damagable target)
	{
		this.target = target;
	}
    public void Attack()
	{
		if (target == null)
			return;
        EffectSoundManager.Instance.SwingWeapon();
		Debug.Log($"{name} : {this.target.name}�� ����!");
		target.Damaged(stat);
	}
}
