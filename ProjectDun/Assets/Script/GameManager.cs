using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    public void OnQuit()
	{
		Application.Quit();
	}
	public void OnRetry()
	{
		SceneManager.LoadScene("Dungeon");
	}
}
