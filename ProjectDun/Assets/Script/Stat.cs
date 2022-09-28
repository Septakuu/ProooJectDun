using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stat : MonoBehaviour
{
    [System.Serializable]
    public enum TYPE
    {
        Melee,         // 근거리 공격
        Range,        // 원거리 공격
        Spell,          // 마법 공격
    }

    [SerializeField] TYPE type;
    public int level;                    // 레벨
    public int hp;                       // 체력
    public int maxHp;                // 최대체력
    public float attackPower;      // 공격력(물리공격력)
    public float spellPower;         // 주문력(마법공격력)
    public float critChance;         // 크리티컬 확률
    public float critMulti;            // 크리티컬 배율
    public float armor;              // 방어력
    public float evasion;            // 회피율
    public float eleResistance;    // 속성저항
}
