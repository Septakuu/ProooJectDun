using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class RewardRoom : MonoBehaviour
{
	[SerializeField] Image panel;
	[SerializeField] Text clearText;
	[SerializeField] Button QuitButton;
	[SerializeField] Button RetryButton;

	private void Start()
	{
		panel.color = new Color(0, 0, 0, 0);
		panel.enabled = false;
		clearText.enabled = false;
		QuitButton.gameObject.SetActive(false);
		RetryButton.gameObject.SetActive(false);
	}
	private void OnTriggerEnter(Collider other)
	{
		StartCoroutine(GameClear());
	}
	IEnumerator GameClear()
	{
		panel.enabled = true;
		float fadeCount = 0;
		while (fadeCount < 1.0f)
		{
			fadeCount += 0.01f;
			yield return new WaitForSeconds(0.01f);
			panel.color = new Color(0, 0, 0, fadeCount);
		}
		clearText.enabled = true;
		QuitButton.gameObject.SetActive(true);
		RetryButton.gameObject.SetActive(true);
	}
}
