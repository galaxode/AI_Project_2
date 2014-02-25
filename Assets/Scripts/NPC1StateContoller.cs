using UnityEngine;
using System.Collections;

public class NPC1StateContoller : MonoBehaviour 
{
	private SearchingState search;
	RaycastHit hit;


	void Awake () 
	{
		search = GetComponent<SearchingState>();
	}
	
	// Update is called once per frame
	void Update ()
	{
		if(Input.GetButtonDown("Fire1") 		//Input manager sets this as Mouse0 which is the primary mouse button
		{
			Ray ray = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			ray.y = transform.position;			//Not sure if needed but we find out when scrips are more together
		
			if(Physics.Raycast(ray, hit, 100)
		    {
				if(hit.transform.tag == "floor")
				{
					Vector3 newTarget = new Vector3(hit.point.x, transform.position.y, hit.point.z)
					//SearchingState search = GetComponent<SearchingState>();				//Maybe not needed if it works during awake

					search.MovetoGoal(newTarget);

				}
			}
		}
	}
}
