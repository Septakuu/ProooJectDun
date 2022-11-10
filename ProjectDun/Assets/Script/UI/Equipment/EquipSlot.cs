using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EquipSlot : Slot
{
	public Equipment.PARTS parts;
	public PlayerStat stat;

	// ����
	public void Equip(Equipment equip)
	{
		slotItem = equip;
		SetUp(slotItem);
		EquipSetUp(equip);
	}

	// equip slot, PlayerStat�� ���� ����.
	void EquipSetUp(Equipment equip)
	{
		slotImage.sprite = equip.itemSprite;    // ��� ��������Ʈ ����
		stat.Defense += equip.defense;
		stat.MaxHp += equip.hp;
		BottomUI.Instance.UpdateBottomUi();         // ü�� �� ���� �� �ϴ� UI�� ����
	}
	// ���� ����
	void EquipClear(Equipment equip)
	{
		slotImage.sprite = null;
		slotItem = null;
		stat.Defense -= equip.defense;
		stat.MaxHp -= equip.hp;
		BottomUI.Instance.UpdateBottomUi();     // ���� �� �ϴ� UI�� ����
	}

	public override void OnPointerClick(PointerEventData eventData)
	{
			// UI�� ����Ǵ� OnPointerClick �� PointerEventData�� ��ư �Է����� ����, Input.GetMouse �ƴ�
		if (eventData.button == PointerEventData.InputButton.Right)
		{
			if (slotItem == null)
				return;
			if(this is EquipSlot)
			{
				inventory.Add(slotItem);
				EquipClear(slotItem as Equipment);
				EquipManager.Instance.SwitchEquip();
			}
		}
	}
}
