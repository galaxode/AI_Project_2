using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GemTracker : MonoBehaviour {

	public string wordGemTag = "WordGem";	//allows us to change the tag from the inspector
	private int amountOfGems;
	GameObject[] wordGems;
	private List<Vector3> gemPositions;
	

	void Awake () 
	{
		//GEMS INSTANTIATION CODE
		wordGems = GameObject.FindGameObjectsWithTag(wordGemTag);	//put all game objects with this tag in an array
		amountOfGems = wordGems.Length;
		gemPositions = new List<Vector3>();

		foreach(GameObject gem in wordGems)				//now we need to get all the positions of the gem game objects and put in a list
		{
			Vector3 aGemPos = gem.transform.position;	//hold one gem position here temporarily
			gemPositions.Add(aGemPos);					//add the gem position to the list
		}
	}

	/**
	 * This method repeats the operations in the awake function so that we can get first gett and updated list of all gem positions
	 * @param startPos the point from where we measure distance (typically the NPC position)
	 * @return the closest available gem
	 */
	public Vector3 FindClosestGem(Vector3 startPos)
	{
		GameObject[] newWordCountArray;       //Use a new array to get a new list of all gams 
		gemPositions = new List<Vector3>();	  //Update the gamPositions reference to start it new

		newWordCountArray = GameObject.FindGameObjectsWithTag(wordGemTag);	//put all game objects with this tag in an array

		foreach(GameObject gem in newWordCountArray)				//now we need to get all the positions of the gem game objects and put in a list
		{
			Vector3 aGemPos = gem.transform.position;	//hold one gem position here temporarily
			gemPositions.Add(aGemPos);					//add the gem position to the list
		}


		Vector3 closestGem = new Vector3();       
		float smallestDistance = 0.0f;			//Only for the firstime we loop
		bool first = true;
		
		foreach(Vector3 pos in gemPositions)
		{
			float aDistance = Vector3.Distance(pos, startPos);
			
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

		return closestGem;
	}

	/**
	 * This method is to be used when the amount of gems do not increase (or spawn) and there is a set amount of gems for every level
	 * This will not work if we want to update gems and we would need to scan for gameObjects with approiate tag again
	 * It Removes a "gem" from our count
	 */
	public void RemoveGem()
	{
		if(amountOfGems > 0)
			amountOfGems--;
		else
			Debug.Log("Nothing to remove");
	}

	/**
	 * If we implement a a system to spawn new gems this would add them to our counter if needed
	 */
	public void AddGem()
	{
		amountOfGems++;
	}


	/**
	 * Again, will need to be recoded if we choose to spawn new gems while in game
	 * @return the current amount of gems
	 */ 
	public int GetAmountOfGems()
	{
		return amountOfGems;
	}
}
