using UnityEngine;
using System.Collections;

public class aCameraController : MonoBehaviour
{
	
	public float turnSpeed = 180f;
	public float moveSpeed = 1f;
	
	aMaze maze;
	
	// Use this for initialization
	void Start ()
	{
		GameObject gMaze = GameObject.Find("Maze");
		
		if (gMaze)
			maze = gMaze.GetComponent<aMaze>();
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (maze && maze.Generating)
			return;
		
		if (Input.GetMouseButton(0))
		{
			transform.Rotate(0f, Input.GetAxis("Mouse X") * turnSpeed * Time.deltaTime, 0f, Space.World);
		}
		
		Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));
		move = transform.TransformDirection(move);
		move.y = 0f;
		move.Normalize();
		
		transform.Translate(move * moveSpeed * Time.deltaTime, Space.World);
	}
}
