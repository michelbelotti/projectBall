using UnityEngine;
using System.Collections;

public class Teleporter : MonoBehaviour {

	[SerializeField] private int redCubeObjective = 1;
	[SerializeField] private int greenCubeObjective = 1;
	[SerializeField] private int blueCubeObjective = 1;
	[SerializeField] private string levelName = "level01";

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

	}

	void OnTriggerEnter (Collider other){

		if (other.gameObject.tag == "Player") {
			int redCubes = other.gameObject.GetComponent<PlayerCharacter>().redCubesCounter;
			int greenCubes = other.gameObject.GetComponent<PlayerCharacter>().greenCubesCounter;
			int blueCubes = other.gameObject.GetComponent<PlayerCharacter>().blueCubesCounter;

			if(redCubes >= redCubeObjective && greenCubes >= greenCubeObjective && blueCubes >= blueCubeObjective) {
				Application.LoadLevel(levelName);
			}
		}
	}
}
