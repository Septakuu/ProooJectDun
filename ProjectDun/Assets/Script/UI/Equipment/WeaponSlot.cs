using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;



public class WeaponSlot : Slot
{
	public enum MAINSUB
	{
		Main,
		Sub,
	}

	public Image selectImage;
	public MAINSUB mainSub;
	public PlayerStat stat;
	public WeaponManager weaponManager;
	public bool isAvailable;

	private void Start()
	{
		isAvailable = slotItem == null;
		selectImage.enabled = false;
	}
	// 장착
	public void Equip(Weapon equip)
	{
		slotItem = equip;
		SetUp(slotItem);
		EquipSetUp(equip);
	}

	// equip slot, PlayerStat에 정보 전달.
	void EquipSetUp(Weapon equip)
	{
		slotImage.sprite = equip.itemSprite;			// 장비 스프라이트 전달
		stat.AttackPower += equip.power;				// 공격력 전달.
		stat.AttackRate += equip.rate;					// 공격 속도 전달
	}
	// 장착 해제
	public void EquipClear(Weapon equip)
	{
		slotImage.sprite = null;
		slotItem = null;
		stat.AttackPower -= equip.power;
		stat.AttackRate -= equip.rate;
	}

	// 슬롯 마우스 우클릭 이벤트(장착해제)
	public override void OnPointerClick(PointerEventData eventData)
	{
		// UI에 적용되는 OnPointerClick 은 PointerEventData에 버튼 입력정보 전달, Input.GetMouse 아님
		if (eventData.button == PointerEventData.InputButton.Right)
		{
			if (slotItem == null)
				return;
			if (this is WeaponSlot)
			{
				inventory.Add(slotItem);
				EquipClear(slotItem as Weapon);
				WeaponManager.Instance.WeaponSlotInfoSend();
				WeaponManager.Instance.AnimStateChange();
				EquipManager.Instance.SwitchEquip();
			}
		}
		if (eventData.button == PointerEventData.InputButton.Left && selectImage.enabled == true)
		{
			if (slotItem == null)
				return;
			if(this is WeaponSlot)
			{
				inventory.inven[weaponManager.invenSlotNum] = weaponManager.Swap(weaponManager.invenWeapon, this);
				weaponManager.SelectImageOff();
				WeaponManager.Instance.WeaponSlotInfoSend();
				WeaponManager.Instance.AnimStateChange();
				inventory.OnUpdateInven();
			}
		}
	}
}
