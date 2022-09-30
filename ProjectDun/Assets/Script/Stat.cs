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
        public float power;
        public float attackRate;
        public float defence;
        public float resistance;
        public float moveSpeed;
        public float attackRange;
    }

    [SerializeField] string nickname;      // 이름.
    public Status basic;     // 기본 스테이터스.
    public Status grow;      // 성장 스테이터스.

    public string Name => nickname;
    public float Level => basic.level;
    public float hp { get;  set; }
    public bool IsAlive => hp > 0f;

    // 스테이터스.
    public float maxHp
    {
        get
        {
            return basic.hp + (grow.hp * basic.level);
        }
    }
    public float power
    {
        get
        {
            return basic.power + (grow.power * basic.level);
        }
    }
    public float attackRate
    {
        get
        {
            return basic.attackRate + (grow.attackRate * basic.level);
        }
    }
    public float defence
    {
        get
        {
            return basic.defence + (grow.defence * basic.level);
        }
    }
    public float resistance
    {
        get
        {
            return basic.resistance + (grow.resistance * basic.level);
        }
    }
    public float moveSpeed
    {
        get
        {
            return basic.moveSpeed + (grow.moveSpeed * basic.level);
        }
    }
    public float attackRange
    {
        get
        {
            return basic.attackRange + (grow.attackRange * basic.level);
        }
    }


    private void Start()
    {
        hp = maxHp;
    }



}
