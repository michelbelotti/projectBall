/*
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerController : MonoBehaviour {
	private Rigidbody rb;
	private int count;			//Passar para Level Manager
	private bool isGrounded;

	[SerializeField] private float speed = 10f;
	[SerializeField] private Text countText;		//Passar para Game Manager
	[SerializeField] private Text winText;			//Passar para Game Manager
	[SerializeField] private float jumpPower = 10f;
	[SerializeField] private float gravityMultiplier = 2.0f;




	void Start ()
	{
		rb = GetComponent<Rigidbody> ();
		count = 0;
		SetCountText ();
		winText.text = "";
	}

	void Update()
	{
		GroundCheck();
		print (isGrounded);
	}

	void FixedUpdate()
	{
		float moveHorizontal = Input.GetAxis ("Horizontal");
		float moveVertical = Input.GetAxis ("Vertical");
		bool jumpButton = Input.GetButtonDown ("Jump");

		Vector3 movement = new Vector3 (moveHorizontal, 0.0f, moveVertical);

		rb.AddForce (movement * speed);

		if (isGrounded)
		{
			Jump(jumpButton);
			jumpButton = false;
		}
		else
		{
			AdditionalGravity();
		}
	}

	void OnTriggerEnter (Collider other)
	{
		if (other.gameObject.tag == "PickUp") 
		{
			other.gameObject.SetActive (false);
			count = count +1;
			SetCountText();
		}
	}

	void SetCountText()
	{
		countText.text = "Cubos Coletados: " + count.ToString ();
		if (count >= 12)
		{
			winText.text = "You Win!!";
		}
	}
	
	void GroundCheck()
	{
		RaycastHit hit;

		if (Physics.Raycast(transform.position, Vector3.down, out hit, 0.7f))
		{
			isGrounded = true;
		}
		else
		{
			isGrounded = false;
		}
	}

	void Jump(bool jumpButton)
	{
		if (isGrounded && jumpButton)
		{
			rb.velocity = new Vector3 (rb.velocity.x, jumpPower, rb.velocity.z);
		}
	}

	void AdditionalGravity()
	{
		Vector3 addGravity = ((Physics.gravity * gravityMultiplier) - Physics.gravity);
		rb.AddForce(addGravity);
	}
}
*/

using UnityEngine;
using System.Collections;


[RequireComponent(typeof(PlayerCharacter))]

public class PlayerController : MonoBehaviour 
{
	private PlayerCharacter playerCharacter; 	//The Player Character reference.
	private Transform cam; 						//Camera reference.
	private Vector3 camForward;				 	//Camera's current forward direction.
	private Vector3 move; 						//The world relative movemente calculated from the camForward and user input.
	private float bounce;						//Variable to get the Bounce button.
	private bool jump;							//Variable to get the jump button.
	private bool dash;							//Variable to get the dash button.
	private bool bodySlam;						//Variable to gte the Body Slam button.
	private bool grab;							//Variable to get the Grab button.

	public bool pause;							//Variable to get the pause button.

	
	[SerializeField] private float movementSpeed = 3f;
	
	private void Start()
	{
		// Get the main camera transform if the "Main Camera" exists.
		if (Camera.main != null)
		{
			cam = Camera.main.transform;
		}
		else
		{
			Debug.LogWarning ("Warning: no main camera found. Third person character needs a Camera tagged \"MainCamera\", for camera-relative controls.");
		}
		
		//Get the main character reference.
		playerCharacter = GetComponent<PlayerCharacter>();
	}
	
	private void Update()
	{
		ReadInputs();
	}
	
	private void FixedUpdate()
	{
		//Read player's inputs
		float h = Input.GetAxis ("Horizontal");
		float v = Input.GetAxis ("Vertical");
		
		//Calculate the move direction to pass to the Character		
		if (cam != null)
		{
			//Calculate camera-relative movement.
			camForward = Vector3.Scale(cam.forward, new Vector3(1,0,1).normalized);
			
			move = v * camForward + h * cam.right;
		}
		else
		{
			//Use world-relative movement
			move = v * Vector3.forward + h * Vector3.right;
		}

		move = move.normalized * movementSpeed;		//Multiplies the move direction by the desired movement speed.

		/*
		if (playerCharacter.attracted) 
		{
			playerCharacter.WallMove (move, jump, dash, bodySlam, bounce);
		} 

		else 
		{
			playerCharacter.Move (move, jump, dash, bodySlam, bounce);
		}*/

		playerCharacter.Move (move, jump, dash, bodySlam, bounce);
		jump = false;
		dash = false;
		bodySlam = false;
	}

	private void ReadInputs()
	{
		//dash = Input.GetAxis("Dash"); //Read the dash input.
		bounce = Input.GetAxis("Bounce"); //Read the bounce input.

		//Read the pause button. (Flip-FLop Haha!)
		if (Input.GetButtonDown("Pause")) 
		{
			if (!pause)
				pause = true;
			else if (pause)
				pause = false;
		} 

		if (!jump) 
		{
			jump = Input.GetButtonDown("Jump"); //Read the jump input.
		}

		if (!bodySlam) 
		{
			bodySlam = Input.GetButtonDown("BodySlam"); //Read the Body Slam input.
		}

		if (!dash) 
		{
			dash = Input.GetButtonDown("Dash"); //Read the Body Slam input.
		}

		//Read the Bounce input and toggle between false and true.
		/*if (Input.GetButtonDown("Bounce"))
		{
			if (!bounce)
				bounce = true;
			else if (bounce)
				bounce = false;
		}*/
	}
	
}

