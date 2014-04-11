using UnityEngine;
using System.Collections;

public class SightController : MonoBehaviour 
{
	public float fieldOfViewAngle = 110f;
	public bool playerInSight;
	public float sightDistance = 4f;
	private GameObject player;

	//private Transform npcCurrentPos;
	//private GameObject theNPC;
	private SphereCollider sightCol;
	private Vector3 lastPlayerSeenPos; 
	// private Vector3 currPlayerPos;

	void Awake()
	{
		//npcCurrentPos = transform.parent.GetComponent(
		//theNPC = transform.parent.gameObject;
		//currPlayerPos = new Vector3(1000f, 1000f, 1000f);
		playerInSight = false;
		player = GameObject.FindGameObjectWithTag("Player");

	}

//	void Update()
//	{
//		if(lastPlayerSeenPos != currPlayerPos)
//			currPlayerPos = lastPlayerSeenPos;
//	
//	}

	private void CheckForPlayer()
	{
		playerInSight = false;

		if(Vector3.Distance(transform.position, player.transform.position) < sightDistance)
		{
			Debug.Log("I see the dude!");

			Vector3 direction = player.transform.position - transform.position;
			float angle = Vector3.Angle(direction, transform.forward);

			if(angle < fieldOfViewAngle * 0.5f)
			{
				RaycastHit hit;
				

				Debug.DrawRay(transform.position + transform.up, direction, Color.blue, 2f);
				if(Physics.Raycast(transform.position, direction.normalized, out hit, sightDistance)) 
				{
					if(hit.collider.gameObject.tag == "Player")
					{
							Debug.Log("I see the player!");
						
						playerInSight = true;
						
						lastPlayerSeenPos = hit.collider.gameObject.transform.position;

					}

				}			
			}
		}
	}

//	void OnTriggerStay(Collider other)
//	{
//		if(other.gameObject.tag == "Player")
//		{
//		//	playerInSight = false;
//
//			//Debug.Log("The player entered my field!");
//
//			Vector3 direction = other.transform.position - theNPC.transform.position;
//			float angle = Vector3.Angle(direction, transform.forward);
//
//			if(angle < fieldOfViewAngle * 0.5f)
//			{
//				RaycastHit hit;
//
//				if(Physics.Raycast(theNPC.transform.position, direction.normalized, out hit, sightCol.radius)) 
//				{
//					if(hit.collider.gameObject.tag == "Player")
//					{
//					//	Debug.Log("I see the player!");
//
//						playerInSight = true;
//
//						lastPlayerSeenPos = hit.collider.gameObject.transform.position;
//
//						Debug.Log("%%%%%%%% AND THE POS: " + lastPlayerSeenPos);
//					}
//				}			
//			}
//		}
//	}



	public bool PlayerSeen()
	{
		CheckForPlayer();

		if(playerInSight)
			return true;
		else
			return false;
	}

	public Vector3 GetLastPlayerSeenPos()
	{
		return lastPlayerSeenPos;
	}

}
