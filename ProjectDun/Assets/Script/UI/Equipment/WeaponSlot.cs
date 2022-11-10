using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class WeaponSlot : Slot
{
	public enum MAINSUB
	{
		Main,
		Sub,
	}

	public MAINSUB mainSub;
	public PlayerStat stat;

	public bool isAvailable;

	private void Start()
	{
		isAvailable = slotItem == null;
	}
	// ����
	public void Equip(Weapon equip)
	{
		slotItem = equip;
		SetUp(slotItem);
		EquipSetUp(equip);
	}

	// equip slot, PlayerStat�� ���� ����.
	void EquipSetUp(Weapon equip)
	{
		slotImage.sprite = equip.itemSprite;			// ��� ��������Ʈ ����
		stat.AttackPower += equip.power;				// ���ݷ� ����.
		stat.AttackRate += equip.rate;					// ���� �ӵ� ����
	}
	// ���� ����
	public void EquipClear(Weapon equip)
	{
		slotImage.sprite = null;
		slotItem = null;
		stat.AttackPower -= equip.power;
		stat.AttackRate -= equip.rate;
	}

	// ���� ���콺 ��Ŭ�� �̺�Ʈ(��������)
	public override void OnPointerClick(PointerEventData eventData)
	{
		// UI�� ����Ǵ� OnPointerClick �� PointerEventData�� ��ư �Է����� ����, Input.GetMouse �ƴ�
		if (eventData.button == PointerEventData.InputButton.Right)
		{
			if (slotItem == null)
				return;
			if (this is WeaponSlot)
			{
				inventory.Add(slotItem);
				EquipClear(slotItem as Weapon);
				EquipManager.Instance.SwitchEquip();
			}
		}
	}
}
