using UnityEngine;
using System.Collections;

public class GameOverTextController : MonoBehaviour {

	public bool isPlayAgainButton = false;
	public bool isQuitButton = false;

	
	GameObject playAgain;
	GameObject quit;


	void Start()
	{
		playAgain = GameObject.FindWithTag("PlayAgainButton");
		quit = GameObject.FindWithTag("QuitButton");
	}


	//change color of text when you hover mouse over text
	void OnMouseEnter()
	{
		renderer.material.color = Color.yellow;
	}
	
	//return to white color after mouse is not hovering over text
	void OnMouseExit()
	{
		renderer.material.color = Color.white;
	}
	
	void OnMouseUp()
	{
		if (isPlayAgainButton)
		{
			Application.LoadLevel(0); 	//load menu screen
		}

		else if (isQuitButton) 
		{
			Application.Quit(); 		//quit game
		}
	}

}
