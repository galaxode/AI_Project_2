using UnityEngine;
using System.Collections;

public class HashIDs : MonoBehaviour 
{
	public int locomotionState;
	public int speedFloat;
	public int angularSpeedFloat;
	public int openBool;

	void Awake()
	{
		locomotionState = Animator.StringToHash("Base Layer.Locomotion");
		speedFloat = Animator.StringToHash("Speed");
		angularSpeedFloat = Animator.StringToHash("AngularSpeed");
		openBool = Animator.StringToHash("OpenBool");
	}


}
