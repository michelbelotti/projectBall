//Camera antiga!!!

/*using UnityEngine;
using System.Collections;

public class CameraBehaviour : MonoBehaviour {

	private GameObject player;

	private Vector3 offset;
	[SerializeField] private float camSpeed = 3.0f;

	// Use this for initialization
	void Start () 
	{
		player = GameObject.FindGameObjectWithTag("Player");
		offset = transform.position - player.transform.position;
	}
	
	// Update is called once per frame
	void LateUpdate () 
	{
		transform.position = Vector3.Lerp (transform.position, player.transform.position + offset, camSpeed * Time.deltaTime);
	}
}*/

//Novo esquema de camera

using UnityEngine;
using System.Collections;

public class CameraBehaviour : MonoBehaviour 
{
	[SerializeField] private float cameraFollowSpeed = 3.0f;					//Speed wich the camera will follow the target position while following the player.
	[SerializeField] private float cameraDistance = 2.0f;						//Distance kept from the player.
	[SerializeField] private float cameraHeightOffset = 1.5f;					//Height offset based on player position.
	[SerializeField] private Vector3 offset = new Vector3 (0.0f, 1.5f, 0.0f);
	[SerializeField] float camSmoothDampTime = 0.1f;
	[SerializeField] private float cameraRotationSpeed = 3f;
	[SerializeField] private float cameraLookOffset = 2f;
	
	GameObject player;								//Player reference.
	Vector3 followTarget;							//Target to be followed.
	Vector3 lookDirection;							//Direction wich the camera is looking at. (Global)
	Vector3 velocityCamSmooth = Vector3.zero;		//Camera velocity smoothing.

	
	
	void Start()
	{
		player = GameObject.FindGameObjectWithTag ("Player");		//Get the player reference.
	}

	// Called every frame, after Update().
	void LateUpdate()
	{
		CameraFollow (cameraDistance, cameraHeightOffset);
	}
	
	// Function that handles how the Camera will follow the player, rotate and prevent wall clipping.
	void CameraFollow(float cameraDistance, float cameraHeightOffset)
	{
		Vector3 characterOffset = player.transform.position + offset;
		
		lookDirection = characterOffset - this.transform.position;
		lookDirection.y = 0.0f;
		lookDirection.Normalize ();
		followTarget = player.transform.position + transform.up * cameraHeightOffset - lookDirection * cameraDistance;
		CompensateForWalls (player.transform.position, ref followTarget);


		// The standard position of the camera is the relative position of the camera from the player.
		Vector3 standardPos = followTarget;
		
		// The abovePos is directly above the player at the same distance as the standard position.
		Vector3 abovePos = standardPos + Vector3.up * cameraHeightOffset * 2.1f;
		
		// An array of 5 points to check if the camera can see the player.
		Vector3[] checkPoints = new Vector3[9];
		
		// The first is the standard position of the camera.
		checkPoints[0] = standardPos;
		
		// The next three are 25%, 50% and 75% of the distance between the standard position and abovePos.
		checkPoints[1] = Vector3.Lerp(standardPos, abovePos, 0.25f);
		checkPoints[2] = Vector3.Lerp(standardPos, abovePos, 0.5f);
		checkPoints[3] = Vector3.Lerp(standardPos, abovePos, 0.75f);
		checkPoints[4] = Vector3.Lerp(standardPos, abovePos, 1f);
		checkPoints[5] = Vector3.Lerp(standardPos, abovePos, 1.25f);
		checkPoints[6] = Vector3.Lerp(standardPos, abovePos, 1.5f);
		checkPoints[7] = Vector3.Lerp(standardPos, abovePos, 1.75f);
		
		// The last is the abovePos.
		checkPoints[8] = abovePos;
		
		// Run through the check points...
		for(int i = 0; i < checkPoints.Length; i++)
		{
			// ... if the camera can see the player...
			if(ViewingPosCheck(checkPoints[i]))
				// ... break from the loop.
				break;
		}

		transform.position = Vector3.Lerp (transform.position, followTarget, cameraFollowSpeed * Time.deltaTime);
		SmoothPosition (transform.position, followTarget);
		CameraRotation();
		transform.LookAt (player.transform.position + Vector3.up * cameraLookOffset);
	}
		
	void SmoothPosition(Vector3 fromPosition, Vector3 toPosition)
	{
		transform.position = Vector3.SmoothDamp (fromPosition, toPosition, ref velocityCamSmooth, camSmoothDampTime);
	}

	void CameraRotation()
	{
		float h = Input.GetAxis ("RightStickH");

		float rotH = h * cameraRotationSpeed * Time.deltaTime;

		transform.RotateAround (player.transform.position, Vector3.up, rotH);
	}

	//Check if the camera is really targeting the player, and if it's not, adjust the position
	bool ViewingPosCheck (Vector3 checkPos)
	{
		RaycastHit hit;
		
		// If a raycast from the check position to the player hits something...
		if(Physics.Raycast(checkPos, player.transform.position - checkPos, out hit, cameraDistance))
			// ... if it is not the player...
			if(hit.transform.tag != "Player")
				// This position isn't appropriate.
				return false;
		
		// If we haven't hit anything or we've hit the player, this is an appropriate position.
		followTarget = checkPos;
		return true;
	}

	//Compensates the wall clipping.
	void CompensateForWalls(Vector3 fromObject, ref Vector3 toTarget)
	{
		RaycastHit wallHit = new RaycastHit();

		if (Physics.Linecast (fromObject, toTarget, out wallHit))
		{
			toTarget = new Vector3(wallHit.point.x, toTarget.y, wallHit.point.z);
		}
	}

}
