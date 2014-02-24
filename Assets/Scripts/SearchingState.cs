using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SearchingState : MonoBehaviour {


	public bool findPathDynamically;		//Allows us to select if we want the NPC to find path dynamically in the inspector
	public bool debugMode;						
	public bool optimizePathfinding;		//Switches optimization to be consitent accross cpu speeds
	public int pathRescanRate;				//Allows us to change the speed at which the path is re-scanned in the editor. We could implement this to make it nicer: http://docs.unity3d.com/Documentation/Components/editor-CustomEditors.html

	public float speedMultiplier;			//Allows us to change the value at which speed is multiplied in the inspector

	private List<Vector3> path; 
	private Vector3 goalPos;				//The goal position as determined by other game classes
	private Vector3 nextNodePos;			//The next node in the path that the NPC will follow
	private int nextNodeIndex;

	private bool onNode; 

	private PathFinderController aPathFinder;

	
	/**
	 * Get the script from whatever compononet we choose to assign the PathFinderController script to and assig it to 
	 */
	void Awake () 
	{
		GameObject myObject = GameObject.FindGameObjectWithTag("PathFinder");  //I created an empty object with tag PathFinder for now
		aPathFinder = myObject.GetComponent<PathFinderController>();
	}

	/**
	 *So this will be the method called by the State controller to have the NPC go to the target. It can be run in StateController and then that can call goalFound to make sure 
	 */

	private void moveToGoal(Vector3 theGoalPos)
	{
		bool goalFound = false;    ////////This is never used

		goalPos = theGoalPos;
		GetNewPath();

//		if(path != null)					// This may not be needed as its not being called as an update method. It will only be called after setting a point 
//		{
			while(!GoalReached())  ////////IS this supposed to  be GoalReached() method call or goalFound local bool variable?
			{
				if(NextNodeReached())
				{
					nextNodeIndex++;
					nextNodePos = path[nextNodeIndex];
				}
				else
				{
				
					Vector3 currPos = transform.position;
					float speed = speedMultiplier * Time.deltaTime;	//Set the speed here in case we want to change it while moving
				
					Vector3 motion = nextNodePos - currPos;			//Okay! Now set the new position for your motion
					motion.Normalize();							//this makes the vector3 motion have a magnitude (length) of 1
				
					Vector3 newPos = currPos * speed;					//create the new position
					transform.position = newPos;				//now move to the new position! ?
				}
			}
//		}
	}
	
//	// Update is called once per frame
//	void Update () 
//	{
//		//Add code for optimization here if needed
//
//		//Some code here...
//		if(findPathDynamically == true)			
//		{
//			//Some code here that would make sure that we wait pathRescanRate amount of seconds until we find new path
//			getNewPath();
//		}
//
//		//Most other code for determining movement should be added here
//
//	}

	/**
	 * This method gets a new path using the PathFinderController class
	 */
	private void GetNewPath()
	{
		//some code here....
		path = aPathfinder.GetBestPath(goalPos);  //compiler might not know that Awake() will always run before this thus the error. not sure though
		nextNodeIndex = 0;
	}

	/**
	 * This method checks if the NPC found its goal. Can be accessed from another class to help with additional behavior
	 * @return true if found the goal/target and false otehrwise
	 */ 
	public bool GoalReached()
	{
		Vector3 currentPos = transform.position;

		float xDistanceToNexNode = Mathf.Abs(currentPos.x - nextNodePos.x);
		float zDistanceToNextNode = Mathf.Abs(currentPos.z - nextNodePos.z);

		if((xDistanceToNexNode < 0.1 && zDistanceToNextNode < 0.1) && goalPos == nextNodePos)
			return true;
		else
			return false;
	}

	/**
	 * This allows this class  to determine if the next node in the path has been found
	 * @return true if found the next node and false otherwise
	 */
	public bool NextNodeReached()
	{
		Vector3 currentPos = transform.position;

		float xDistanceToNexNode = Mathf.Abs(currentPos.x - nextNodePos.x);
		float zDistanceToNextNode = Mathf.Abs(currentPos.z - nextNodePos.z);

		if(xDistanceToNexNode < 0.1 && zDistanceToNextNode < 0.1)
			return true;
		else
			return false;
	}
		 
	/**
	 * This method allows other classes to set if the pathfinding should be dynamic or not
	 * @parm option true would set findPathDynamically to true
	 */
	public void SetPathfindingToDynamic(bool option)
	{
        findPathDynamically = option;
	}

	/**
	 * Method allows other classes to change the speed in which the NPC moves towards goal
	 * @param theSpeed a float that will change the meters per seconds at which the NPC will direct to goal
	 */
	public void SetPathfindingSpeed(float theSpeed)
	{
        speedMultiplier = theSpeed;
	}
}
