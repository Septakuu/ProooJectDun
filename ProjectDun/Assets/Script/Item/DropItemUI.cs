using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
public class DropItemUI : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] GameObject panel;
    public TMP_Text uiText;
    public DropItem dropItem;
    
    Camera cam;
   
    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
        panel.gameObject.SetActive(false);
    }
    private void Update()
    {
        if (dropItem != null)
		{
            Debug.Log("DropUI에서 보는 아이템의 NP :"+dropItem.isNearPlayer);
            transform.position = cam.WorldToScreenPoint(dropItem.transform.position + new Vector3(0, 1, 0));
            if (dropItem.isNearPlayer)
			{
                panel.gameObject.SetActive(true);
			}
			else
			{
                panel.gameObject.SetActive(false);
			}
		}
    }
    public void SetUp(DropItem target)
    {
        dropItem = target;
        uiText.text = dropItem.itemInfo.itemName;
        // Vector3 UIpos = new Vector3(dropItem.transform.position.x, dropItem.transform.position.y + 3f, dropItem.transform.position.z);
    }
    public void SettingClear()
	{
        panel.gameObject.SetActive(false);
        dropItem = null;
        uiText.text = null;
	}
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            if (dropItem != null)
            {
                DropUIManager.Instance.Rooting(dropItem,this);
                Destroy(dropItem.gameObject);
                SettingClear();
            }
        }
    }

}
