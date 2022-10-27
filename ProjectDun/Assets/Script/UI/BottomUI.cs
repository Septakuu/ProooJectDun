using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class BottomUI : MonoBehaviour
{
    public static BottomUI Instance { get; private set; }
    [SerializeField] PlayerStat stat;
    [SerializeField] TMP_Text level;
    [SerializeField] TMP_Text hpText;
    [SerializeField] TMP_Text mpText;
    [SerializeField] TMP_Text expText;
    [SerializeField] Image expImage;
    [SerializeField] Image hpImage;
    [SerializeField] Image mpImage;
	private void Awake()
	{
        Instance = this;
	}

	void Start()
    {
        UpdateBottomUi();
    }

    public void UpdateBottomUi()
	{
        level.text = string.Format("lv. {0}",stat.basic.level);
        hpText.text = string.Format("{0}/{1}", stat.HP, stat.MaxHp);
        mpText.text = string.Format("{0}/{1}", stat.MP, stat.MaxMp);
        expText.text = string.Format("{0}/{1}", stat.exp, stat.maxExp);

        expImage.fillAmount = stat.exp / stat.maxExp;
        hpImage.fillAmount = stat.HP / stat.MaxHp;
        mpImage.fillAmount = stat.MP / stat.MaxMp;
    }
    
    
}
