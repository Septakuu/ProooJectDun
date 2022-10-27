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

	[SerializeField] string nickname;      // 이름.
    public Status basic;     // 기본 스테이터스.

   
    public string Name => nickname;
    public float Level => basic.level;
    public bool IsAlive => basic.maxHP > 0f;

    // 스테이터스.

}
