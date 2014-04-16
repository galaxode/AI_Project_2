using UnityEngine;
using System.Collections;

public class Level1CompleteTextController : MonoBehaviour {

	public bool isContinueButton = false;
	public bool isQuitButton = false;
	

	GameObject continueToNext;
	GameObject quit;
	
	
	void Start()
	{
		continueToNext = GameObject.FindWithTag("ContinueButton");
		quit = GameObject.FindWithTag("QuitButton");
	}
	
	
	//change color of text when you hover mouse over text
	void OnMouseEnter()
	{
		renderer.material.color = Color.green;
	}
	
	//return to white color after mouse is not hovering over text
	void OnMouseExit()
	{
		renderer.material.color = Color.white;
	}
	
	void OnMouseUp()
	{
		if (isContinueButton)
		{
			Application.LoadLevel(2); 	//load level 2
		}
		
		else if (isQuitButton) 
		{
			Application.Quit(); 		//quit game
		}
	}
}
