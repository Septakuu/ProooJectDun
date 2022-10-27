using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EquipSlot : Slot
{
	public EquipItem.PARTS parts;
	public PlayerStat stat;

	// ����
	public void Equip(EquipItem equip)
	{
		slotItem = equip;
		SetUp(slotItem);
		EquipSetUp(equip);
	}

	// equip slot, PlayerStat�� ���� ����.
	void EquipSetUp(EquipItem equip)
	{
		slotImage.sprite = equip.itemSprite;	// ��� ��������Ʈ ����

		if(equip.parts == EquipItem.PARTS.Weapon)	// Ÿ���� �����̸� ���ݷ�, ���ݼӵ�
		{
			stat.AttackPower += equip.stat1;
			stat.AttackRate += equip.stat2;
		}
		else																// �������� ���°� �ִ� ü�¿� ����
		{
			stat.Defense += equip.stat1;
			stat.MaxHp += equip.stat2;
		}
			BottomUI.Instance.UpdateBottomUi();			// ü�� �� ���� �� �ϴ� UI�� ����
	}
	// ���� ����
	void EquipClear(EquipItem equip)
	{
		slotImage.sprite = null;
		slotItem = null;

		if (equip.parts == EquipItem.PARTS.Weapon)	// �����̸� ���ݷ¿� - ����
		{
			stat.AttackPower -= equip.stat1;
			stat.AttackRate -= equip.stat2;
		}
		else																// �������� ���� �� �ִ�ü�¿� ����
		{
			stat.Defense -= equip.stat1;
			stat.MaxHp -= equip.stat2;
		}
			BottomUI.Instance.UpdateBottomUi();		// ���� �� �ϴ� UI�� ����
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
				EquipClear(slotItem as EquipItem);
				EquipManager.Instance.SwitchEquip();
			}
		}
	}
}
