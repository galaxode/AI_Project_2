using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StateController : MonoBehaviour 
{
	public bool debugMode;					//toggle debug mode from inspector

	public string wordGemTag = "WordGem";	//allows us to change the tag from the inspector
	public float detectDistance; 

	private SearchingState search;			//reference to the SearchingState script
	private bool inASearch;					//tells us whether we are in an active search
	private List<Vector3> gemPositions;		//list of all gem positions
	private bool searchNewGem;				//ensures that we only set a new goal when necessary
	private bool chasing;
	private int gemCount;					//keep track of number of gems
	private ChasingState chase;

	private SightController sight;			//reference that will allows us to use NPCs senses to detect player

	private GameObject player;				//reference to player game object
	private AttackingState attack;			//reference to AttackingState script


	private NPC1Health health;				//reference to NPC1Health script

	private float chaseTimer;
	private float chaseWaitTime  = 1f;

	private bool moving = false;
	private bool gotGem = false;

	Vector3 closestGem = new Vector3();

	private Vector3 playerPos;
	private Vector3 escapePos;
	private Vector3 goalPos;

	public float rotationRate = 180.0f;// degrees

	void Awake()
	{	
		//GEM SEARCHING INSTANTIATION CODE
		search = GetComponent<SearchingState>();		//we make search hold a reference of the SearchState script
		inASearch = false;								//Set the searching state false by default
		gemPositions = new List<Vector3>();				//create new list of Vector3 positions for the gems
		searchNewGem = true;							//set the searchNewGem to true upon start
		sight = GetComponent<SightController>();

		chaseTimer = 3f;

		GameObject[] wordGems = GameObject.FindGameObjectsWithTag(wordGemTag);	//put all game objects with this tag in an array

		//now we need to get all the positions of the gem game objects and put in a list
		foreach(GameObject gem in wordGems)
		{
			Vector3 aGemPos = gem.transform.position;	//hold one gem position here temporarily
			if(debugMode){Debug.Log(aGemPos);}     	    //display each gem position in console
			gemPositions.Add(aGemPos);					//add the gem position to the list
			gemCount++;									//count the gems on the board
		}

		//ATTACKING INSTANTIATION CODE
		attack = GetComponent<AttackingState>();
		player = GameObject.FindGameObjectWithTag("Player");

		//HEALTH INSTANTIATION CODE
		health = GetComponent<NPC1Health>();

		//CHASE INSTANTIATION CODE
		chase = GetComponent<ChasingState>();
		chasing = false;

		playerPos = new Vector3();
		escapePos = new Vector3();
		goalPos = new Vector3();

	}

	void Update() 
	{

		chaseTimer += Time.deltaTime;


		if(chaseTimer > chaseWaitTime)
		{
			chaseTimer = 0;

			//If the player gets too close attack and chase, otherwise just chase
			if(sight.PlayerSeen())
			{
				//search.stop = true;
				searchNewGem = false;

				Debug.Log("I SEE THE PLAYER MAN!");

				playerPos = sight.GetLastPlayerSeenPos();

				if(!attack.OutOfAmmo())
				{
//					playerPos = sight.GetLastPlayerSeenPos();

					chasing = true;
					searchNewGem = false;

					//TurnToTarget();

					search.SetGoalPos(new Vector3(playerPos.x, transform.position.y, playerPos.z));
					inASearch = true;
//					attack.AttackPlayer();
					//chase.ChasePlayer();
				}
				else
				{
					chasing = false;
					searchNewGem = false;
					inASearch = true;

					escapePos = search.GetFurthestPoint(playerPos);
					search.SetGoalPos(escapePos);


				}

			}
			else
			{
				if(!attack.OutOfAmmo())
				{
					//inASearch = false;
					searchNewGem = true;
				}

				chasing = false;
			}
				
		}


		//only set a new goal if searchNewGem is true
		if(!chasing && searchNewGem && health.GetHealth() < 4 &&!inASearch)
		{
			FindClosestGem();	//hold closest gem position here
			Vector3 goalPos = new Vector3(closestGem.x, transform.position.y, closestGem.z); //constraing the y pos to this object y pos

			search.SetGoalPos(goalPos);		//set the goal by calling SetGoalPos method in SearchingState script
			inASearch = true;
			searchNewGem = false;			//set searchNewGem to false until we find this gem						

			if(debugMode)
			{
				Debug.Log ("Closest Gem Pos: " + closestGem); //display value of closestGemPos in console
				Debug.Log ("Goal Pos: " + goalPos);				//display value of goalPos in console
			}

		}

		//keep searching until no longer in an active search
		if(inASearch)
		{
			if(chasing)
			{
				TurnToTarget(playerPos);
				attack.AttackPlayer();
			}

			Searching();
		}
	}


	/**
	 * This method will give you the closest gem position to the NPC
	 * @return the closest gem position to this NPC
	 */
	private void FindClosestGem()
	{
		Vector3 myPos = transform.position;
//		Vector3 closestGem = new Vector3();
		float smallestDistance = 0.0f;
		bool first = true;

		foreach(Vector3 pos in gemPositions)
		{
			float aDistance = Vector3.Distance(pos, myPos);

			if(first)									
			{
				smallestDistance = aDistance;	//since this is first position set smallestDistance to first calculation
				closestGem = pos;				//since this is first position set closestGem to corresponding position
				first = false;					//set to false so we do this only once per method call
			}
			else if(aDistance < smallestDistance)
			{
				smallestDistance = aDistance;	//set a new smallest distance if one exists
				closestGem = pos;				//set closestGem that corresponds to that distance
			}
		}

		//gemPositions.Remove(closestGem);  //remove this from the position list so NPC does not search for it on next round
			
		//return closestGem;
	}

	private void TurnToTarget(Vector3 targetPos)
	{
		Vector3 toTarget = transform.position - targetPos;
		//toTarget.z = 0.0f;
		
		// get angle between my facing and vector to target
		float angleDiff = Vector3.Angle(toTarget, transform.up);
		
		if (angleDiff < 2.0f) // degrees
		{
			Debug.Log("No need to turn");
		}
		else // turn
		{
//			if (toTarget != Vector3.zero)
//			{
				// calculates the angle we should turn towards, - 90 makes the sprite rotate
				
				//float targetAngle = (Mathf.Atan2(toTarget.y, toTarget.x) * Mathf.Rad2Deg) - 90.0f;
				
				// actually rotates the sprite using Slerp (from its previous rotation, to the new one at the designated speed.
				
//				transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(0, 0, targetAngle), rotationRate * Time.deltaTime);


				// Rotate towards target    
		toTarget.y = 0f;
				//var targetPoint = target.position;
				Quaternion targetRotation = Quaternion.LookRotation (toTarget);
				
				

//				targetRotation.x = 0.0f;
//				targetRotation.z = 0.0f;
				
				//transform.rotation = Quaternion.Slerp(transform.rotation, newRotation, Time.deltaTime * 8);

				//transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 2.0);
				//transform.rotation = targetRotation;

			transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation (toTarget), Time.deltaTime * 8);
//			}
		}
	}


	/**
	 * This method for now just keeps moving to a goal using the search state MoveToGoal method 
	 */
	private void Searching()
	{
		if (search.GoalReached() == false)  	//We use the method GoalReached rather than the public boolean
		{	
			if(debugMode){Debug.Log("Goal Reached returns: " + search.GoalReached());} //See what this value is in debug mode
			
			search.MoveToGoal();		//because otherwise NPC will move to only one goal

			//searchNewGem = false;
		}
		else if (search.GoalReached() == true) 
		{
			Debug.Log("I got here!!!!!!!!!!!!!!!!!!!!");


			if(!chasing && !searchNewGem)
			{
				gemCount--;					//there are now one less gems on the board
				gemPositions.Remove(closestGem);  //remove this from the position list so NPC does not search for it on next round

				Debug.Log("BYE BYE BYE BYEEEEEEEEEEEEEEEEEEEEEE MYYYYYYYYY GEMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMM");
			}

			search.stop = true;
			
			inASearch = false;			//Once GoalReached() method returns true we can stop searching
			searchNewGem = true;		//set to true in case life is still too low

		}
	}
	
}


