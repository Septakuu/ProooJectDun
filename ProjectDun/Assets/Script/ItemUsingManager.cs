using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemUsingManager : MonoBehaviour
{
	[SerializeField] PlayerStat stat;
	ConsumeItem item;
	Coroutine buffDuration;

	public void Use(ConsumeItem usingItem)
	{
		item = usingItem;

		if (item.count <= 0)
			return;

		switch (item.effect)
		{
			case ConsumeItem.EFFECT.Hp:
				if (stat.HP >= stat.MaxHp)
				{
					Debug.Log("현재 체력 이상으로 회복 할 수 없습니다.");
					return;
				}

				stat.HP +=item.effection;
				item.count--;
				BottomUI.Instance.UpdateBottomUi();
				break;
			case ConsumeItem.EFFECT.Mp:
				if (stat.MP >= stat.MaxMp)
				{
					Debug.Log("현재 마나 이상으로 회복 할 수 없습니다.");
					return;
				}

				stat.MP += item.effection;
				item.count--;
				BottomUI.Instance.UpdateBottomUi();

				break;
			case ConsumeItem.EFFECT.Buff:
				if (buffDuration != null)
					return;

				buffDuration = StartCoroutine(PowerUp());
				item.count--;

				break;
			case ConsumeItem.EFFECT.Damage:
				if (buffDuration != null)
					return;

				buffDuration = StartCoroutine(PowerUp());
				item.count--;

				break;
			case ConsumeItem.EFFECT.Other:
				break;
			default:
				break;
		}

	}
	IEnumerator PowerUp()
	{
		float time = 0;
		float limitTime = 30;
		while (time < limitTime)
		{
			time += Time.deltaTime;
			stat.AttackPower += item.effection;
			yield return null;
		}
		stat.AttackPower -= item.effection;
	}
}
