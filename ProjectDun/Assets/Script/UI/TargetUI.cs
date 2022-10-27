using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TargetUI : MonoBehaviour
{
	public static TargetUI Instance { get; private set; }

	[SerializeField] GameObject panel;
	[SerializeField] Image targetImage;
	[SerializeField] Image hpImage;
	[SerializeField] Image mpImage;
	[SerializeField] TMP_Text targetName;
	[SerializeField] TMP_Text targetLevel;
	[SerializeField] TMP_Text targetHpText;
	[SerializeField] TMP_Text targetMpText;

	Stat stat;
	private void Awake()
	{
		Instance = this;
	}
	private void Start()
	{
		panel.SetActive(false);
	}
	public void UpdateTargetUI(Damagable target)
	{
		if (target != null)
		{
			stat = target.stat;
			panel.SetActive(true);
			targetName.text = stat.Name;
			targetLevel.text = stat.Level.ToString();
			targetHpText.text = string.Format("{0} / {1}", stat.basic.hp, stat.basic.maxHP);
			targetMpText.text = string.Format("{0} / {1}", stat.basic.mp, stat.basic.maxMP);

			hpImage.fillAmount = stat.basic.hp / stat.basic.maxHP;
			mpImage.fillAmount = stat.basic.mp / stat.basic.maxMP;
		}
		else
			panel.SetActive(false);

	}
}
