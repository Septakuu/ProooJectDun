using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
	public static Inventory Instance { get; private set; }

	[SerializeField] ItemUsingManager usingManager;
	[SerializeField] EquipManager equipManager;
	[SerializeField] WeaponManager weaponManager;

	public GameObject panel;
	public List<Item> inven = new List<Item>();
	public Slot[] slots;
	public ItemWindow itemDescription;
	public ItemWindow compareItemWindow;
	public int gold;
	private void Start()
	{
		panel.gameObject.SetActive(false);
		Debug.Log(inven.Count);

		for (int i = 0; i < slots.Length; i++)
		{
			slots[i].slotImage.gameObject.SetActive(false);
			slots[i].countText.gameObject.SetActive(false);
			slots[i].itemDescription = itemDescription;
			slots[i].compareItemWindow = compareItemWindow;
			slots[i].inventory = this;
		}
	}
	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.I))
		{
			panel.SetActive(!panel.activeSelf);
			OnUpdateInven();
			if (panel.activeSelf == false)
			{
				itemDescription.panel.SetActive(false);
				compareItemWindow.panel.SetActive(false);
			}
		}
	}
	public void OnUpdateInven()
	{
		if (inven.Count >= 0)
		{
			for (int i = 0; i < slots.Length; i++)
			{
				if (i >= inven.Count)
				{
					slots[i].slotItem = null;
					slots[i].slotImage.gameObject.SetActive(false);
					slots[i].countText.gameObject.SetActive(false);
				}
				else if (inven[i] != null)
				{
					slots[i].slotItem = inven[i];
					slots[i].slotImage.sprite = slots[i].slotItem.itemSprite;
					slots[i].slotImage.gameObject.SetActive(true);
					if (slots[i].slotItem is ConsumeItem)
					{
						ConsumeItem consume = slots[i].slotItem as ConsumeItem;
						slots[i].countText.text = consume.count.ToString();
						slots[i].countText.gameObject.SetActive(true);
					}
				}
			}
		}
	}

	public void Use(int slotNum)
	{
		if (inven[slotNum] is Equipment)
		{
			Equipment item = inven[slotNum] as Equipment;
			equipManager.EquipSetUp(item, slotNum);
			itemDescription.SwitchPanel(false);
		}
		else if (inven[slotNum] is ConsumeItem)
		{
			ConsumeItem item = inven[slotNum] as ConsumeItem;
			usingManager.Use(item);
			inven[slotNum] = item;
			if (item.count <= 0)
			{
				inven.Remove(item);
				itemDescription.SwitchPanel(false);
			}
		}
		else if(inven[slotNum] is Weapon)
		{
			Weapon item = inven[slotNum] as Weapon;
			
			weaponManager.WeaponSetup(item,slotNum);
			itemDescription.SwitchPanel(false);
		}
		OnUpdateInven();
	}
	public void Add(Item item)
	{
		inven.Add(item);
		OnUpdateInven();
	}
}
