using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class ItemWindow : MonoBehaviour
{
    public GameObject panel;
    public Image itemImage;
    public TMP_Text itemName;
    public TMP_Text limitLevel;
    public TMP_Text itemStat1;
    public TMP_Text itemStat2;
    public TMP_Text itemDescription;

    // 윈도우에 setup기능을 만들어도 될 듯. 현재는 slot에 들어있어서 start 시에 인벤토리에서 slot에게 넣어줘야하지만(프리팹 문제) 
    // 윈도우 자체에 setup기능을 이관하면 slot은 자신이 가진 아이템만 window에 전달하면 끝.

	private void Start()
	{
        panel.SetActive(false);
	}
	public void SwitchPanel(bool isSwitch)
	{
        panel.SetActive(isSwitch);
	}
}
