using UnityEngine;
using System.Collections;

public class AmmoDestroyer : MonoBehaviour 
{

	// Destroy ammo game object after 4 seconds
	void Start () 
	{
		Destroy(gameObject, 4);
	}
	

}
