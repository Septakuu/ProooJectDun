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
		 
			1. 슬롯이 모두 비어있다면, main에 장착한다.
			2. main 슬롯의 아이템이 한 손 무기이고, 장착하려는 아이템 또한 한 손 무기(또는 방패)라면, 장착하려는 아이템은
				subSlot에 장착한다.
			3. main 슬롯의 아이템이 비어있고 장착하려는 무기가 양 손 무기라면, main에 장착하고, subSlot의 slotImage는 동일한 아이템 스프라이트의 알파 값을 낮춰서 전달한다.
				subSlot의 isAvailable을 false로 전환한다.
			4. main 슬롯이 아이템이 양손무기이고, 장착하려는 무기가 한손무기라면, subSlot의 isAvailable을 true로 전환한다.
				양손무기의 장착을 해제하고 main(방패의 경우 sub)Slot에 장착한다.
				
		*/

		// 두 손 무기를 장착하려 할 때. 는 그냥 메인 서브 슬롯 다 빼서 인벤에 넣어주면 되는거 아닌가..?

		// 슬롯이 모두 비었을 때. ( 두 손 무기를 장착하려 할 때 )
		if (mainSlot.slotItem == null && subSlot.slotItem == null)
		{
			// 방패 장착
			if (invenWeapon.type == Weapon.TYPE.Shield)
			{
				subSlot.Equip(invenWeapon);
				inven.inven.Remove(invenWeapon);
			}
			// 두 손 무기 장착
			else if (invenWeapon.hand == Weapon.HAND.TwoHand)
			{
				mainSlot.Equip(invenWeapon);
				inven.inven.Remove(invenWeapon);

				// 서브슬롯 조정.
				TwoHandEquip(invenWeapon);
			}
			// 한 손 무기 장착
			else
			{
				mainSlot.Equip(invenWeapon);
				inven.inven.Remove(invenWeapon);
			}
			WeaponSlotInfoSend();
			AnimStateChange();
			return;
		}

		// main 슬롯에 한 손무기를 장착했고, 한 손 무기를 장착하려 할 때.(sub슬롯에 아이템이 없을 때.)
		if (mainWeapon.hand == Weapon.HAND.OneHand && invenWeapon.hand == Weapon.HAND.OneHand && subWeapon == null)
		{
			subSlot.Equip(invenWeapon);
			inven.inven.Remove(invenWeapon);
			WeaponSlotInfoSend();
			AnimStateChange();
			return;

		}

		// main 슬롯에 한 손무기를 장착했고, 두 손 무기를 장착하려 할 때.(sub 슬롯 체크 포함)
		if (mainWeapon.hand == Weapon.HAND.OneHand && invenWeapon.hand == Weapon.HAND.TwoHand)
		{
			// sub 슬롯 체크
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

		// main 슬롯에 두 손무기를 장착했고, 한 손 무기를 장착하려 할 때. ( 장착하려는 아이템이 방패라면 sub )
		if (mainWeapon.hand == Weapon.HAND.TwoHand && invenWeapon.hand == Weapon.HAND.OneHand)
		{
				TwoHandClear();     // 서브슬롯 정상화
			if (invenWeapon.type == Weapon.TYPE.Shield)
			{
				inven.Add(mainWeapon);
				mainSlot.EquipClear(mainWeapon);
				subSlot.Equip(invenWeapon);
			}
			else
				inven.inven[slotNum] = Swap(invenWeapon, mainSlot); // 장착아이템 교체

			WeaponSlotInfoSend();
			AnimStateChange();
			return;
		}

		// main 슬롯에 두 손 무기를 장착했고, 두 손 무기를 장착하려 할 떄.
		if (mainWeapon.hand == Weapon.HAND.TwoHand && invenWeapon.hand == Weapon.HAND.TwoHand)
		{
			inven.inven[slotNum] = Swap(invenWeapon, mainSlot);
			TwoHandEquip(invenWeapon);
			WeaponSlotInfoSend();
			AnimStateChange();
			return;
		}

		// sub 슬롯에 방패를 장착했고, 한 손 무기를 장착하려 할 때.
		if (subWeapon.type == Weapon.TYPE.Shield && invenWeapon.hand == Weapon.HAND.OneHand)
		{
			// 방패를 끼려고 할 때.
			if (invenWeapon.type == Weapon.TYPE.Shield)
			{
				inven.inven[slotNum] = Swap(invenWeapon, subSlot);
				WeaponSlotInfoSend();
				AnimStateChange();
				return;
			}

			// 한 손 무기를 장착할 때
			if (mainWeapon != null)		// 메인 슬롯에 장착한 장비가 있을 때
			{
				inven.inven[slotNum] = Swap(invenWeapon, mainSlot);
				WeaponSlotInfoSend();
				AnimStateChange();
				return;
			}
			else    //메인 슬롯에 장착한 장비가 없을 때
			{
				mainSlot.Equip(invenWeapon);
				WeaponSlotInfoSend();
				AnimStateChange();
				return;
			}
		}

		// sub 슬롯에 방패를 장착했고, 두 손 무기를 장착하려 할 때.
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

		//  main sub 슬롯에 한 손 무기를 장착했고, 한 손 무기를 장착하려 할 때
		if(mainWeapon!=null&&subWeapon!=null&& invenWeapon.hand == Weapon.HAND.OneHand)
		{
			// 둘 다 장착한 상태에서 한 손 무기 클릭 시 장비 슬롯 테두리에 표시 << 이 상태에서 해당 슬롯 왼쪽 클릭 시 
			// 해당 위치에 장비 장착.
			mainSlot.selectImage.enabled = true;
			subSlot.selectImage.enabled = true;

		}
		// 1. I를 누르면 보관함과 장비창이 함께 떠서 클릭 이벤트로 연동시킨다. << 이걸로 가자
		// 2. 드래그 앤 드롭으로 원하는 슬롯에 장착시킨다.
		
	}

	// 양 손 무기 장착. 서브슬롯 잠금
	private void TwoHandEquip(Weapon weapon)
	{
		subSlot.isAvailable = false;
		subSlot.slotImage.sprite = weapon.itemSprite;
		Color newColor = subSlot.slotImage.color;
		newColor.a = 0.4f;
		subSlot.slotImage.color = newColor;
	}

	// 양 손 무기 장착 해제. 서브슬롯 정상화
	private void TwoHandClear()
	{
		subSlot.isAvailable = true;
		subSlot.slotImage.sprite = null;
		Color newColor = subSlot.slotImage.color;
		newColor.a = 1;
		subSlot.slotImage.color = newColor;
	}

	// 아이템 장착 시 아이템 모델링 On Off
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

	// 장비 스왑
	public Weapon Swap(Weapon equipment, WeaponSlot e)
	{
		Weapon temp = null;

		temp = (Weapon)e.slotItem;
		e.Equip(equipment);

		return temp;
	}
}
