using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PathFinderController : MonoBehaviour
{

	public string nodeTag; //allows us to change the tag used for nodes from inspector

	private List<Node> nodes; //will be used to insert all the nodes in a list
	private Node startNode;// = GetComponent<Node>();
	private Node endNode;// = GetComponent<Node>();


//	void Start()
//	{
//		startNode = GetComponent<Node>();
//	}


	/**
	 * This method will be the main one that will be referenced by the NPC controller class to get the best path
	 * It will start setting up the nodes, including the start and end Node
	 */
	public List<Vector3> GetBestPath(Vector3 theStartNode, Vector3 theEndNode)
	{
		startNode = new Node(theStartNode, Node.State.START);
		endNode = new Node(theEndNode, Node.State.GOAL);

		//Check is we can reach the goal in a straight path
		if(NodeReachable())
		{
			List<Vector3> bestPath = new List<Vector3>();
			bestPath.Add(startNode.GetPos());
			bestPath.Add(endNode.GetPos());

			return bestPath;
		}
		else
		{
			GameObject[] gameObjectNodes = GameObject.FindGameObjectsWithTag(nodeTag);
			nodes = new List<Node>();

			//Create the nodes and add properties as needed such as parent if it connects to start. Add to the node array list
			foreach (GameObject node in gameObjectNodes)
			{
				//Node aNode = GetComponent<Node>;
				Node aNode = new Node(node.transform.position);
				ConnectStart(aNode);
				//ConnectEnd(aNode);
				nodes.Add(aNode);
			}

			ConnectNodes(nodes);
			
			List<Node> nodesWithPossibleParents = GetNodesWithPossibleParents();
			FindParentNode(nodesWithPossibleParents);
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
		    return true;
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
		
		if(!Physics.Raycast(startNode.GetPos(), aNode.GetPos(), distanceToStart))
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
					 //Debug.DrawRay(node1.GetPos(), node2.GetPos() - node1.GetPos(), Color.white, 1);
					  node1.AddConnectedNode(node2);
				 } 
			 }
						
			float distance2 = Vector3.Distance(endNode.GetPos(), node1.GetPos());			//this part connects them to endpoint (it's still inside that outer For Each loop
						
			if (!Physics.Raycast(endNode.GetPos(), node1.GetPos() - endNode.GetPos(), distance2))
			{
					//Debug.DrawRay(targetPos, point.GetPos() - targetPos, Color.white, 1);
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
					
					while(!allNodesParented)
					{
						allNodesParented = true;
						List<Node> parentedNodes = new List<Node>(); 
						
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
										float distanceToStart = Vector3.Distance(connectedNode.GetPos(), startNode.GetPos());
										float distanceToGoal = Vector3.Distance(connectedNode.GetPos(), endNode.GetPos()); 
										
										connectedNode.SetScore(distanceToStart + distanceToGoal);
										connectedNode.AddPossibleParent(aNode);
										
										parentedNodes.Add(connectedNode);
										
										connectedNode.SetState(Node.State.OPEN);				//Open the connected node so that is checked next time around (I HOPE)		
										
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
			}


	/**
	 * This method takes a list of open nodes and sets their best parent
	 * @param openNodes a list of open nodes
	 * FOR THIS METHOD TO WORK EACH NODE IN THE ARGUMENTLIST WILL HAVE TO HAVE possibleParents NOT NULL
	 */ 
	private void FindParentNode(List<Node> openNodes) 
	{

	}

	/**
	 * This method take a list of fully configured nodes (each node has a bestParent) and traces back routes from the goal 
	 * and selects best route based on total score
	 * @param parentedNodes a list of nodes with parents
	 * @return a list of positions for the best path to take from start to goal
	 */
	private List<Vector3> SelectBestPath(List<Node> parentedNodes)
	{
		
	}

	/**
	 * This method just checks if the node checked is the goal
	 * @param aNode the node checked
	 * @return true if the node is the goal, false if its not
	 */
	private bool GoalFound (Node aNode)
	{
		if (aNode.GetState() == Node.State.GOAL)
			return true;
		else
			return false;
	}







}
