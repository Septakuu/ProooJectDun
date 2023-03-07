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

    [SerializeField] GameObject[] mainHandWeaponModel;
    [SerializeField] GameObject[] subHandWeaponModel;

    [SerializeField] ItemWindow itemDescription;
    [SerializeField] ItemWindow compareItemWindow;

    public Weapon invenWeapon;
    public int invenSlotNum;

    [SerializeField] Weapon mainWeapon;
    [SerializeField] Weapon subWeapon;
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
    public void WeaponSlotInfoClear(WeaponSlot slot)
    {
        if (slot.mainSub == WeaponSlot.MAINSUB.Main)
        {
            mainWeapon = null;
        }
        if (slot.mainSub == WeaponSlot.MAINSUB.Sub)
        {
            subWeapon = null;
        }
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
    public void WeaponSetup(Weapon weapon, int slotNum)
    {
        invenWeapon = weapon;
        invenSlotNum = slotNum;


        /* 
		 
			1. ?????? ??? ???????, main?? ???????.
			2. main ?????? ???????? ?? ?? ???????, ????????? ?????? ???? ?? ?? ????(??? ????)???, ????????? ????????
				subSlot?? ???????.
			3. main ?????? ???????? ?????? ????????? ???? ?? ?? ??????, main?? ???????, subSlot?? slotImage?? ?????? ?????? ??????????? ???? ???? ???? ???????.
				subSlot?? isAvailable?? false?? ??????.
			4. main ?????? ???????? ?????????, ????????? ???? ????????, subSlot?? isAvailable?? true?? ??????.
				???????? ?????? ??????? main(?????? ??? sub)Slot?? ???????.
				
		*/

        // ?? ?? ???? ??????? ?? ??. ?? ??? ???? ???? ???? ?? ???? ?��??? ?????? ??��? ????..?

        // ?????? ??? ????? ??. ( ?? ?? ???? ??????? ?? ?? )
        if (mainSlot.slotItem == null && subSlot.slotItem == null)
        {
            // ???? ????
            if (invenWeapon.type == Weapon.TYPE.Shield)
            {
                subSlot.Equip(invenWeapon);
                inven.inven.Remove(invenWeapon);
            }
            // ?? ?? ???? ????
            else if (invenWeapon.hand == Weapon.HAND.TwoHand)
            {
                mainSlot.Equip(invenWeapon);
                inven.inven.Remove(invenWeapon);

                // ???��?? ????.
                TwoHandEquip(invenWeapon);
            }
            // ?? ?? ???? ????
            else
            {
                mainSlot.Equip(invenWeapon);
                inven.inven.Remove(invenWeapon);
            }
            WeaponSlotInfoSend();
            AnimStateChange();
            SwitchEquip();
            invenWeapon = null;

            return;
        }
        if (mainSlot.slotItem == null && subSlot.slotItem != null && invenWeapon.hand == Weapon.HAND.OneHand)
        {
            // ???? ????
            if (invenWeapon.type == Weapon.TYPE.Shield && subWeapon.type == Weapon.TYPE.Shield)
            {
                inven.inven[slotNum] = Swap(invenWeapon, subSlot);
                inven.inven.Remove(invenWeapon);
            }
            // ?? ?? ???? ????
            else if (invenWeapon.hand == Weapon.HAND.TwoHand)
            {
                mainSlot.Equip(invenWeapon);
                inven.inven.Remove(invenWeapon);

                // ???��?? ????.
                TwoHandEquip(invenWeapon);
            }
            // ?? ?? ???? ????
            else
            {
                mainSlot.Equip(invenWeapon);
                inven.inven.Remove(invenWeapon);
            }
            WeaponSlotInfoSend();
            AnimStateChange();
            SwitchEquip();
            invenWeapon = null;
            return;
        }
        // main ????? ?? ????? ???????, ?? ?? ???? ??????? ?? ??.(sub????? ???????? ???? ??.)
        if (mainWeapon != null && mainWeapon.hand == Weapon.HAND.OneHand && invenWeapon.hand == Weapon.HAND.OneHand && subWeapon == null)
        {
            subSlot.Equip(invenWeapon);
            inven.inven.Remove(invenWeapon);
            WeaponSlotInfoSend();
            AnimStateChange();
            SwitchEquip();
            invenWeapon = null;

            return;

        }

        // main ????? ?? ????? ???????, ?? ?? ???? ??????? ?? ??.(sub ???? ?? ????)
        if (mainWeapon != null && mainWeapon.hand == Weapon.HAND.OneHand && invenWeapon.hand == Weapon.HAND.TwoHand)
        {
            // sub ???? ??
            if (subWeapon != null)
            {
                inven.Add(subWeapon);
                subSlot.EquipClear(subWeapon);
            }

            inven.inven[slotNum] = Swap(invenWeapon, mainSlot);
            TwoHandEquip(invenWeapon);
            WeaponSlotInfoSend();
            AnimStateChange();
            SwitchEquip();
            invenWeapon = null;

            return;
        }

        // main ????? ?? ????? ???????, ?? ?? ???? ??????? ?? ??. ( ????????? ???????? ???��?? sub )
        if (mainWeapon != null && mainWeapon.hand == Weapon.HAND.TwoHand && invenWeapon.hand == Weapon.HAND.OneHand)
        {
            TwoHandClear();     // ???��?? ?????
            if (invenWeapon.type == Weapon.TYPE.Shield)
            {
                inven.Add(mainWeapon);
                mainSlot.EquipClear(mainWeapon);
                subSlot.Equip(invenWeapon);
            }
            else
                inven.inven[slotNum] = Swap(invenWeapon, mainSlot); // ?????????? ???

            WeaponSlotInfoSend();
            AnimStateChange();
            SwitchEquip();
            invenWeapon = null;

            return;
        }

        // main ????? ?? ?? ???? ???????, ?? ?? ???? ??????? ?? ??.
        if (mainWeapon != null && mainWeapon.hand == Weapon.HAND.TwoHand && invenWeapon.hand == Weapon.HAND.TwoHand)
        {
            inven.inven[slotNum] = Swap(invenWeapon, mainSlot);
            TwoHandEquip(invenWeapon);
            WeaponSlotInfoSend();
            AnimStateChange();
            SwitchEquip();
            invenWeapon = null;

            return;
        }

        // sub ????? ???��? ???????, ?? ?? ???? ??????? ?? ??.
        if (subWeapon.type == Weapon.TYPE.Shield && invenWeapon.hand == Weapon.HAND.OneHand)
        {
            // ???��? ?????? ?? ??.
            if (invenWeapon.type == Weapon.TYPE.Shield)
            {
                inven.inven[slotNum] = Swap(invenWeapon, subSlot);
                WeaponSlotInfoSend();
                AnimStateChange();
                SwitchEquip();
                invenWeapon = null;

                return;
            }

            // ?? ?? ???? ?????? ??
            if (mainWeapon != null)     // ???? ????? ?????? ??? ???? ??
            {
                inven.inven[slotNum] = Swap(invenWeapon, mainSlot);
                WeaponSlotInfoSend();
                AnimStateChange();
                SwitchEquip();
                invenWeapon = null;

                return;
            }
            else    //???? ????? ?????? ??? ???? ??
            {
                mainSlot.Equip(invenWeapon);
                WeaponSlotInfoSend();
                AnimStateChange();
                SwitchEquip();
                invenWeapon = null;
                return;
            }
        }

        // sub ????? ???��? ???????, ?? ?? ???? ??????? ?? ??.
        if (mainWeapon == null && subWeapon != null && invenWeapon.hand == Weapon.HAND.TwoHand)
        {
            inven.Add(subWeapon);
            subSlot.EquipClear(subWeapon);
            mainSlot.Equip(invenWeapon);
            TwoHandEquip(invenWeapon);
            inven.inven.Remove(invenWeapon);

            WeaponSlotInfoSend();
            AnimStateChange();
            SwitchEquip();
            invenWeapon = null;
            return;
        }

        //  main sub ????? ?? ?? ???? ???????, ?? ?? ???? ??????? ?? ??
        if (mainWeapon != null && subWeapon != null && invenWeapon.hand == Weapon.HAND.OneHand)
        {
            // ?? ?? ?????? ???��??? ?? ?? ???? ??? ?? ??? ???? ??��??? ??? << ?? ???��??? ??? ???? ???? ??? ?? 
            // ??? ????? ??? ????.
            mainSlot.selectImage.enabled = true;
            subSlot.selectImage.enabled = true;

        }
        // 1. I?? ?????? ??????? ?????? ??? ???? ??? ?????? ?????????. << ???? ????
        // 2. ?��?? ?? ??????? ????? ????? ?????????.

    }

    // ?? ?? ???? ????. ???��?? ???
    private void TwoHandEquip(Weapon weapon)
    {
        subSlot.isAvailable = false;
        subSlot.slotImage.sprite = weapon.itemSprite;
        Color newColor = subSlot.slotImage.color;
        newColor.a = 0.4f;
        subSlot.slotImage.color = newColor;
    }

    // ?? ?? ???? ???? ????. ???��?? ?????
    public void TwoHandClear()
    {
        subSlot.isAvailable = true;
        subSlot.slotImage.sprite = null;
        Color newColor = subSlot.slotImage.color;
        newColor.a = 1;
        subSlot.slotImage.color = newColor;
    }

    // ?????? ???? ?? ?????? ??? On Off
    public void SwitchEquip()
    {
        SetWeaponActive(mainSlot);
        SetWeaponActive(subSlot);
    }

    private void SetWeaponActive(WeaponSlot slot)
    {
        if (slot.slotItem != null)
        {
            if (slot.mainSub == WeaponSlot.MAINSUB.Main)
            {
                foreach (GameObject a in mainHandWeaponModel)
                {
                    if (a.name == slot.slotItem.itemCode)
                        a.SetActive(true);
                    else
                        a.SetActive(false);
                }
            }
            else if (slot.mainSub == WeaponSlot.MAINSUB.Sub)
            {
                foreach (GameObject a in subHandWeaponModel)
                {
                    if (a.name == slot.slotItem.itemCode)
                        a.SetActive(true);
                    else
                        a.SetActive(false);
                }
            }
        }
        else
        {
            if (slot.mainSub == WeaponSlot.MAINSUB.Main)
                foreach (GameObject a in mainHandWeaponModel)
                {
                    a.SetActive(false);
                }
            if (slot.mainSub == WeaponSlot.MAINSUB.Sub)
                foreach (GameObject a in subHandWeaponModel)
                {
                    a.SetActive(false);
                }
        }
    }

    public void SelectImageOff()
    {
        mainSlot.selectImage.enabled = false;
        subSlot.selectImage.enabled = false;
    }

    // ??? ????
    public Weapon Swap(Weapon equipment, WeaponSlot e)
    {
        Weapon temp = null;

        temp = (Weapon)e.slotItem;
        e.Equip(equipment);

        return temp;
    }
}
