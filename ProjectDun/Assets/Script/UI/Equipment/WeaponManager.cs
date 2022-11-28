using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
	public static WeaponManager Instance { get; private set; }

	[SerializeField] Animator anim;
	[SerializeField] PlayerStat playerStat;
	[SerializeField] Inventory inven;

	[SerializeField] WeaponSlot mainSlot;
	[SerializeField] WeaponSlot subSlot;

	[SerializeField] GameObject[] weaponModel;

	[SerializeField] ItemWindow itemDescription;
	[SerializeField] ItemWindow compareItemWindow;

	public Weapon invenWeapon;
	public int invenSlotNum;

	Weapon mainWeapon;
	Weapon subWeapon;
	private void Awake()
	{
		Instance = this;
	}
	private void Start()
	{
		SlotSetUp();
		AnimStateChange();
	}

	public void AnimStateChange()
	{
		if (mainWeapon != null)
		{
			anim.SetBool("isUnarmed", false);
			if (mainWeapon.type == Weapon.TYPE.Bow)
			{
				anim.SetBool("isBow", true);
				anim.SetBool("isOneHand", false);
				anim.SetBool("isTwoHand", false);
			}
			else
			{
				switch (mainWeapon.hand)
				{
					case Weapon.HAND.OneHand:
						anim.SetBool("isBow", false);
						anim.SetBool("isOneHand", true);
						anim.SetBool("isTwoHand", false);
						break;
					case Weapon.HAND.TwoHand:
						anim.SetBool("isBow", false);
						anim.SetBool("isOneHand", false);
						anim.SetBool("isTwoHand", true);
						break;
					default:
						break;
				}
			}
		}
		else if (mainWeapon == null)
		{
			anim.SetBool("isUnarmed", true);
			anim.SetBool("isBow", false);
			anim.SetBool("isOneHand", false);
			anim.SetBool("isTwoHand", false);

		}
	}

	public void WeaponSlotInfoSend()
	{
		mainWeapon = mainSlot.slotItem as Weapon;
		subWeapon = subSlot.slotItem as Weapon;
	}

	public void SlotSetUp()
	{
		mainSlot.weaponManager = this;
		mainSlot.stat = playerStat;
		mainSlot.inventory = inven;
		mainSlot.itemDescription = itemDescription;
		mainSlot.compareItemWindow = compareItemWindow;

		subSlot.weaponManager = this;
		subSlot.stat = playerStat;
		subSlot.inventory = inven;
		subSlot.itemDescription = itemDescription;
		subSlot.compareItemWindow = compareItemWindow;
		
	}
	public void WeaponSetup(Weapon weapon,int slotNum)
	{
		invenWeapon = weapon;
		invenSlotNum = slotNum;

		SwitchEquip();
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
			if (invenWeapon.type == Weapon.TYPE.Shield)
			{
				subSlot.Equip(invenWeapon);
				inven.inven.Remove(invenWeapon);
			}
			// �� �� ���� ����
			else if (invenWeapon.hand == Weapon.HAND.TwoHand)
			{
				mainSlot.Equip(invenWeapon);
				inven.inven.Remove(invenWeapon);

				// ���꽽�� ����.
				TwoHandEquip(invenWeapon);
			}
			// �� �� ���� ����
			else
			{
				mainSlot.Equip(invenWeapon);
				inven.inven.Remove(invenWeapon);
			}
			WeaponSlotInfoSend();
			AnimStateChange();
			return;
		}

		// main ���Կ� �� �չ��⸦ �����߰�, �� �� ���⸦ �����Ϸ� �� ��.(sub���Կ� �������� ���� ��.)
		if (mainWeapon.hand == Weapon.HAND.OneHand && invenWeapon.hand == Weapon.HAND.OneHand && subWeapon == null)
		{
			subSlot.Equip(invenWeapon);
			inven.inven.Remove(invenWeapon);
			WeaponSlotInfoSend();
			AnimStateChange();
			return;

		}

		// main ���Կ� �� �չ��⸦ �����߰�, �� �� ���⸦ �����Ϸ� �� ��.(sub ���� üũ ����)
		if (mainWeapon.hand == Weapon.HAND.OneHand && invenWeapon.hand == Weapon.HAND.TwoHand)
		{
			// sub ���� üũ
			if (subWeapon != null)
			{
				inven.Add(subWeapon);
				subSlot.EquipClear(subWeapon);
			}

			inven.inven[slotNum]=Swap(invenWeapon, mainSlot);
			TwoHandEquip(invenWeapon);
			WeaponSlotInfoSend();
			AnimStateChange();
			return;
		}

		// main ���Կ� �� �չ��⸦ �����߰�, �� �� ���⸦ �����Ϸ� �� ��. ( �����Ϸ��� �������� ���ж�� sub )
		if (mainWeapon.hand == Weapon.HAND.TwoHand && invenWeapon.hand == Weapon.HAND.OneHand)
		{
				TwoHandClear();     // ���꽽�� ����ȭ
			if (invenWeapon.type == Weapon.TYPE.Shield)
			{
				inven.Add(mainWeapon);
				mainSlot.EquipClear(mainWeapon);
				subSlot.Equip(invenWeapon);
			}
			else
				inven.inven[slotNum] = Swap(invenWeapon, mainSlot); // ���������� ��ü

			WeaponSlotInfoSend();
			AnimStateChange();
			return;
		}

		// main ���Կ� �� �� ���⸦ �����߰�, �� �� ���⸦ �����Ϸ� �� ��.
		if (mainWeapon.hand == Weapon.HAND.TwoHand && invenWeapon.hand == Weapon.HAND.TwoHand)
		{
			inven.inven[slotNum] = Swap(invenWeapon, mainSlot);
			TwoHandEquip(invenWeapon);
			WeaponSlotInfoSend();
			AnimStateChange();
			return;
		}

		// sub ���Կ� ���и� �����߰�, �� �� ���⸦ �����Ϸ� �� ��.
		if (subWeapon.type == Weapon.TYPE.Shield && invenWeapon.hand == Weapon.HAND.OneHand)
		{
			// ���и� ������ �� ��.
			if (invenWeapon.type == Weapon.TYPE.Shield)
			{
				inven.inven[slotNum] = Swap(invenWeapon, subSlot);
				WeaponSlotInfoSend();
				AnimStateChange();
				return;
			}

			// �� �� ���⸦ ������ ��
			if (mainWeapon != null)		// ���� ���Կ� ������ ��� ���� ��
			{
				inven.inven[slotNum] = Swap(invenWeapon, mainSlot);
				WeaponSlotInfoSend();
				AnimStateChange();
				return;
			}
			else    //���� ���Կ� ������ ��� ���� ��
			{
				mainSlot.Equip(invenWeapon);
				WeaponSlotInfoSend();
				AnimStateChange();
				return;
			}
		}

		// sub ���Կ� ���и� �����߰�, �� �� ���⸦ �����Ϸ� �� ��.
		if (mainWeapon==null&&subWeapon!=null && invenWeapon.hand == Weapon.HAND.TwoHand)
		{
			inven.Add(subWeapon);
			subSlot.EquipClear(subWeapon);
			mainSlot.Equip(invenWeapon);
			TwoHandEquip(invenWeapon);

			WeaponSlotInfoSend();
			AnimStateChange();
			return;
		}

		//  main sub ���Կ� �� �� ���⸦ �����߰�, �� �� ���⸦ �����Ϸ� �� ��
		if(mainWeapon!=null&&subWeapon!=null&& invenWeapon.hand == Weapon.HAND.OneHand)
		{
			// �� �� ������ ���¿��� �� �� ���� Ŭ�� �� ��� ���� �׵θ��� ǥ�� << �� ���¿��� �ش� ���� ���� Ŭ�� �� 
			// �ش� ��ġ�� ��� ����.
			mainSlot.selectImage.enabled = true;
			subSlot.selectImage.enabled = true;

		}
		// 1. I�� ������ �����԰� ���â�� �Բ� ���� Ŭ�� �̺�Ʈ�� ������Ų��. << �̰ɷ� ����
		// 2. �巡�� �� ������� ���ϴ� ���Կ� ������Ų��.
		
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

	public void SelectImageOff()
	{
		mainSlot.selectImage.enabled = false;
		subSlot.selectImage.enabled = false;
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
