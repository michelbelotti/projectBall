using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Playtime : MonoBehaviour {

	public static int playtime = 0;
	private static int seconds = 0;
	private static int minutes = 0;
	private static int hours = 0;

	Text text;

	void Start () {
		text = GetComponent<Text> ();
		StartCoroutine("Playtimer");
		if (Application.loadedLevelName == "ending") {
			text.text = ("You did it!! And spend,\n" + hours.ToString () + ":" + minutes.ToString () + ":" + seconds.ToString () + "\n Congratulations and thanks for Playing!");
		}
		if (Application.loadedLevelName == "tuto01") {
			playtime = 0;
			seconds = 0;
			minutes = 0;
			hours = 0;
		}
	}

	private IEnumerator Playtimer(){
		while (true) {
			yield return new WaitForSeconds(1);
			playtime += 1;
			seconds = playtime % 60;
			minutes = (playtime / 60) % 60;
			hours = playtime / 3600;
		}
	}

	void Update (){
		if (Application.loadedLevelName != "ending") {
			text.text = ("Time: " + hours.ToString () + ":" + minutes.ToString () + ":" + seconds.ToString ());
		}
	}
}
