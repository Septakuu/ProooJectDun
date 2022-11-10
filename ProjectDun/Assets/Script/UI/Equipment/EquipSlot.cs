using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EquipSlot : Slot
{
	public Equipment.PARTS parts;
	public PlayerStat stat;

	// 장착
	public void Equip(Equipment equip)
	{
		slotItem = equip;
		SetUp(slotItem);
		EquipSetUp(equip);
	}

	// equip slot, PlayerStat에 정보 전달.
	void EquipSetUp(Equipment equip)
	{
		slotImage.sprite = equip.itemSprite;    // 장비 스프라이트 전달
		stat.Defense += equip.defense;
		stat.MaxHp += equip.hp;
		BottomUI.Instance.UpdateBottomUi();         // 체력 등 변경 값 하단 UI에 전달
	}
	// 장착 해제
	void EquipClear(Equipment equip)
	{
		slotImage.sprite = null;
		slotItem = null;
		stat.Defense -= equip.defense;
		stat.MaxHp -= equip.hp;
		BottomUI.Instance.UpdateBottomUi();     // 변경 값 하단 UI에 전달
	}

	public override void OnPointerClick(PointerEventData eventData)
	{
			// UI에 적용되는 OnPointerClick 은 PointerEventData에 버튼 입력정보 전달, Input.GetMouse 아님
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
