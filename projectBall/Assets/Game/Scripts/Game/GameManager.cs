using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameManager : MonoBehaviour 
{
	bool credits = false;

	public Image textBackground;
	public Image creditsBackground;
	public Text creditsText;
	GameObject mainMenu;

	void Awake()
	{
		QualitySettings.vSyncCount = 1;
		DontDestroyOnLoad (gameObject);
	}

	void Start()
	{
		mainMenu = GameObject.FindGameObjectWithTag ("MainMenu");
	}

	void Update()
	{
		if (credits = true && Input.GetButtonDown("Cancel") || Input.GetButtonDown("Submit"))
		{
			credits = false;
			textBackground.enabled = false;
			creditsBackground.enabled = false;
			creditsText.enabled = false;
		}
	}

	public void StartGame()
	{
		Application.LoadLevel ("tuto01");
	}
	
	public void QuitGame()
	{
		Application.Quit ();
	}

	public void Credits()
	{
		credits = true;
		textBackground.enabled = true;
		creditsBackground.enabled = true;
		creditsText.enabled = true;
	}
}
