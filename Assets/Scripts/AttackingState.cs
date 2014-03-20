using UnityEngine;
using System.Collections;

public class AttackingState : MonoBehaviour {

	public static int amountOfAmmo = 10;
	public float timeInterval = 2f;
	public Rigidbody npcAmmo;
	public float shootingForce = 2f;

	private float timer;
	private Vector3 playerPosition;
	private Vector3 shootingDirection;
	private GameObject hand;

	void Awake()
	{
		timer = timeInterval;
		hand = GameObject.FindGameObjectWithTag("Hand");  
	}



	public void AttackPlayer()
	{

		timer -= Time.deltaTime;

		//We set this up to throw ammo every two seconds or those sepcifed for timeInterval
		if(timer <= 0 && amountOfAmmo > 0)
		{
			playerPosition = GameObject.FindGameObjectWithTag("Player").transform.position;
			shootingDirection = playerPosition - hand.transform.position;
			shootingDirection.Normalize();
			//shootingDirection.y = 2f;

			//The below creates new instances of the amo and positiones it at the "hand" position. 
			Rigidbody cloneAmmo;

			cloneAmmo = Instantiate(npcAmmo, hand.transform.position, hand.transform.rotation) as Rigidbody;
			cloneAmmo.rigidbody.AddForce(shootingDirection * shootingForce);	//Thow it at player 

			timer = timeInterval;		//Reset timer before next throw
			amountOfAmmo--;				// Reduce the ammount of ammo available 
		}
	}


	//Allows other classes to determine if the NPC is out of ammo
	public bool OutOfAmmo()
	{
		if(amountOfAmmo <= 0) 
			return true;
		else
			return false;
	}
}
