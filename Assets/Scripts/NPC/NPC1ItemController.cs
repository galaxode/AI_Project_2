using UnityEngine;
using System.Collections;

public class NPC1ItemController : MonoBehaviour 
{

	private NPC1Health health;
	public int gemHealingPower;

	public GameObject gemTrackerObject;		//Object that will hold whatever is holding the script for keeping track of gems
	private GemTracker gemTracker;			//Same as above

	
	void Awake()
	{
		health = GetComponent<NPC1Health>();
		gemTracker = gemTrackerObject.GetComponent<GemTracker>();		//This is to allows us to keep track of gems
	}
	
	/**
	* This method creates a trigger that causes an item to be collected,
	* @param other the collectible item is being collided with
	*/
	void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.tag == "WordGem") //make word gems disappear
		{
			gemTracker.RemoveGem();				//Update tracker by 1
			other.gameObject.SetActive(false);
			health.Heal(gemHealingPower);

		
		}
		if (other.gameObject.tag == "Food") //make food disappear
		{
			other.gameObject.SetActive (false);

			
		}
	}

}
