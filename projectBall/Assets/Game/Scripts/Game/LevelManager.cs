using UnityEngine;
using System.Collections;

public class LevelManager : MonoBehaviour 
{
	bool levelCompleted;
	PlayerCharacter playerCharacter;
	GameObject hud;
	public GameObject[] redCubes;
	public GameObject[] greenCubes;
	public GameObject[] blueCubes;
	[SerializeField] private int levelToLoad;
		
	void Start()
	{
		levelCompleted = false;
		playerCharacter = GameObject.FindGameObjectWithTag ("Player").GetComponent<PlayerCharacter>();
		hud = Resources.Load ("HUD", typeof(GameObject)) as GameObject;
		redCubes = GameObject.FindGameObjectsWithTag ("RedCube");
		greenCubes = GameObject.FindGameObjectsWithTag ("GreenCube");
		blueCubes = GameObject.FindGameObjectsWithTag ("BlueCube");
	}

	void Update()
	{
		Quests ();
		if (levelCompleted)
			LoadLevel ();
	}

	void LoadLevel()
	{
		//Application.LoadLevel (levelToLoad);
	}

	void Quests()
	{
		if (playerCharacter.redCubesCounter >= redCubes.Length && playerCharacter.greenCubesCounter >= greenCubes.Length && playerCharacter.blueCubesCounter >= blueCubes.Length)
			levelCompleted = true;
	}
}
