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
