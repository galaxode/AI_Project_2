using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SearchingState : MonoBehaviour {


	public bool findPathDinamically;		//Allows us to select if we want the NPC to find path dynamically in the inspector
	public bool debugMode;						
	public bool optimizePathfinding;		//Switches optimization to be consitent accross cpu speeds
	public int pathRescanRate = 0;				//Allows us to change the speed at which the path is re-scanned in the editor. We could implement this to make it nicer: http://docs.unity3d.com/Documentation/Components/editor-CustomEditors.html
	private float reScanTime;

	public float speedMultiplier = 2;			//Allows us to change the value at which speed is multiplied in the inspector

	private List<Vector3> path; 
	private Vector3 goalPos;				//The goal position as determined by other game classes
	private Vector3 nextNodePos;			//The next node in the path that the NPC will follow
	private int nextNodeIndex;

	private bool onNode; 
	public bool goalFound;

	float elapsedTime;

	private PathFinderController aPathFinder;
	

	
	/**
	 * Get the script from whatever compononet we choose to assign the PathFinderController script to and assig it to 
	 */
	void Awake () 
	{
		path = new List<Vector3>();

		goalPos = new Vector3(0,0,0);

		goalFound = false;
		onNode = true;

		elapsedTime = 0;
		reScanTime = 0;

		GameObject myObject = GameObject.FindGameObjectWithTag("PathFinder");  //I created an empty object with tag PathFinder for now
		aPathFinder = myObject.GetComponent<PathFinderController>();
	}

	/**
	 *So this will be the method called by the State controller to have the NPC go to the target. It can be run in StateController and then that can call goalFound to make sure 
	 */

	public void MoveToGoal()
	{
		elapsedTime += Time.deltaTime;	//Maybe this can just go in the following if clause?

		Debug.Log("Our goal is to get to : " + goalPos.x + " " + goalPos.y + " " + goalPos.z);

		Debug.Log("should be same as: " + path[path.Count - 1].x + " " + path[path.Count - 1].y + " " + path[path.Count -1].z);


		//Keep rechecking path every pathRescanRate time 
		if(findPathDinamically)
		{
			if(elapsedTime > reScanTime)
			{
				reScanTime = elapsedTime + pathRescanRate;
				//GetNewPath();
			}
		}

		if(path != null)
		{
			Debug.Log("path is not null");

			//Debug.Log(NextNodeReached());
			if(onNode)
			{
				onNode = false;
	
				Debug.Log("the current Path count is: " + path.Count);

				if(nextNodeIndex < path.Count)
				{	
					nextNodePos = path[nextNodeIndex];
				}	

				Debug.Log ("the current node index is : " + nextNodeIndex);
				Debug.Log ("The next Node x is " + nextNodePos.x);
			}
			else
			{
				
				Vector3 currPos = transform.position;

				Debug.Log("Our current pos is : " + currPos.x + " " + currPos.y + " " + currPos.z);

				float speed = speedMultiplier * Time.deltaTime;	//Set the speed here in case we want to change it while moving
				
				Vector3 motion = nextNodePos - currPos;			//Okay! Now set the new position for your motion
				motion.Normalize();							//this makes the vector3 motion have a magnitude (length) of 1


				
				//Vector3 newPos = new Vector3();
				currPos += motion * speed;					//create the new position
				transform.position = currPos;				//now move to the new position! ?

				NextNodeReached();

				Debug.Log ("Movement functions have ran");
			}
		}
	}

	/**
	 * This method gets a new path using the PathFinderController class
	 */
	private void GetNewPath()
	{
		path = aPathFinder.GetBestPath(transform.position, goalPos);  //compiler might not know that Awake() will always run before this thus the error. not sure though

		Debug.Log ("There are " + path.Count + " in sol");

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
        findPathDinamically = option;
	}

	/**
	 * Method allows other classes to change the speed in which the NPC moves towards goal
	 * @param theSpeed a float that will change the meters per seconds at which the NPC will direct to goal
	 */
	public void SetPathfindingSpeed(float theSpeed)
	{
        speedMultiplier = theSpeed;
	}


	public void setGoalPos(Vector3 pos)
	{
		goalPos = pos;
		GetNewPath();
	}
}
