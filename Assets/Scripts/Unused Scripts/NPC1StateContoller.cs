using UnityEngine;
using System.Collections;

public class NPC1StateContoller : MonoBehaviour 
{
	private SearchingState search;
	private RaycastHit hit;
	private bool inASearch;
	private GameObject player;
	private AttackingState attack;


	void Awake () 
	{
		search = GetComponent<SearchingState>();		//we make search hold a reference of the SearchState script
		inASearch = false;								//Set the searching state false by default
		attack = GetComponent<AttackingState>();
		player = GameObject.FindGameObjectWithTag("Player");

	}
	
	// Update is called once per frame
	void Update ()
	{
		if(Input.GetButtonDown("Fire1"))	//Input manager sets this as Mouse0 which is the primary mouse button
		{
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

			if(Physics.Raycast(ray, out hit, 100))
		    {
				if(hit.transform.tag == "floor")
				{
					Vector3 newTarget = new Vector3(hit.point.x, transform.position.y, hit.point.z);	//Set tartget to point where ray hits
					search.SetGoalPos(newTarget);		//Now we only set the goal for our search state

					inASearch = true;					//Now we say we are in active search state
				}
			}
		}

		if (inASearch)						//Keep searching (and moving) until goal is found as determined by Searching method
		{
			Searching();
		}

		//If the player gets too close attack and chase, otherwise just chase
		if(Vector3.Distance(transform.position, player.transform.position) < 3)
		{
			if(!attack.OutOfAmmo())
				attack.AttackPlayer();
		}
	}

	/**
	 * This method for now just keeps moving to a goal using the search state MoveToGoal method 
	 */
	private void Searching()
	{
		if (!search.GoalReached())		//We use the method GoalReached rather than the public boolean
			search.MoveToGoal();		//because otherwise NPC will move to only one goal
		else
			inASearch = false;			//Once GoalReached() method returns true we can stop searching 
	}
}
