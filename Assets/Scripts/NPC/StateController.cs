﻿	using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StateController : MonoBehaviour 
{
	public bool debugMode;					//toggle debug mode from inspector
	public string wordGemTag = "WordGem";	//allows us to change the tag from the inspector
	public float playerDetectionWaitTime  = 1f;		//How often we check if plater is near 
	
	private bool inASearch;					//tells us whether we are in an active search
	private bool searchNewGem;				//ensures that we only set a new goal when necessary
	private bool chasing;					//to determine if we are chasing or not
	private bool fleeing;					//To determine if we are fleeing or not

	private int gemCount;					//keep track of number of gems
	private float playerDetectionTimer;		// To keep a timer for when we are to find player

	private SearchingState search;			//reference to the SearchingState script
	private SightController sight;			//reference that will allows us to use NPCs senses to detect player
	private GameObject player;				//reference to player game object
	private AttackingState attack;			//reference to AttackingState script
	private NPC1Health health;				//reference to NPC1Health script

	private List<Vector3> gemPositions;		//list of all gem positions
	private Vector3 closestGem;				//The position of the closest gem
	private Vector3 playerPos;				//The current position of the player
	private Vector3 escapePos;				//The point that is the furthest from player calculated using straight line distance
	private Vector3 goalPos;				//The current goal position
	
	void Awake()
	{	
		//SEARCHING INSTANTIATION CODE
		search = GetComponent<SearchingState>();		//we make search hold a reference of the SearchState script
		inASearch = false;								//Set the searching state false by default

		//GEMS INSTANTIATION CODE
		GameObject[] wordGems = GameObject.FindGameObjectsWithTag(wordGemTag);	//put all game objects with this tag in an array
		gemPositions = new List<Vector3>();				//create new list of Vector3 positions for the gems
		searchNewGem = true;							//set the searchNewGem to true upon start
		closestGem = new Vector3();
		foreach(GameObject gem in wordGems)				//now we need to get all the positions of the gem game objects and put in a list
		{
			Vector3 aGemPos = gem.transform.position;	//hold one gem position here temporarily
			if(debugMode){Debug.Log(aGemPos);}     	    //display each gem position in console
			gemPositions.Add(aGemPos);					//add the gem position to the list
			gemCount++;									//count the gems on the board
		}

		//SIGHT INSTANTIATION CODE
		sight = GetComponent<SightController>();

		//ATTACKING INSTANTIATION CODE
		attack = GetComponent<AttackingState>();
		player = GameObject.FindGameObjectWithTag("Player");

		//HEALTH INSTANTIATION CODE
		health = GetComponent<NPC1Health>();

		//CHASE INSTANTIATION CODE
		chasing = false;

		//PLAYER DETECTION INSTANTIATION CODE
		playerDetectionTimer = 3f;
		playerPos = new Vector3();

		//FLEEING INSTATIATION CODE
		escapePos = new Vector3();
		goalPos = new Vector3();
		fleeing = false;

	}

	void Update() 
	{
		playerDetectionTimer += Time.deltaTime;
																			//We could have the below check only occurwhen player is close enough to reduce operations
		if(playerDetectionTimer > playerDetectionWaitTime)					//Only do this check every chaseWaitTime seconds 
		{
			playerDetectionTimer = 0;					

			sight.CheckForPlayer();							//Have NPC check if player is seen

			if(sight.PlayerSeen())							//If the player is seen
			{
				search.Stop(true);							//Stop if currentely searching
				searchNewGem = false;						//Do not search for a gem any longer 
				playerPos = sight.GetLastPlayerSeenPos();	//Get the position of the player

				if(!attack.OutOfAmmo())						//We only want to chase if we are NOT out of ammo
				{
					chasing = true;							//We are chaing now

					goalPos = new Vector3(playerPos.x, transform.position.y, playerPos.z);
					search.SetGoalPos(goalPos); //New searching goal is the player's position
					inASearch = true;		//Allow to start searching!
				}
				else //If we NPC is out of ammo then run away from it
				{
					chasing = false;						//Make sure we are not chasing
					searchNewGem = false;					//Make sure we are not looking for Gems
					fleeing = true;							//We need to flee

					escapePos = search.GetFurthestPoint(playerPos);  //Get the furtherst point based on straight line distance form where the player is
					goalPos = new Vector3(escapePos.x, transform.position.y, escapePos.z);
					search.SetGoalPos(goalPos);					//The new search goal is as far as the NPC thinks it can get from player
					inASearch = true;								//We make sure to have it true for ins a search
				}
			}
			else   // I the player is not seen 
			{
				if(!attack.OutOfAmmo())		//If the player is NOT seen but we are out of ammo, is a good time to find new gems
				{
					searchNewGem = true;
				}
				chasing = false;			//But not a good time to chase (this check may not be needed)
			}
				
		}


		//only set a new goal if searchNewGem is true and if not already in the middle of search and if we not chasing the player
		if(!chasing && searchNewGem && health.GetHealth() < 4 &&!inASearch)
		{
			FindClosestGem();	//hold closest gem position here
			goalPos = new Vector3(closestGem.x, transform.position.y, closestGem.z); //constraing the y pos to this object y pos

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
				attack.AttackPlayer();  //If we are chasing the player we want to attack it to while chasing 
			}

			Searching();
		}
	}


	/**
	 * This method will give you the closest gem position to the NPC and set the Closest Gem variable
	 */
	private void FindClosestGem()
	{
		Vector3 myPos = transform.position;
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
	}

	/**
	 * This method for now just keeps moving to a goal using the search state MoveToGoal method 
	 */
	private void Searching()
	{
		if (search.GoalReached() == false)  			//We use the method GoalReached rather than the public boolean
		{	
			if(debugMode){Debug.Log("Goal Reached returns: " + search.GoalReached());} //See what this value is in debug mode
			
			search.MoveToGoal();				//because otherwise NPC will move to only one goal
		}
		else if (search.GoalReached() == true) 	
		{
			inASearch = false;			//Once GoalReached() method returns true we can stop searching

			if(!chasing && !fleeing && !searchNewGem)	//Make sure we were not just chasing or fleeing a		
			{
				if(goalPos.x == closestGem.x && goalPos.z == closestGem.z)
				{
					gemCount--;							//there are now one less gems on the board
					gemPositions.Remove(closestGem);  //remove this from the position list so NPC does not search for it on next round

					Debug.Log("A gem was removed!!");
				}
			}
			
			if(fleeing)	//Stop fleeing if thats what NPC was doing
			{
				fleeing = false;
			}

			if(gemCount > 1)		//If there are still Gems available
			{
				searchNewGem = true;		//set to true in case life is still too low
			}
		}	
	}
}