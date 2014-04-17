using UnityEngine;
using System.Collections;

public class PlayerThrowingController : MonoBehaviour {

	public int amountOfAmmo = 20;

	public float shootingForce = 300;
	public Transform ammoSpawner;
	public Rigidbody ammo;

	// Update is called once per frame
	void Update () 
	{
		if(Input.GetButtonDown("Shoot") && amountOfAmmo > 0)
		{
			Rigidbody cloneAmmo;
			
			cloneAmmo = Instantiate(ammo, ammoSpawner.position, ammoSpawner.rotation) as Rigidbody;
			cloneAmmo.rigidbody.AddForce(rigidbody.velocity + ammoSpawner.forward * shootingForce);	//Thow it at player 
		
			amountOfAmmo--;				// Reduce the ammount of ammo available 
		}
	}

	void OnTriggerEnter(Collider col)
	{
		if(col.gameObject.tag == "PlayerAmmo")
		{
			Destroy(col.gameObject);

			amountOfAmmo++;
		}
	}
}
