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

    // �����쿡 setup����� ���� �� ��. ����� slot�� ����־ start �ÿ� �κ��丮���� slot���� �־����������(������ ����) 
    // ������ ��ü�� setup����� �̰��ϸ� slot�� �ڽ��� ���� �����۸� window�� �����ϸ� ��.

	private void Start()
	{
        panel.SetActive(false);
	}
	public void SwitchPanel(bool isSwitch)
	{
        panel.SetActive(isSwitch);
	}
}
