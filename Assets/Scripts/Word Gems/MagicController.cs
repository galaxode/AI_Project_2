//using UnityEngine;
//using System.Collections;
//
//public class MagicController : MonoBehaviour 
//{
//	ParticleSystem myPart;
//	
//	void Awake()
//	{
//		myPart = transform.parent.gameObject.GetComponent<ParticleSystem>();
//	}
//
//	void OnTriggerEnter(Collider col)
//	{
//		if(col.tag == "Player")
//			myPart.Play();
//
//	}
//
//
//	void OnTriggerExit(Collider col)
//	{
//		if(col.tag == "Player")
//			myPart.Stop();
//		
//	}
//}
