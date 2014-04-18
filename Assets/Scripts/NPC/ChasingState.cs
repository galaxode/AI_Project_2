using UnityEngine;
using System.Collections;

public class ChasingState : MonoBehaviour {

	private Vector3 lastPlayerSeenPos;
	private SightController sight;
	private SearchingState search;
	private Vector3 currPlayerPos;
	private static float chaseTimer;

	public float chaseWaitTime = 2f;

	void Awake()
	{
		sight = GetComponent<SightController>();
		search = GetComponent<SearchingState>();
		currPlayerPos = new Vector3(1000f, 1000f, 1000f);
		lastPlayerSeenPos = new Vector3(0f,0f,0f);
		chaseTimer = 5f;
	}
	
	// Update is called once per frame
	public void ChasePlayer ()
	{

		chaseTimer += Time.deltaTime;

		if(chaseTimer > chaseWaitTime)
		{
			chaseTimer = 0f;
			lastPlayerSeenPos = sight.GetLastPlayerSeenPos();

			if(lastPlayerSeenPos != currPlayerPos)
			{
				currPlayerPos = new Vector3(lastPlayerSeenPos.x, transform.position.y, lastPlayerSeenPos.z);

				search.SetGoalPos(currPlayerPos);
				search.MoveToGoal();

				//Debug.Log("SHOULD BE CHASING!!!");

			}
		}
			

	}
}
