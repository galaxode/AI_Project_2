using UnityEngine;
using System.Collections;

public class PlayerThrowingController : MonoBehaviour {

	private int amountOfAmmo = 5;

	public float shootingForce = 300;
	public Transform ammoSpawner;
	public Rigidbody ammo;

	// Update is called once per frame
	void Update () 
	{
		if(Input.GetButtonDown("Fire1") && amountOfAmmo > 0)
		{
			Rigidbody cloneAmmo;
			
			cloneAmmo = Instantiate(ammo, ammoSpawner.position, ammoSpawner.rotation) as Rigidbody;
			cloneAmmo.rigidbody.AddForce(rigidbody.velocity + ammoSpawner.forward * shootingForce);	//Thow it at player 
		
			amountOfAmmo--;				// Reduce the ammount of ammo available 
		}
	}

	void OnTriggerEnter(Collider col)
	{
		if(col.gameObject.tag == "Ammo")
		{
			Destroy(col.gameObject);

			amountOfAmmo++;
		}
	}
}
