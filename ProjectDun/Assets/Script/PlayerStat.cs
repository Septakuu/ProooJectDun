using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStat : Stat
{
    public static PlayerStat Instance { get; private set; }
    [SerializeField] ParticleSystem levelUpFx;
    public float exp;
    public float maxExp;
    public int str;
    public int dex;
    public int wis;         // ���� int
	private void Awake()
	{
        Instance = this;
	}
	public Status grow;

    // ���� ����,  ������Ƽ ���� set ���� ������Ƽ ������ ���� �� ��� ���� �����÷ο� �߻� 
    public float MaxHp
    {
        get
        {
            return basic.maxHP;
        }
		set 
        {
            basic.maxHP = value;
        }
    }
    public float HP
	{
		get
		{
            return Mathf.Clamp(basic.hp, 0, MaxHp);
		}
		set
		{
            basic.hp = value;
		}
	}
    public float MaxMp
    {
        get
        {
            return basic.maxMP;
            
        }
		set
		{
            basic.maxMP = value;
        }
    }
    public float MP
	{
		get
		{
            return Mathf.Clamp(basic.mp, 0, MaxMp);
        }
		set
		{
            basic.mp = value;
        }
	}

    public float AttackPower
    {
        get
        {
            return basic.attackPower;
		}
		set
		{
          basic.attackPower = value;
        }
    }
    public float AttackRate
    {
        get
        {
            return basic.attackRate;
		}
		set
		{
            basic.attackRate= value;
        }
    }
    public float MoveSpeed
    {
        get
        {
            return basic.moveSpeed;
		}
		set
		{
          basic.moveSpeed =value;
        }
    }
    public float AttackRange
    {
        get
        {
            return basic.attackRange;
		}
		set
		{
          basic.attackRange = value;
        }
    }
    public float Defense
	{
		get
		{
            return basic.defense;
		}
		set
		{
          basic.defense = value;
        }
	}
	private void Start()
	{
        basic.hp = basic.maxHP;
        basic.mp = basic.maxMP;

        HP = MaxHp;
        MP = MaxMp;
	}
	private void Update()
	{
        basic.hp = Mathf.Clamp(basic.hp, 0, MaxHp);
        if (exp >= maxExp)
		{
            LevelUp();
		}
	}
    public void TakeEXP(float exp)
	{
        this.exp += exp;
        BottomUI.Instance.UpdateBottomUi();
	}
	public void LevelUp()
    {
        ParticleSystem lvUp = Instantiate(levelUpFx, transform.position, transform.rotation);

        // ������ ����Ʈ ���
        lvUp.Play();
        // ������ 1 �ø���.
        basic.level += 1;
        exp = 0;
        maxExp = (basic.level + 2) * basic.level * 100;

        MaxHp = basic.maxHP + (grow.maxHP*basic.level);
        MaxMp = basic.maxMP +( grow.maxMP*basic.level);
        AttackPower = basic.attackPower + (grow.attackPower*basic.level);

        // ü���� �ִ�ü������ ȸ��.
        HP = MaxHp;
        MP = MaxMp;

        BottomUI.Instance.UpdateBottomUi();

    }
}
