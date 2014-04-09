using UnityEngine;
using System.Collections;

public class GateController : MonoBehaviour {

	private float intervalCheckTime = 2f;
	private Vector3 upPos;
	private Vector3 downPos;
	private bool wallIsUp = false;
	public float smooth = 10f;

	void Awake()
	{
		upPos = new Vector3(transform.position.x,transform.position.y + 3.5f, transform.position.z); 
		downPos = transform.position;
	}
	
	void Update ()
	{
		intervalCheckTime -= Time.deltaTime;
		
		if (intervalCheckTime <= 0)
		{
			Debug.Log("Entered first part");

			if(!wallIsUp)
			{
				Debug.Log("Entered Second part");
				//transform.position = Vector3.Lerp(transform.position, upPos, smooth * Time.deltaTime);
				transform.position = upPos;
				wallIsUp = true;
			}
			else
			{
				Debug.Log("Entered Third part");
				//transform.position = Vector3.Lerp(transform.position, downPos, smooth * Time.deltaTime);
				transform.position = downPos;
				wallIsUp = false;
			}

			intervalCheckTime = 5f;
		}
	}
}
