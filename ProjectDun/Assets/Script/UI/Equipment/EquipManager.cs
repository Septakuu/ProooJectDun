using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipManager : MonoBehaviour
{
	public static EquipManager Instance { get; private set; }
	[SerializeField] PlayerStat playerStat;
	[SerializeField] GameObject panel;
	[SerializeField] Inventory inven;

	[SerializeField] EquipSlot[] equips;
	[SerializeField] GameObject[] equipments;

	[SerializeField] ItemWindow itemDescription;
	[SerializeField] ItemWindow compareItemWindow;

	private void Awake()
	{
		Instance = this;
	}
	private void Start()
	{
		panel.SetActive(false);

		foreach (EquipSlot e in equips)
		{
			e.stat = playerStat;
			e.inventory = inven;
			e.itemDescription = itemDescription;
			e.compareItemWindow = compareItemWindow;
		}
	}
	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.I))
		{
			panel.SetActive(!panel.activeSelf);
		}
	}
	public void EquipSetUp(Equipment equipment, int slotNum)
	{
		for (int i = 0; i < equips.Length; i++)
		{
			if (equips[i].parts == equipment.parts)
			{
				if (equips[i].slotItem != null)
					inven.inven[slotNum] = Swap(equipment, equips[i]);

				else
				{
					equips[i].Equip(equipment);
					inven.inven.Remove(equipment);
				}
			}
		}
		SwitchEquip();
	}

	public void SwitchEquip()
	{
		for(int i =0; i < equips.Length; i++)
		{
			bool isWear = equips[i].slotItem != null;
			equipments[i].SetActive(isWear);
		}
	}

	public Equipment Swap(Equipment equipment, EquipSlot e)
	{
		Equipment temp = null;

		temp = (Equipment)e.slotItem;
		e.Equip(equipment);

		return temp;
	}
}
