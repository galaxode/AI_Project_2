using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SearchingState : MonoBehaviour {


	public bool findPathDynamically;		//Allows us to select if we want the NPC to find path dynamically in the inspector
	public bool debugMode;						
	public bool optimizePathfinding;		//Switches optimization to be consitent accross cpu speeds
	public float pathRescanRate = 0;		//Allows us to change the speed at which the path is re-scanned in the editor. We could implement this to make it nicer: http://docs.unity3d.com/Documentation/Components/editor-CustomEditors.html
	private float reScanTime;
	private float elapsedTime;

	public float speedMultiplier = 2;		//Allows us to change the value at which speed is multiplied in the inspector
	
	private List<Vector3> path; 			// and array of vectors containing the path to take 
	private Vector3 goalPos;				//The goal position as determined by other game classes
	private Vector3 nextNodePos;			//The next node in the path that the NPC will follow
	private int nextNodeIndex;				// used to keep count of node index

	private bool onNode; 					//To determined when a node has been reached
	public bool goalFound;					// NOT USED but keeping it as it may become useful later on

	private PathFinderController aPathFinder;	// This stores a reference to the PathFinderController script

	/**
	 * We start we an awake function to initalize all variables that need initialized
	 */
	void Awake () 
	{
		path = new List<Vector3>();		
		goalPos = new Vector3(0,0,0);	//If this is not done goalPos remains null

		goalFound = false;
		onNode = true;

		elapsedTime = 0;
		reScanTime = 0;

		GameObject myObject = GameObject.FindGameObjectWithTag("PathFinder");  //Empty object where we store all nodes 
		aPathFinder = myObject.GetComponent<PathFinderController>();
	}

	/**
	 *This method gets called by the StateController class of the NPC that will make use of this state 
	 */
	public void MoveToGoal()
	{
		elapsedTime += Time.deltaTime;	//Maybe this can just go in the following if clause?

		//If we choose to find path dinamically this will run every pathRescanRate seconds
		if(findPathDynamically)
		{
			if(elapsedTime > reScanTime)			//Will evaluate to true only every pathRescanRate seconds
			{
				reScanTime = elapsedTime + pathRescanRate;
				GetNewPath();
			}
		}

		if(path != null)			
		{
			if(onNode)
			{
				onNode = false;
			
				if(nextNodeIndex < path.Count)				//Just to avoid out of bounds 
				{	
					nextNodePos = path[nextNodeIndex];		//Change were NPC is walking towards after reaching each node
					nextNodePos.y = transform.position.y;
				}	
			}
			else
			{
				//This draws a ray in the scene view only - for troubleshooting
				if(debugMode)
				{
					for (int i=0; i<path.Count-1; ++i)
					{
						Debug.DrawLine((Vector3)path[i], (Vector3)path[i+1], Color.white, 0.01f);
					}
				}
			
				//The following lines move the NPC by a number of units determined by the value of speedMutiplier 
				Vector3 currPos = transform.position;	
				float speed = speedMultiplier * Time.deltaTime;	//Set the speed here in case we want to change it while moving
				Vector3 motion = nextNodePos - currPos;			//Now set the new position 
				motion.Normalize();								//this makes the vector3 motion have a magnitude (length) of 1

				currPos += motion * speed;					//create the new position by adding motion * speed units to our current position
				transform.position = currPos;				//now move to the new position

				NextNodeReached();							//Sets onNode variable - asking: should we keep moving to the same node? 
			}
		}
	}

	/**
	 * This method gets a new path using the PathFinderController class
	 */
	private void GetNewPath()
	{
		path = aPathFinder.GetBestPath(transform.position, goalPos);  
		nextNodeIndex = 0;	//Everytime we get a new path we must start from index 0
		onNode = true;		//If we do not set onNode back to true every time we click on a new point, the NPC would have to finish reaching the next node before following new path
	}

	/**
	 * This method allows us to set the goal position that we will move to from other classes
	 * Right now gets called every time we click somewhere on the environment 
	 */
	public void SetGoalPos(Vector3 pos)
	{
		goalPos = pos;
		if(debugMode){Debug.Log("goalPos: " + goalPos);}
		GetNewPath();		//We get a new path for every new position entered
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
		{
			goalFound = true;
			return true;
		}else
		{
			goalFound  = false;
			return false;
		}
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
		{
			onNode = true;
			nextNodeIndex++;
			return true;
		} else
		{
			onNode = false;
			return false;
		}
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
