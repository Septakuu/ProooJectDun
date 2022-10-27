using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

using UnityEngine.UI;
using TMPro;
public class Slot : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler,IPointerClickHandler
{
	[SerializeField] protected int index;
	[SerializeField] public Inventory inventory;
	public Item slotItem;
	public Image slotImage;
	public TMP_Text countText;

	 public ItemWindow itemDescription;
	 public ItemWindow compareItemWindow;

	public virtual void OnPointerClick(PointerEventData eventData)
	{
		if (eventData.button == PointerEventData.InputButton.Right)
		{
			if (slotItem == null)
				return;
			Debug.Log("Use ��");
			inventory.Use(index);
		}
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		if (slotItem == null)
			return;

		Debug.Log("Enter");
		SetUp(slotItem);
	}

	public void OnPointerExit(PointerEventData eventData)
	{

		Debug.Log("Exit");

		itemDescription.SwitchPanel(false);
	}

	public void SetUp(Item targetItem)
	{
		if(targetItem is EquipItem)
		{
			EquipItem item = targetItem as EquipItem;
			itemDescription.itemName.text = item.itemName;
			itemDescription.itemImage.sprite = item.itemSprite;

			itemDescription.limitLevel.text = string.Format($"���� ���� : {item.limitLevel}");
			itemDescription.limitLevel.gameObject.SetActive(true);

			itemDescription.itemStat1.text =
				string.Format(item.parts == EquipItem.PARTS.Weapon ? $"���ݷ� : {item.stat1}" : $"���� : {item.stat1}");

			itemDescription.itemStat2.text=
				string.Format(item.parts == EquipItem.PARTS.Weapon ? $"���ݼӵ� : {item.stat2}" : $"ü�� : {item.stat2}");
			itemDescription.itemStat2.gameObject.SetActive(true);

			itemDescription.itemDescription.text = slotItem.description;

			itemDescription.SwitchPanel(true);
		}
		else if (targetItem is ConsumeItem)
		{
			ConsumeItem item = targetItem as ConsumeItem;

			itemDescription.itemName.text = item.itemName;
			itemDescription.itemImage.sprite = item.itemSprite;

			itemDescription.limitLevel.gameObject.SetActive(false);
			itemDescription.itemStat1.text = string.Format($"{item.effect} ���� : {item.effection}");
			itemDescription.itemStat2.text = string.Format($"���� ���� : {countText.text}");


			itemDescription.itemDescription.text = item.description;

			itemDescription.SwitchPanel(true);
		}
	}
}
