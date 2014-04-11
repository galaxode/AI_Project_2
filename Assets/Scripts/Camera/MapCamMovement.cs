using UnityEngine;
using System.Collections;

public class MapCamMovement : MonoBehaviour {

	private Transform player;
	
	void Awake()
	{
		player = GameObject.FindGameObjectWithTag("Player").transform;
	}
	
	void FixedUpdate()
	{
		Vector3 newPos = new Vector3(player.position.x, transform.position.y, player.position.z);

		transform.position = Vector3.Lerp(transform.position, newPos, 1f * Time.deltaTime);
	}


}
