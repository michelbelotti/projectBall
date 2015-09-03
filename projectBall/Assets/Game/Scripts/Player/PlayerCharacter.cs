using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]

public class PlayerCharacter : MonoBehaviour 
{
	private Rigidbody 				rb;
	private bool 					isGrounded;
	private bool					bodySlamTrigger;
	private SphereCollider			sphereCollider;
	private Color 					redColor;
	private Color 					blueColor;
	private Color					greenColor;
	private Color					defaultColor;
	private Color					newColor;
	private int						jumpCounter = 0;

	public int 						blueCubesCounter = 0;
	public int 						redCubesCounter = 0;
	public int 						greenCubesCounter = 0;
	public int 						dashCounter = 0;

	ParticleSystem					particles;
	AudioSource						jumpAudio;



	public bool 					isInMagneticField;
	public bool						attracted;

	[SerializeField] private float deathHeight = -20f;
	[SerializeField] private int dashLimit = 2;
	[SerializeField] private int jumpLimit = 2;
	[SerializeField] private float jumpPower = 12f;	//The player jump power.
	[SerializeField] private float dashPower = 5f;	//The player dash power.
	[SerializeField] private float dashCooldown = 1.5f;	//The player jump power.
	[SerializeField] private float bodySlamPower = 15f;//The player body slam power.
	[SerializeField] private PhysicMaterial	rubberMaterial;	//Ball physic material.
	[SerializeField] private PhysicMaterial	ironMaterial;	//Ball physic material.

	[Range(1f, 4f)][SerializeField] float gravityMultiplier = 2.5f;		//How much the gravity will be multiplied while the player is on air.
	
	
	
	void Start()
	{
		rb = this.GetComponent<Rigidbody> ();							//Get the rigidbody reference.
		sphereCollider = this.GetComponent<SphereCollider>();			//Get the ball's collider reference.
		defaultColor = Color.black;
		redColor = defaultColor;
		blueColor = defaultColor;
		greenColor = defaultColor;
		newColor = defaultColor;
		isGrounded = true;
		bodySlamTrigger = false;
		dashCounter = 0;
		jumpCounter = 0;
		particles = GetComponent<ParticleSystem> ();
		particles.startColor = defaultColor;
		jumpAudio = GetComponent<AudioSource> ();
	}	

	void Update()
	{
		SmoothColorShift();
		CheckPlayerDeath();
	}

	void CheckPlayerDeath (){
		if (transform.position.y < deathHeight) {
			Application.LoadLevel(Application.loadedLevelName);
		}
	}
	
	// Function that handles the locomotion of the player (ball).
	public void Move(Vector3 move, bool jump, bool dash, bool bodySlam, float bounce)
	{
		//Locomotion
		rb.useGravity = true;
		rb.AddForce (move);
		IsGrounded ();


		if (!isGrounded) { //What the player can do while is on the air
			HandleAirborneMovement ();
			BodySlam (bodySlam);
			
		} else { //What the player can do while is on the ground
			if (jumpCounter > 0) {
				jumpCounter = 0;
			}
			if (bodySlamTrigger == true){
				bodySlamTrigger = false;
			}
		}

		//What the player can do in any condition
		Bounce (bounce);
		Dash (dash, move);
		Jump(jump);
	}

	//Function that handles the physic material change.
	void Bounce(float bounce)
	{

		if (bounce > 0.2f) {
			sphereCollider.material = rubberMaterial;
		}
		else 
		{
			sphereCollider.material = ironMaterial;
		}
	}

	//Function that handles the dash cooldown
	void DashCooldown ()
	{
		dashCounter = 0;
	}

	// Function that handles the dash ability.
	void Dash(bool dashInput, Vector3 move)
	{

		if (dashInput == true && dashCounter < dashLimit) 
		{
			float currentPower = Mathf.Abs(rb.velocity.z) + Mathf.Abs(rb.velocity.x);
			if(Mathf.Abs(move.normalized.x) > 0 || Mathf.Abs(move.normalized.z) > 0){
				rb.velocity = move.normalized * (dashPower + currentPower);
				if (dashCounter <= 0)
				{
					Invoke("DashCooldown", dashCooldown);
				}
				dashCounter++;
			}
		} 
	}

