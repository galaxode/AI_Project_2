using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour 
{
	public float turnSmoothing = 15f;
	public float speedDampTime = 0.1f;
	public float runningSpeed = 3.0f;

	private Animator anim;
	private HashIDs hash;


	void Awake()
	{
		anim = GetComponent<Animator>();
		hash = GameObject.FindGameObjectWithTag("GameController").GetComponent<HashIDs>();
	}

	void FixedUpdate()
	{
		float h = Input.GetAxis("Horizontal");
		float v = Input.GetAxis("Vertical");

		MovementManagement(h, v);

	}

	void MovementManagement(float horizontal, float vertical)
	{
		if (horizontal != 0f || vertical != 0f)
		{
			Rotating(horizontal, vertical);
			anim.SetFloat(hash.speedFloat, 5.5f, speedDampTime, Time.deltaTime);

			Vector3 currentPos = transform.position;
			Vector3 targetDirection = new Vector3(horizontal, 0f, vertical);
			Vector3 newPos = currentPos + targetDirection * runningSpeed * Time.deltaTime;
			rigidbody.MovePosition(newPos);
		}
		else
		{
			anim.SetFloat(hash.speedFloat, 0);
		}
	}

	void Rotating(float horizontal, float vertical)
	{
		Vector3 targetDirection = new Vector3(horizontal, 0f, vertical);
		Quaternion targetRotation = Quaternion.LookRotation(targetDirection, Vector3.up);
		Quaternion newRotation = Quaternion.Lerp(rigidbody.rotation, targetRotation, turnSmoothing * Time.deltaTime);
		rigidbody.MoveRotation(newRotation);

	}
}
