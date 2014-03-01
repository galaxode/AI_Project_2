using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Actor : MonoBehaviour {
	
	enum State
	{
		IDLE,										//create an IDLE state for when actor is not moving
		MOVING,										//create a MOVING state for when actor is moving
	}
	
	float m_speed;									//this is the movement speed attribute
	float m_speed_multi = 5;						//this is a movement multiplyer, a constant
	public bool DebugMode;							//default debug mode is true but you can set to false in inspector because public (see MoveToward() method on line 70.)
	
	bool onNode = true;								//is the actor on a node?
	Vector3 m_target = new Vector3(0, 0, 0);
	Vector3 currNode;
	int nodeIndex;
	List<Vector3> path = new List<Vector3>();
	NodeControl control;							//reference to NodeControl object/script
	State state = State.IDLE;						//the initial state is set to IDLE
	float OldTime = 0;								//not sure what these are yet
	float checkTime = 0;
	float elapsedTime = 0;
	
	void Awake()
	{
		GameObject cam = GameObject.FindGameObjectWithTag("MainCamera"); //get MainCamera object
		control = (NodeControl)cam.GetComponent(typeof(NodeControl)); //get NodeControl from MainCamera	
	}
	
	void Update () 
	{
		m_speed = Time.deltaTime * m_speed_multi;	//set the movement speed
		elapsedTime += Time.deltaTime;				//set elapsed time to elapsed time plus time.deltatime
		
		if (elapsedTime > OldTime)					//not sure why this is here, but oldtime is initialized at 0, so at leaste the first time around elapsedTime will definitely be more than oldTime
		{
			switch (state)							//test whether the the actor is IDLE or MOVING
			{
			case State.IDLE:						//if IDLE just get out of this switch
				break;
				
			case State.MOVING:						//if in MOVING state then do the following code
				OldTime = elapsedTime + 0.01f;		//ugh what is this

				if (elapsedTime > checkTime)		//ugh why?????
				{
					checkTime = elapsedTime + 1;	//make checkTime bigger than elapsedTime so that you do not do this again??
					SetTarget();					//call method on line 105 to get the path
				}
				
				if (path != null)					//now if you get a path from NodeControl then path will not be null
				{
					if (onNode)						//if you are on a node then do this, otherwise skip to the MoveToward() method call and move towards the next node
					{
						onNode = false;				//now we want to reset the onNode boolean
						if (nodeIndex < path.Count) //we want to do this if nodeIndex is less than tha number of elements (vector3 positions) in path list
							currNode = path[nodeIndex]; //set the current node to the position at nodeIndex
					} else
						MoveToward();				//calls the MoveToward method on line 70
				}
				break;
			}
		}
	}
	
	void MoveToward()
	{
		if (DebugMode)									//it will draw a line from node to node of the path in the editor window during play if this is turned on
		{
			for (int i=0; i<path.Count-1; ++i)
			{
				Debug.DrawLine((Vector3)path[i], (Vector3)path[i+1], Color.white, 0.01f);
			}
		}
		
		Vector3 newPos = transform.position;			//set newPos to the current position

		float Xdistance = newPos.x - currNode.x;		//calculate x-axis diestance from newPos.x to currNode.x	
		if (Xdistance < 0) Xdistance -= Xdistance*2;  	//if Xdistance is negative, then turn it into a positive
		float Ydistance = newPos.z - currNode.z;		//calculate z-axis (why is it called Ydistance?? isn't it Z?) distance from newPos.z to currNode.z
		if (Ydistance < 0) Ydistance -= Ydistance*2;	//if Ydistance is negative, then turn it into a positive
	
		if ((Xdistance < 0.1 && Ydistance < 0.1) && m_target == currNode) //Reached target
		{
			ChangeState(State.IDLE);
		}
		else if (Xdistance < 0.1 && Ydistance < 0.1) //It reaches a node but not the target node
		{
			nodeIndex++;							//we want to increment the nodeIndex so that in the update() we'll use the next node to sent currNode
			onNode = true;							//onNode needs to be true in order to do what I just said. (the check is on line 57)
		}
		
		/***Move toward waypoint***/
		Vector3 motion = currNode - newPos;			//Okay! Now set the new position for your motion
		motion.Normalize();							//this makes the vector3 motion have a magnitude (length) of 1
		newPos += motion * m_speed;					//create the new position
		
		transform.position = newPos;				//now move to the new position! ?
	}
	
	private void SetTarget()
	{
		path = control.Path(transform.position, m_target);		//this calls the Path method from NodeControl and assigns the list of vector3 positions to path attribute
		nodeIndex = 0;											
		onNode = true;
	}
	
	public void MoveOrder(Vector3 pos)  			//the CameraControl calls this method when you click on the game screen  on a walkable space
	{
		m_target = pos;
		SetTarget();
		ChangeState(State.MOVING);
	}
	
	private void ChangeState(State newState)		//simple setter to change the state
	{
		state = newState;
	}
}
