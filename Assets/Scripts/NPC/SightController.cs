using UnityEngine;
using System.Collections;

public class SightController : MonoBehaviour 
{
	public float fieldOfViewAngle = 110f;	//The filed of view of the NPC
	public bool playerInSight;				//A boolean to mark if player is being seen or not
	public float sightDistance = 4f;		//How far away can player see and/or detect player

	private GameObject player;				//Holds a reference to the player 
	private Vector3 lastPlayerSeenPos; 		//Holds the last postion where player was seen 

	void Awake()
	{
		playerInSight = false;
		player = GameObject.FindGameObjectWithTag("Player");
	}
	
	/**
	 * This method allows the NPC to "sense" that the player is around
	 */
	public void CheckForPlayer()
	{
		playerInSight = false;

		//Check if the NPC is within detectable distance at all
		if(Vector3.Distance(transform.position, player.transform.position) < sightDistance)
		{
			Vector3 direction = player.transform.position - transform.position;	//Calculate direction where player is
			float angle = Vector3.Angle(direction, transform.forward);			//Get the angle from forward dir to dir where player is

			if(angle < fieldOfViewAngle * 0.5f)		//If the angle is less than half the NPC's field of view (because above we used forward for angle)
			{
				RaycastHit hit;

				Debug.DrawRay(transform.position + transform.up, direction, Color.blue, 2f);	//For debuging purposes draw ray below
				if(Physics.Raycast(transform.position, direction.normalized, out hit, sightDistance)) //Cast a ray in the direction of the player
				{
					if(hit.collider.gameObject.tag == "Player")				//Make sure what we hit with the ray is the player
					{
						Debug.Log("I see the player!");					
						playerInSight = true;								//Mark playerInSight as true
						
						lastPlayerSeenPos = hit.collider.gameObject.transform.position;  //Record the position where player was scene
					}

				}			
			}
		}
	}

	/**
	 * This method allows other classes to query this script to check if the player is see
	 */
	public bool PlayerSeen()
	{
		if(playerInSight)
			return true;
		else
			return false;
	}

	/**
	 * This method allows other classes to ask where player was last seen
	 * @Return a vector3 where player was last seen. 
	 */
	public Vector3 GetLastPlayerSeenPos()
	{
		return lastPlayerSeenPos;
	}

}
