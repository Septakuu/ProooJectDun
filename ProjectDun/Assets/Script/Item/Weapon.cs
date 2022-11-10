using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DB", menuName = "Item/Weapon")]
public class Weapon : Item
{
	[System.Serializable]
  public enum HAND
	{
		OneHand,
		TwoHand,
	}

	[System.Serializable]
	public enum TYPE
	{
		Sword,
		Axe,
		Blunt,
		Dagger,
		Bow,
		Staff,
		Shield,
	}

	public HAND hand;
	public TYPE type;
	public float power, rate;
	
}
