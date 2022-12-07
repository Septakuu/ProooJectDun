using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropUIManager : MonoBehaviour
{
	public static DropUIManager Instance { get; private set; }
	[SerializeField] DropItemUI dropItemUI;
	[SerializeField] Canvas mainCanvas;
	[SerializeField] Inventory inven;
	Stack<DropItemUI> dropItemUIStack;
	private void Awake()
	{
		Instance = this;
	}
	private void Start()
	{
		dropItemUIStack = new Stack<DropItemUI>();
		for (int i =0; i < 20; i++)
		{
			dropItemUIStack.Push(Instantiate(dropItemUI,transform));
		}
	}
	public void SetUp(DropItem dropItem)
	{
		DropItemUI newDropUI = dropItemUIStack.Pop();
		newDropUI.gameObject.SetActive(true);
		newDropUI.transform.SetParent(mainCanvas.transform);
		Debug.Log("Stack Pop 카운트 : " + dropItemUIStack.Count);
		newDropUI.SetUp(dropItem);
	}
	public void Rooting(DropItem item,DropItemUI ui)
	{
		ui.transform.SetParent(transform);
		dropItemUIStack.Push(ui);
		Debug.Log("Stack Push 카운트 : " + dropItemUIStack.Count);
		inven.Add(item.itemInfo);
	}


}
