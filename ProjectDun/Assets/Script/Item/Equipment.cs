using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DB", menuName = "Item/Equipment")]
public class Equipment : Item
{
	[System.Serializable]
	public enum PARTS
	{
		Head,
		Chest,
		Hand,
		Legs,
		Shose,
	}

	public PARTS parts;
	public int defense;
	public float hp;
}