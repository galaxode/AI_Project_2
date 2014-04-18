using UnityEngine;
using System.Collections;

public class MagicalTeleportSpace : MonoBehaviour {

	public GameObject gemTrackerObject;
	private GemTracker gemTracker;
	
	void Awake()
	{
		gemTracker = gemTrackerObject.GetComponent<GemTracker>();
	}
	
	void OnTriggerEnter(Collider other)
	{
		if(other.gameObject.tag == "Player")
		{
			if (gemTracker.GetAmountOfGems() == 0)
				Application.LoadLevel(6);			//transition to words collected scene showing words with definition
			//need to figure out how to transfer info to another scene (ie. wordCount)
		}
	}
}
