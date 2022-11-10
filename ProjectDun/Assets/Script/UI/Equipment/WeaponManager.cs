using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
	public static WeaponManager Instance { get; private set; }
	[SerializeField] PlayerStat playerStat;
	[SerializeField] Inventory inven;

	[SerializeField] WeaponSlot mainSlot;
	[SerializeField] WeaponSlot subSlot;

	[SerializeField] GameObject[] weaponModel;

	[SerializeField] ItemWindow itemDescription;
	[SerializeField] ItemWindow compareItemWindow;

	
	private void Awake()
	{
		Instance = this;
	}
	private void Start()
	{
		SlotSetUp();
	}
	public void SlotSetUp()
	{
		mainSlot.stat = playerStat;
		mainSlot.inventory = inven;
		mainSlot.itemDescription = itemDescription;
		mainSlot.compareItemWindow = compareItemWindow;

		subSlot.stat = playerStat;
		subSlot.inventory = inven;
		subSlot.itemDescription = itemDescription;
		subSlot.compareItemWindow = compareItemWindow;
	}
	public void WeaponSetup(Weapon weapon,int slotNum)
	{
		Weapon mainWeapon = null;
		Weapon subWeapon = null;
		if (mainSlot.slotItem != null)
			mainWeapon = mainSlot.slotItem as Weapon;
		if (subSlot.slotItem != null)
			subWeapon = subSlot.slotItem as Weapon;

		/* 
		 
			1. ������ ��� ����ִٸ�, main�� �����Ѵ�.
			2. main ������ �������� �� �� �����̰�, �����Ϸ��� ������ ���� �� �� ����(�Ǵ� ����)���, �����Ϸ��� ��������
				subSlot�� �����Ѵ�.
			3. main ������ �������� ����ְ� �����Ϸ��� ���Ⱑ �� �� ������, main�� �����ϰ�, subSlot�� slotImage�� ������ ������ ��������Ʈ�� ���� ���� ���缭 �����Ѵ�.
				subSlot�� isAvailable�� false�� ��ȯ�Ѵ�.
			4. main ������ �������� ��չ����̰�, �����Ϸ��� ���Ⱑ �Ѽչ�����, subSlot�� isAvailable�� true�� ��ȯ�Ѵ�.
				��չ����� ������ �����ϰ� main(������ ��� sub)Slot�� �����Ѵ�.
				
		*/

		// �� �� ���⸦ �����Ϸ� �� ��. �� �׳� ���� ���� ���� �� ���� �κ��� �־��ָ� �Ǵ°� �ƴѰ�..?

		// ������ ��� ����� ��. ( �� �� ���⸦ �����Ϸ� �� �� )
		if (mainSlot.slotItem == null && subSlot.slotItem == null)
		{
			// ���� ����
			if (weapon.type == Weapon.TYPE.Shield)
			{
				subSlot.Equip(weapon);
				inven.inven.Remove(weapon);
			}
			// �� �� ���� ����
			else if (weapon.hand == Weapon.HAND.TwoHand)
			{
				mainSlot.Equip(weapon);
				inven.inven.Remove(weapon);

				// ���꽽�� ����.
				TwoHandEquip(weapon);
			}
			// �� �� ���� ����
			else
			{
				mainSlot.Equip(weapon);
				inven.inven.Remove(weapon);
			}
			return;
		}

		// main ���Կ� �� �չ��⸦ �����߰�, �� �� ���⸦ �����Ϸ� �� ��.(sub���Կ� �������� ���� ��.)
		if (mainWeapon.hand == Weapon.HAND.OneHand && weapon.hand == Weapon.HAND.OneHand && subWeapon == null)
		{
			subSlot.Equip(weapon);
			inven.inven.Remove(weapon);
			return;

		}

		// main ���Կ� �� �չ��⸦ �����߰�, �� �� ���⸦ �����Ϸ� �� ��.(sub ���� üũ ����)
		if (mainWeapon.hand == Weapon.HAND.OneHand && weapon.hand == Weapon.HAND.TwoHand)
		{
			// sub ���� üũ
			if (subWeapon != null)
			{
				inven.Add(subWeapon);
				subSlot.EquipClear(subWeapon);
			}

			inven.inven[slotNum]=Swap(weapon, mainSlot);
			TwoHandEquip(weapon);

			return;
		}

		// main ���Կ� �� �չ��⸦ �����߰�, �� �� ���⸦ �����Ϸ� �� ��. ( �����Ϸ��� �������� ���ж�� sub )
		if (mainWeapon.hand == Weapon.HAND.TwoHand && weapon.hand == Weapon.HAND.OneHand)
		{
				TwoHandClear();     // ���꽽�� ����ȭ
			if (weapon.type == Weapon.TYPE.Shield)
			{
				inven.Add(mainWeapon);
				mainSlot.EquipClear(mainWeapon);
				subSlot.Equip(weapon);
			}
			else
				inven.inven[slotNum] = Swap(weapon, mainSlot);	// ���������� ��ü

			return;
		}

		// main ���Կ� �� �� ���⸦ �����߰�, �� �� ���⸦ �����Ϸ� �� ��.
		if (mainWeapon.hand == Weapon.HAND.TwoHand && weapon.hand == Weapon.HAND.TwoHand)
		{
			inven.inven[slotNum] = Swap(weapon, mainSlot);
			TwoHandEquip(weapon);
			return;
		}

		// sub ���Կ� ���и� �����߰�, �� �� ���⸦ �����Ϸ� �� ��.
		if (subWeapon.type == Weapon.TYPE.Shield && weapon.hand == Weapon.HAND.OneHand)
		{
			// ���и� ������ �� ��.
			if (weapon.type == Weapon.TYPE.Shield)
			{
				inven.inven[slotNum] = Swap(weapon, subSlot);
				return;
			}

			// �� �� ���⸦ ������ ��
			if (mainWeapon != null)		// ���� ���Կ� ������ ��� ���� ��
			{
				inven.inven[slotNum] = Swap(weapon, mainSlot);
				return;
			}
			else    //���� ���Կ� ������ ��� ���� ��
			{
				mainSlot.Equip(weapon);
				return;
			}
		}

		// sub ���Կ� ���и� �����߰�, �� �� ���⸦ �����Ϸ� �� ��.
		if (mainWeapon==null&&subWeapon!=null && weapon.hand == Weapon.HAND.TwoHand)
		{
			inven.Add(subWeapon);
			subSlot.EquipClear(subWeapon);
			mainSlot.Equip(weapon);
			TwoHandEquip(weapon);
			return;
		}

		//  main sub ���Կ� �� �� ���⸦ �����߰�, �� �� ���⸦ �����Ϸ� �� ��

		// 1. I�� ������ �����԰� ���â�� �Բ� ���� Ŭ�� �̺�Ʈ�� ������Ų��.
		// 2. �巡�� �� ������� ���ϴ� ���Կ� ������Ų��.
		// 3. ���� / ���� �ȳ� ��ư�� �˾����� ������ ���� �����ǰ� �Ѵ�.

		

		SwitchEquip();
	}
	// ��ư�� Equip �Լ�
	public void EquipMain()
	{

	}
	public void EquipSub()
	{

	}


	// �� �� ���� ����. ���꽽�� ���
	private void TwoHandEquip(Weapon weapon)
	{
		subSlot.isAvailable = false;
		subSlot.slotImage.sprite = weapon.itemSprite;
		Color newColor = subSlot.slotImage.color;
		newColor.a = 0.4f;
		subSlot.slotImage.color = newColor;
	}

	// �� �� ���� ���� ����. ���꽽�� ����ȭ
	private void TwoHandClear()
	{
		subSlot.isAvailable = true;
		subSlot.slotImage.sprite = null;
		Color newColor = subSlot.slotImage.color;
		newColor.a = 1;
		subSlot.slotImage.color = newColor;
	}

	// ������ ���� �� ������ �𵨸� On Off
	public void SwitchEquip()
	{
		if (mainSlot.slotItem != null)
			SetWeaponActive(mainSlot);

		else if (subSlot.slotItem != null)
			SetWeaponActive(subSlot);
	}
	private void SetWeaponActive(WeaponSlot slot)
	{

	}

	// ��� ����
	public Weapon Swap(Weapon equipment, WeaponSlot e)
	{
		Weapon temp = null;

		temp = (Weapon)e.slotItem;
		e.Equip(equipment);

		return temp;
	}
}
