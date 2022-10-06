using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stat : MonoBehaviour
{
    [System.Serializable]
    public struct Status
    {
        public int level;
        public float hp;
        public float attackPower;
        public float attackRate;
        public float attackRange;
        public float moveSpeed;
    }

    [SerializeField] string nickname;      // 이름.
    public Status basic;     // 기본 스테이터스.

    public string Name => nickname;
    public float Level => basic.level;
    public float hp { get;  set; }
    public bool IsAlive => hp > 0f;

    // 스테이터스.

}
