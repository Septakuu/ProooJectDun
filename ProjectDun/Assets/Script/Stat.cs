using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stat : MonoBehaviour
{
    [System.Serializable]
    public enum TYPE
    {
        Melee,         // �ٰŸ� ����
        Range,        // ���Ÿ� ����
        Spell,          // ���� ����
    }

    [SerializeField] TYPE type;
    public int level;                    // ����
    public int hp;                       // ü��
    public int maxHp;                // �ִ�ü��
    public float attackPower;      // ���ݷ�(�������ݷ�)
    public float spellPower;         // �ֹ���(�������ݷ�)
    public float critChance;         // ũ��Ƽ�� Ȯ��
    public float critMulti;            // ũ��Ƽ�� ����
    public float armor;              // ����
    public float evasion;            // ȸ����
    public float eleResistance;    // �Ӽ�����
}