	// Function that handles the Body Slam ability.
	void BodySlam(bool bodySlamInput )
	{
		if (bodySlamInput == true && bodySlamTrigger == false) {
			rb.velocity = new Vector3 (rb.velocity.x, -bodySlamPower, rb.velocity.z);
			bodySlamTrigger = true;
		}
	}

	// Function that handles the jump ability.
	void Jump( bool jumpInput )
	{
		if(jumpInput == true && jumpCounter < jumpLimit)
		{
			rb.velocity = new Vector3(rb.velocity.x * 0.7f, jumpPower, rb.velocity.z * 0.7f);
			jumpAudio.Play();
			jumpCounter++;
		}
	}

	// Function that handles the player behaviour while in the air.
	void HandleAirborneMovement()
	{
		//Apply extra gravity force while in the air.
		Vector3 extraGravityForce = (Physics.gravity * gravityMultiplier) - Physics.gravity;
		rb.AddForce (extraGravityForce);
	}

	// Function that check if the player is on the ground.
	void IsGrounded()
	{
		RaycastHit hit;
		
		//if(Physics.Raycast(transform.position, -this.transform.up, out hit, 0.7f))
		if(Physics.Raycast(transform.position, -Vector3.up, out hit, 0.7f))
		{
			isGrounded = true;
		}

		else
		{
			isGrounded = false;
		}
	}

	// Function that handles collision (trigger) events, like collectiong pick-ups.
	void OnTriggerEnter (Collider other)
	{
		if (other.gameObject.tag == "RedCube") 
		{
			other.gameObject.SetActive (false);

			Color otherColor = other.gameObject.GetComponent<Renderer>().material.color;
			redColor += otherColor * 0.1f;

			redCubesCounter++;

			particles.Play();
		}

		if (other.gameObject.tag == "GreenCube") 
		{
			other.gameObject.SetActive (false);
			
			Color otherColor = other.gameObject.GetComponent<Renderer>().material.color;
			greenColor += otherColor * 0.1f;

			greenCubesCounter++;

			particles.Play();
		}

		if (other.gameObject.tag == "BlueCube") 
		{
			other.gameObject.SetActive (false);
			
			Color otherColor = other.gameObject.GetComponent<Renderer>().material.color;
			blueColor += otherColor * 0.1f;

			blueCubesCounter++;

			particles.Play();
		}
	}

	//Function that changes the material and light color depending on the cubes colleted
	void SmoothColorShift()
	{
		newColor = (redColor + greenColor + blueColor);
		this.GetComponent<Renderer>().material.color = Color.Lerp (this.GetComponent<Renderer>().material.color, newColor, 0.6f * Time.deltaTime);
		this.GetComponent<Renderer>().material.SetColor("_EmissionColor", Color.Lerp (this.GetComponent<Renderer>().material.color, newColor, 0.6f * Time.deltaTime));
		this.GetComponent<Light>().color = Color.Lerp (this.GetComponent<Light>().color, newColor, 0.6f * Time.deltaTime);
		particles.startColor = Color.Lerp (particles.startColor, newColor, 0.6f * Time.deltaTime);

		redColor = Color.Lerp (redColor, defaultColor, 0.1f * Time.deltaTime);
		greenColor = Color.Lerp (greenColor, defaultColor, 0.1f * Time.deltaTime);
		blueColor = Color.Lerp (blueColor, defaultColor, 0.1f * Time.deltaTime);
	}

	void OnCollisionEnter(Collision other)
	{
		if (other.collider.gameObject.CompareTag ("MagneticWall")) 
		{
			ContactPoint contactPoint = other.contacts[0];
			Vector3 direction = transform.position - contactPoint.point;
			this.transform.up = direction.normalized;
			attracted = true;
		}
	}

	void OnCollisionExit(Collision other)
	{
		if (other.collider.gameObject.CompareTag ("MagneticWall")) 
		{
			this.transform.up = Vector3.up;
			attracted = false;
		}
	}

	void MagneticGravity(bool isInMagneticField, bool attracted)
	{
		if(isInMagneticField && attracted)
			rb.AddForce (Vector3.Scale (this.transform.up, ((Physics.gravity * gravityMultiplier) - Physics.gravity)));
	}


}
