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
    public int wis;         // 지능 int


	private void Update()
	{
        if (exp >= maxExp)
		{
            LevelUp();
		}
	}
	public void LevelUp()
    {
        // 레벨을 1 올린다.
        
        basic.level += 1;
        exp = 0;
        maxExp = (basic.level + 2) * basic.level * 100;
        // 레벨업 이펙트 재생?

        // 체력을 최대체력으로 회복.
        hp = maxHp;
        mp = maxMp;
    }
}
