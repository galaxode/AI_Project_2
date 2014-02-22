using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/**
 * Class for creating node Objects with their own properties. 
 * We could alternatively put this class somewhere else (maybe assign it to each node and 
 * make each node into a prefab?
 */	
public class Node : MonoBehaviour 
{


	public enum State { OPEN, CLOSED, ACTIVE, START, GOAL };		//I changed "States" to "State" because I thought it's more intuitive when referencing it
		
	//These variables could be coded as properties. That could potentially reduce the need for additional methods
	//See http://msdn.microsoft.com/en-us/library/x9fsa0sw.aspx
	private Vector3 pos;
	private float score;
	private Node parentNode;
	private State state;

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
	public void ChangeNodeState(State aState)
	{
		State = aState;
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
