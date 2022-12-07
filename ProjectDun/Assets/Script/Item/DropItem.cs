using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropItem : MonoBehaviour
{
    public Item itemInfo;
    public bool isNearPlayer;
	public PlayerMovement player;

	private void Update()
	{
		if (player != null)
		{
			if (Vector3.Distance(transform.position, player.transform.position) <= 15f)
			{
				isNearPlayer = true;
			}
			else
			{
				isNearPlayer = false;
			}
		}
	}
	public void SetUp(Item item)
	{
        itemInfo = item;
        DropUIManager.Instance.SetUp(this);
    }
}
