﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StateController : MonoBehaviour 
{
	public bool debugMode;					//toggle debug mode from inspector
	public string wordGemTag = "WordGem";	//this allows us to change the tag from the inspector
	private SearchingState search;			//this is a reference to the SearchingState script
	private bool inASearch;					//this tells us whether we are in an active search
	private List<Vector3> gemPositions;		//this will be the list of all gem positions
	private bool searchNewGem;				//this ensures that we only set a new goal when necessary
	private int gemCount;


	void Start()
	{
		search = GetComponent<SearchingState>();		//we make search hold a reference of the SearchState script
		inASearch = true;								//Set the searching state TRUE by default
		gemPositions = new List<Vector3>();				//create new list of Vector3 positions for the gems
		searchNewGem = true;							//set the searchNewGem to true upon start

		GameObject[] wordGems = GameObject.FindGameObjectsWithTag(wordGemTag);	//put all game objects with this tag in an array

		//now we need to get all the positions of the gem game objects and put in a list
		foreach(GameObject gem in wordGems)
		{
			Vector3 aGemPos = gem.transform.position;	//hold one gem position here temporarily
			if(debugMode){Debug.Log(aGemPos);}     	    //display each gem position in console
			gemPositions.Add(aGemPos);					//add the gem position to the list
			gemCount++;									//count the gems on the board
		}

	}

	void Update() 
	{

		//only set a new goal if searchNewGem is true
		if(searchNewGem)
		{
			Vector3 closestGemPos = FindClosestGem();	//hold closest gem position here
			Vector3 goalPos = new Vector3(closestGemPos.x, transform.position.y, closestGemPos.z); //constraing the y pos to this object y pos

			search.SetGoalPos(goalPos);		//set the goal by calling SetGoalPos method in SearchingState script
			searchNewGem = false;			//set searchNewGem to false until we find this gem						

			if(debugMode)
			{
				Debug.Log ("Closest Gem Pos: " + closestGemPos); //display value of closestGemPos in console
				Debug.Log ("Goal Pos: " + goalPos);				//display value of goalPos in console
			}

		}

		//keep searching until no longer in an active search
		if(inASearch)
		{
			Searching();
		}
		//if not in a search and there are still gems on the board, search for next gem
		if(!inASearch && gemCount > 0)
		{
			searchNewGem = true;
			inASearch = true;
		}
	}


	/**
	 * This method will give you the closest gem position to the NPC
	 * @return the closest gem position to this NPC
	 */
	private Vector3 FindClosestGem()
	{
		Vector3 myPos = transform.position;
		Vector3 closestGem = new Vector3();
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

		gemPositions.Remove(closestGem);  //remove this from the position list so NPC does not search for it on next round
			
		return closestGem;
	}



	/**
	 * This method for now just keeps moving to a goal using the search state MoveToGoal method 
	 */
	private void Searching()
	{
		if (!search.GoalReached())  	//We use the method GoalReached rather than the public boolean
		{	
			if(debugMode){Debug.Log("Goal Reached returns: " + search.GoalReached());} //See what this value is in debug mode

			search.MoveToGoal();		//because otherwise NPC will move to only one goal
		}
		else
		{
			inASearch = false;			//Once GoalReached() method returns true we can stop searching
			gemCount--;					//there are now one less gems on the board
		}
	}

}
