using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "DB", menuName = "Item/ConsumeItem")]
public class ConsumeItem : Item
{
	[System.Serializable]
	public enum EFFECT
	{
		Hp,
		Mp,
		Buff,
		Damage,
		Other,
	}
	public EFFECT effect;
	public float effection;
	public int count;
}
