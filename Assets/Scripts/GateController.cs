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
		
		//Go up and then down every intervalCheckTime seconds 
		if (intervalCheckTime <= 0)
		{
			if(!wallIsUp)
			{
				transform.position = upPos;
				wallIsUp = true;
			}
			else
			{
				transform.position = downPos;
				wallIsUp = false;
			}

			intervalCheckTime = 5f;
		}
	 }
	public void OpenGate()
	{
		if(!wallIsUp)
		{
			transform.position = upPos;
			wallIsUp = true;
		}

	}
	public void CloseGate()
	{
		if(wallIsUp)
		{
			transform.position = upPos;
			wallIsUp = true;
		}
		
	}
}
