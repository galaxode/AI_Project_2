using UnityEngine;
using System.Collections;

public class NPC1StateContoller : MonoBehaviour 
{
	private SearchingState search;
	RaycastHit hit;
	bool inASearch;


	void Awake () 
	{
		search = GetComponent<SearchingState>();
		inASearch = false;
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (inASearch)
		{
			if (!search.GoalReached())
				search.MoveToGoal();
		}

		if(Input.GetButtonDown("Fire1")) 		//Input manager sets this as Mouse0 which is the primary mouse button
		{
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			Debug.DrawRay(ray.origin, ray.direction * 10, Color.yellow, 200);
			Debug.Log("in the first if");

			if(Physics.Raycast(ray, out hit, 100))
		    {
				Debug.Log("in the Second if");
				if(hit.transform.tag == "floor")
				{
					Debug.Log("in the third if");

					Vector3 newTarget = new Vector3(hit.point.x, transform.position.y, hit.point.z);
				//	SearchingState search = GetComponent<SearchingState>();				//Maybe not needed if it works during awake

					Debug.Log(hit.point.x + " and Y " + transform.position.y + "and z  " + hit.point.z);
				
					search.setGoalPos(newTarget);

					inASearch = true;

				}
			}
		}
	}
}
