using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stat : MonoBehaviour
{
    [System.Serializable]
    public struct Status
    {
        public float maxMP;
        public float mp;

        public int level;
        public float maxHP;
        public float hp;
        public float attackPower;
        public float attackRate;
        public float attackRange;
        public float moveSpeed;
        public float defense;
    }
	private void Start()
	{
        basic.hp = basic.maxHP;
        basic.mp = basic.maxMP;
	}

	[SerializeField] string nickname;      // �̸�.
    public Status basic;     // �⺻ �������ͽ�.

   
    public string Name => nickname;
    public float Level => basic.level;
    public bool IsAlive => basic.maxHP > 0f;

    // �������ͽ�.

}
