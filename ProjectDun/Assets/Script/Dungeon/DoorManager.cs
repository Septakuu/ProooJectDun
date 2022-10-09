using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorManager : MonoBehaviour
{
	public static DoorManager Instance { get; private set; }
	[SerializeField] Door[] doors;
	private void Awake()
	{
		Instance = this;
	}
	public void OnSwitchDoor(bool isOpen)
	{
		foreach (Door d in doors)
		{
			d.OnSwitchDoor(isOpen);
			Debug.Log($"{d.name} {isOpen} ¿€µø!");
		}
	}
}
