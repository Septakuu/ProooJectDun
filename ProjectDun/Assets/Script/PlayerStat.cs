using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStat : Stat
{
    public float mp;
    public float maxMp;
    public int exp;
    public int maxExp;
    public int str;
    public int dex;
    public int wis;         // ���� int


	private void Update()
	{
        if (exp >= maxExp)
		{
            LevelUp();
		}
	}
	public void LevelUp()
    {
        // ������ 1 �ø���.
        
        basic.level += 1;
        exp = 0;
        maxExp = (basic.level + 2) * basic.level * 100;
        // ������ ����Ʈ ���?

        // ü���� �ִ�ü������ ȸ��.
        hp = maxHp;
        mp = maxMp;
    }
}
