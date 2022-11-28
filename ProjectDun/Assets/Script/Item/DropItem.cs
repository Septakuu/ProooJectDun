using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropItem : MonoBehaviour
{
    public Item itemInfo;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void SetUp(Item item)
	{
        itemInfo = item;
	}
}
