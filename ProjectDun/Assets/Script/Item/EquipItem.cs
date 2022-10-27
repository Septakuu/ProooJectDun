using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DB", menuName = "Item/EquipItem")]
public class EquipItem : Item
{
	[System.Serializable]
	public enum PARTS
	{
		Head,
		Armour,
		Glove,
		Pants,
		Shose,
		Weapon,
	}
	public PARTS parts;
	public int stat1;
	public float stat2;
	public float limitLevel;
}
