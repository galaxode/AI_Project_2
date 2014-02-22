using UnityEngine;
using System.Collections;

public class CameraControl : MonoBehaviour {
	
	RaycastHit hit;
	bool leftClickFlag = true;
	
	public GameObject actor;
	public string floorTag;

	Actor actorScript;
	
	void Start()
	{
		if (actor != null)
		{
			actorScript = (Actor)actor.GetComponent(typeof(Actor)); //why is he casting to Actor and using typeof?
		}
	}
	
	void Update () 
	{
		/***Left Click****/
		if (Input.GetKey(KeyCode.Mouse0) && leftClickFlag)
			leftClickFlag = false;
		
		if (!Input.GetKey(KeyCode.Mouse0) && !leftClickFlag)
		{
			leftClickFlag = true;
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			//Debug.DrawRay(ray.origin, ray.direction * 10, Color.yellow); //just to see the ray
			if (Physics.Raycast(ray, out hit, 100))
			{
				if (hit.transform.tag == floorTag)
				{
					float X = hit.point.x;
					float Z = hit.point.z;
					Vector3 target = new Vector3(X, actor.transform.position.y, Z);
					
					actorScript.MoveOrder(target);
				}
			}
		}
	}
}
