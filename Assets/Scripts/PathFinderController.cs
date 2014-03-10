using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PathFinderController : MonoBehaviour
{

	public string nodeTag; //allows us to change the tag used for nodes from inspector

	private List<Node> nodes;	//Our main list of nodes we will use
	private Node startNode;
	private Node endNode;

	class Node
	{
		public enum State { ACTIVE, OPEN, CLOSED, START, GOAL };		

		private Vector3 pos;
		private float score = 0;
		private Node parentNode;
		private State state = State.ACTIVE;
	
		private List<Node> connectedNodes = new List<Node>();
		private List<Node> possibleParents = new List<Node>();

		/**
		 * This is the constructor method for Node class.
		 */ 
		public Node(Vector3 aPos, State aState = State.ACTIVE)
		{
			pos = aPos;
			state = aState;
		}
	
		/**
		 * This method sets the score of a node
		 * @param aScore
		 */
		public void SetScore(float aScore)
		{
			score = aScore;
		}

		/**
		 * This method sets the posisiton of the node as a vector
		 */
		
		public void SetPos(Vector3 aPos)
		{
			pos = aPos;
		}
		
		/**
		 * This method sets the parentNode for this node
		 * @param aNode the parent node of this node
		 */
		public void SetParentNode(Node aNode)
		{
			parentNode = aNode;
		}
		
		/**
		 * This method changes a node's state
		 * @param aNode the node whose state you want to change
		 */
		public void SetState(State aState)
		{
			state = aState;
		}
		
		/**
	    * This method adds a node to this node's connectedNode list
	    * @param aNode the node being added
	    */
		public void AddConnectedNode(Node aNode)
		{
			connectedNodes.Add (aNode);
		}
		
		/**
	    * This method adds a possible parent node to possibleParentNodes list
	    * @param aNode the node being added
	    */
		public void AddPossibleParent(Node aNode)
		{
			possibleParents.Add (aNode);
		}
		
		/**
	    * This method gets you the position of a certain node
	    * @return returns the node's state
	    */
		public Vector3 GetPos()	
		{
			return pos;
		}
		
		/**
	    * This method gets you the score of a node
	    * @return returns the score
	    */
		public float GetScore()
		{
			return score;
		}
		
		/**
		 * This method gets you the state of a certain node
	     * @param aNode the node whose state you want to get
		 * @return returns the node's state
		 */
		public State GetState()
		{
			return state;
		}
		
		/**
	    * This method gets you the position of a certain node
	    * @return returns the node's state
	    */
		public Node GetParentNode()
		{
			return parentNode;
		}
		
		/**
		 * This method gets you the list of connected nodes
		 * @return returns the list connected nodes
		 */
		public List<Node> GetConnectedNodes()
		{
			return connectedNodes;
		}
		
		/**
		 * This method gets you the list of possible parent nodes
		 * @return returns the list possible parent nodes
		 */
		public List<Node> GetPossibleParents()
		{
			return possibleParents;
		}

	}
	/**
	 * This method will be the main one that will be referenced by the NPC controller class to get the best path
	 * It will start setting up the nodes, including the start and end Node
	 */
	public List<Vector3> GetBestPath(Vector3 theStartNode, Vector3 theEndNode)
	{
		//Start by setting the global variables for startNode and endNode so they are accessible by all methods
		startNode = new Node(theStartNode);		
		startNode.SetState(Node.State.START);

		endNode = new Node(theEndNode);
		endNode.SetState(Node.State.GOAL);

		//Check if we can reach the goal in a straight path
		if(NodeReachable())
		{
			List<Vector3> bestPath = new List<Vector3>();
			bestPath.Add(theStartNode);
			bestPath.Add(theEndNode);

			return bestPath;
		}
		else
		{
			// Since we cannot trace a straight line from start to end we detect all nodes and put them in a list
			GameObject[] gameObjectNodes = GameObject.FindGameObjectsWithTag(nodeTag);
			nodes = new List<Node>();

			//Create the nodes and add properties as needed 
			foreach (GameObject node in gameObjectNodes)
			{
				Node aNode = new Node(node.transform.position);
				ConnectStart(aNode);	//Check right away if the node connects to start and set it to oppen if it does
				nodes.Add(aNode);		//Add nodes to our list of nodes
			}

			ConnectNodes(nodes);		//Now find possible connections for all nodes
			
			List<Node> nodesWithConnections= GetNodesWithPossibleParents(); //Get a new list of nodes with possible parents
			FindParentNode(nodesWithConnections);			//Set the best paent for each node

			List<Vector3> thePath = SelectBestPath(nodesWithConnections);		//Use our list of parented nodes to select the best path 

			return thePath;
		}

	}



	/**
	 * This Method will check if the end target can be reached without need for a pathfinding algorithm
	 * @return returns a bool indicated whether the we can reach the end without extra work or not
	 * @param start the start location 
	 * @param goal the target or goal location
	 */
	private bool NodeReachable()
	{
		Vector3 start = startNode.GetPos();
		Vector3 end = endNode.GetPos();

		float goalDistance = Vector3.Distance(start, end);

		if (goalDistance > .7f)
			goalDistance -= .7f;

		if (!Physics.Raycast(start, end - start, goalDistance))
		{
		    return true;
		}
		else
		    return false;
	}

	/**
	 * This method allows us to check one node at the time and if it connects to the start we open the node and add a score to it as well as add
	 * the start node as its parent
	 * @param start the start position
	 */
	private void ConnectStart(Node aNode)
	{
		float distanceToStart = Vector3.Distance(startNode.GetPos(), aNode.GetPos());
		
		if(!Physics.Raycast(startNode.GetPos(), aNode.GetPos() - startNode.GetPos(), distanceToStart))
		{
			aNode.SetParentNode(startNode);
			aNode.SetState(Node.State.OPEN);
			
			float distanceToGoal = Vector3.Distance(aNode.GetPos(), endNode.GetPos()); 
			aNode.SetScore(distanceToStart + distanceToGoal);

		}
	}

	/**
	 * This method connectes all the nodes that can connect to each other. Start node not connected here 
	 * THIS METHOD COULD POTENTIALLY DO THE SAME WORK AS THE findOpenNodeConnections METHOD. This could have an "if" check to see if the nodes are already in open state
	 * @param allNodes the list of nodes found (this list is set up in the getBestPath method)
	 */
	private void ConnectNodes (List<Node> allNodes)  //this method also connects to endpoints. we could try doing that in a separate method but it will												
	{																//require another 'for each' loop to do it
					
		foreach (Node node1 in allNodes)
		{
			foreach (Node node2 in allNodes)
			{
				 float distance = Vector3.Distance(node1.GetPos(), node2.GetPos());
							
				 if (!Physics.Raycast(node1.GetPos(), node2.GetPos() - node1.GetPos(), distance))
				 {
					  node1.AddConnectedNode(node2);
				 } 
			 }
						
			float distance2 = Vector3.Distance(endNode.GetPos(), node1.GetPos());			//this part connects them to endpoint (it's still inside that outer For Each loop
						
			if (!Physics.Raycast(endNode.GetPos(), node1.GetPos() - endNode.GetPos(), distance2))
			{
					node1.AddConnectedNode(endNode);
			}
		}
	}

	/**
	 * This method goes through all the nodes that are in OPEN state only and find nodes that it can connect to. It returns a list of new connections
	 * and sets those new connections to Open
	 * WE DONT HAVE TO MAKE IT SO THAT IT RETURNS A NEW LIST, BUT THIS IS WHAT I THINK WE COULD HAVE IT DO FOR NOW
	 * ALSO, THE CONNECTED NODES METHOD (LINE 63) COULD POTENTIALLY DO THIS JOB TOO
	 * @param allNodes a list of OPEN nodes to check their individual connection. EACH NODE IN THIS LIST WILL HAVE A LIST OF POTENTIAL PARENTS (possible parents)
	 * @return a list of new open nodes (those that connect to the nodes passed as argument 
	 */
	 private List<Node> GetNodesWithPossibleParents()
	 {  
		bool allNodesParented = false;
			
		List<Node> parentedNodes = new List<Node>(); 
			while(!allNodesParented)
			{
				allNodesParented = true;
						
				foreach(Node aNode in nodes)
				{
					if(aNode.GetState() == Node.State.OPEN)
					{
						allNodesParented = false;
						List<Node> connectedNodes = aNode.GetConnectedNodes();
								
						foreach(Node connectedNode in connectedNodes)
						{
							if(connectedNode.GetState() == Node.State.ACTIVE)
							{
								//Calculate a s cost in the case that aNode would be the connected node
								float distanceToStart = aNode.GetScore() + Vector3.Distance(connectedNode.GetPos(), connectedNode.GetPos());
								//float distanceToStart = Vector3.Distance(connectedNode.GetPos(), startNode.GetPos());
								float distanceToGoal = Vector3.Distance(connectedNode.GetPos(), endNode.GetPos()); 
										
								connectedNode.SetScore(distanceToStart + distanceToGoal);
								connectedNode.AddPossibleParent(aNode);
										
								parentedNodes.Add(connectedNode);
										
								connectedNode.SetState(Node.State.OPEN);				//Open the connected node so that is checked next time around	
										
							}
							else if(connectedNode.GetState() == Node.State.GOAL)
							{
								endNode.AddConnectedNode(aNode);
							}

							aNode.SetState(Node.State.CLOSED);							//Close the previous node
						}
					}
				}
			}
		return parentedNodes;							//This list will have ALL nodes with possible parents 
		}


	/**
	 * This method takes a list of open nodes and sets their best parent
	 * @param openNodes a list of open nodes
	 * FOR THIS METHOD TO WORK EACH NODE IN THE ARGUMENTLIST WILL HAVE TO HAVE possibleParents NOT NULL
	 */ 
	private void FindParentNode(List<Node> nodesWithPossibleParents) 
	{
		foreach (Node node in nodesWithPossibleParents)
		{
			List<Node> possibleParents = node.GetPossibleParents();

			foreach (Node parent in possibleParents)
			{
				float lowestScore = 0;
				Node bestParent = null;
				bool first = true;

				if (first)
				{
					lowestScore = parent.GetScore();
					bestParent = parent;
					first = false;
				} else if (lowestScore > parent.GetScore())
				{
					lowestScore = parent.GetScore();
					bestParent = parent;
				}
				node.SetParentNode(bestParent);
			}
		}
	}

	/**
	 * This method take a list of fully configured nodes (each node has a bestParent) and traces back routes from the goal 
	 * and selects best route based on total score
	 * @param parentedNodes a list of nodes with parents
	 * @return a list of positions for the best path to take from start to goal
	 */
	private List<Vector3> SelectBestPath(List<Node> parentedNodes)
	{
		//travel back through nodes to finding shortest route (lowest score)
		List<Node> bestPath = null;
		float lowestScore = -1;  //lowest score is set to -1 for the first journey back to the start node, at which point it will be set to
								// the first score. later, it will take on the score value only if it is greater than score
		foreach (Node node in endNode.GetConnectedNodes())
		{
			float score = 0;
			bool backtracking = true;
			Node currNode = node;
			List<Node> nodePath = new List<Node>();

			nodePath.Add(endNode);

			//Keep adding score (line 387 and 388) until we get to the start node
			while(backtracking)
			{
				nodePath.Add(currNode);

				//If we get to the start node, then compare total scores for each path and select only lowest total one
				if (currNode.GetState() == Node.State.START)
				{
					if (lowestScore < 0 || lowestScore > score) //check if lowestScore has not been set yet, or if there is a lower score
					{
						bestPath = nodePath;		//Add the current list of node being checked as the list of path with best score
						lowestScore = score;
					} 
					
					backtracking = false;
					break;
				}
				score += currNode.GetScore();			//Add the score of the next node 
				currNode = currNode.GetParentNode();	//Change the value of our curr node to the next node
			}
		}

		// Now we reverse the path as right now is from end to start
		bestPath.Reverse();
		List<Vector3> VectorPath = new List<Vector3>();
		foreach (Node node in bestPath)
		{
			VectorPath.Add(node.GetPos());
		}
		return VectorPath;
	}
}
