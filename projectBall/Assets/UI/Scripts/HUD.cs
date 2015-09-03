using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HUD : MonoBehaviour 
{
	PlayerCharacter playerCharacter;
	PlayerController playerController;
	LevelManager levelManager;

	public Image			dashUnit0;
	public Image			dashUnit1;
	public Slider			timeSlider;
	public Text				dashText;
	public Text				winText;
	public Text				redCubesText;
	public Text				greenCubesText;
	public Text				blueCubesText;


	public Button			pauseButton;
	public Button			quitButton;
	public Image			pauseBackground;

	public int				redCubesObjective = 0;
	public int				greenCubesObjective = 0;
	public int				blueCubesObjective = 0;

	//GameObject				hudEventSystem;
	GameObject				gameManager;
	
	float time = 0f;

	void Awake()
	{
		//DontDestroyOnLoad (this);
		//DontDestroyOnLoad (hudEventSystem);

		//hudEventSystem = Resources.Load ("HUDEventSystem", typeof(GameObject)) as GameObject;
	}

	void Start()
	{
		playerCharacter = GameObject.FindWithTag("Player").GetComponent<PlayerCharacter>();
		playerController = GameObject.FindGameObjectWithTag ("Player").GetComponent<PlayerController> ();
		gameManager = GameObject.FindGameObjectWithTag ("GameManager");
		//Instantiate (hudEventSystem, new Vector3 (0f, 0f, 0f), Quaternion.identity);
		levelManager = GameObject.FindGameObjectWithTag ("LevelManager").GetComponent<LevelManager>();
	}

	void Update()
	{
		DashGraphic();
		Cubes ();
		Pause (playerController.pause);
	}


	private bool dashBarEnable = false;
	void DashGraphic()
	{
		/*if (playerCharacter.dashCounter > 0)
			dashUnit1.enabled = false;
		else
			dashUnit1.enabled = true;
		
		if (playerCharacter.dashCounter > 1)
			dashUnit0.enabled = false;
		else
			dashUnit0.enabled = true;*/

		if (playerCharacter.dashCounter == 0) {
			dashBarEnable = true;
		}

		if (playerCharacter.dashCounter > 0 && dashBarEnable == true) {
			//time += Time.deltaTime * 0.3334f;
			time += Time.deltaTime;
			if (time < 1.5f)
				timeSlider.value = Mathf.Lerp (timeSlider.minValue, timeSlider.maxValue, time);
			else if (time > 1.5f){
				time = 0f;
				dashBarEnable = false;
			}

		}
	}

	void Pause(bool pause)
	{
		if (Application.loadedLevelName != "MainMenu")
		{
			if (pause) 
			{
				pauseButton.enabled = true;
				pauseButton.image.enabled = true;
				pauseButton.GetComponentInChildren<Text>().enabled = true;
				quitButton.enabled = true;
				quitButton.image.enabled = true;
				quitButton.GetComponentInChildren<Text>().enabled = true;
				pauseBackground.enabled = true;
				Time.timeScale = 0f;
			}
			else
			{
				Time.timeScale = 1f;
				pauseButton.enabled = false;
				pauseButton.image.enabled = false;
				pauseButton.GetComponentInChildren<Text>().enabled = false;
				quitButton.enabled = false;
				quitButton.image.enabled = false;
				quitButton.GetComponentInChildren<Text>().enabled = false;
				pauseBackground.enabled = false;
			}
		}
	}

	void Cubes()
	{
		//redCubesText.text = (playerCharacter.redCubesCounter + " / " + levelManager.redCubes.Length);
		//greenCubesText.text = (playerCharacter.greenCubesCounter + " / " + levelManager.greenCubes.Length);
		//blueCubesText.text = (playerCharacter.blueCubesCounter + " / " + levelManager.blueCubes.Length);
		redCubesText.text = (playerCharacter.redCubesCounter + " / " + redCubesObjective);
		greenCubesText.text = (playerCharacter.greenCubesCounter + " / " + greenCubesObjective);
		blueCubesText.text = (playerCharacter.blueCubesCounter + " / " + blueCubesObjective);
	}

	public void PauseButtonTest()
	{
		playerController.pause = false;
	}
	
	public void QuitButtonTest()
	{
		Application.LoadLevel ("MainMenu");
		Destroy (gameManager);
		Destroy (this);
	}
}
