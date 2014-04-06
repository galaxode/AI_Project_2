using UnityEngine;
using System.Collections;

public class NPC1ItemController : MonoBehaviour 
{

	private NPC1Health health;
	public int gemHealingPower;
	
	void Start()
	{
		health = GetComponent<NPC1Health>();
	}
	
	/**
	* This method creates a trigger that causes an item to be collected,
	* @param other the collectible item is being collided with
	*/
	void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.tag == "WordGem") //make word gems disappear
		{
			other.gameObject.SetActive(false);
			health.Heal(gemHealingPower);

		
		}
		if (other.gameObject.tag == "Food") //make food disappear
		{
			other.gameObject.SetActive (false);

			
		}
	}

}
