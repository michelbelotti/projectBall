using UnityEngine;
using System.Collections;

public class PlatformTransform : MonoBehaviour {

	[SerializeField] private bool rotation = false;
	[SerializeField] private bool translate = false;
	[SerializeField] private int xRotSpeed = 0;
	[SerializeField] private int yRotSpeed = 0;
	[SerializeField] private int zRotSpeed = 0;

	[SerializeField] private float translateSpeed = 10f;
	[SerializeField] private float distance = 10f;

	[SerializeField] private bool localDirection = true;
	[Range(0, 1)][SerializeField] private int xDirection = 1;
	[Range(0, 1)][SerializeField] private int yDirection = 0;
	[Range(0, 1)][SerializeField] private int zDirection = 0;

	private float currentDistance = 0f;
	private bool forward = true;

	// Update is called once per frame
	void Update () 
	{

		if (rotation == true) {
			transform.Rotate (new Vector3 (xRotSpeed, yRotSpeed, zRotSpeed) * Time.deltaTime);
		}
		if ( translate == true) {

			if (forward == true){
				if (localDirection == true){
					transform.Translate (new Vector3 (xDirection, yDirection, zDirection) * translateSpeed * Time.deltaTime);
				} else {
					transform.Translate (new Vector3 (xDirection, yDirection, zDirection) * translateSpeed * Time.deltaTime, Space.World);
				}

			} else {
				if (localDirection == true){
					transform.Translate (-(new Vector3 (xDirection, yDirection, zDirection)) * translateSpeed * Time.deltaTime);
				} else {
					transform.Translate (-(new Vector3 (xDirection, yDirection, zDirection)) * translateSpeed * Time.deltaTime, Space.World);
				}
			}
			currentDistance = currentDistance + (translateSpeed * Time.deltaTime);

			if (currentDistance >= distance){

				if (forward == true){
					forward = false;
				} else {
					forward = true;
				}
				currentDistance = 0;
			}
		}
	}
}

