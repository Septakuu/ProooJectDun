using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damagable : MonoBehaviour
{
    [SerializeField] Stat stat;     // �ǰ��� ������ ����.
    float attackedPower;
    private void Start()
    {
    }
    public void Damaged(Stat stat)
    {
        attackedPower = stat.basic.attackPower;
        this.stat.basic.hp -= attackedPower;
        Debug.Log($"{name}�� ü�� : "+ this.stat.basic.hp);
    }
}
