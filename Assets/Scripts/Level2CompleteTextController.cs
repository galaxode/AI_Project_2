using UnityEngine;
using System.Collections;

public class Level2CompleteTextController : MonoBehaviour {

	public bool isGoToMenuButton = false;
	public bool isQuitButton = false;
	
	
	GameObject goToMenu;
	GameObject quit;
	
	
	void Start()
	{
		goToMenu = GameObject.FindWithTag("GoToMenuButton");
		quit = GameObject.FindWithTag("QuitButton");
	}
	
	
	//change color of text when you hover mouse over text
	void OnMouseEnter()
	{
		renderer.material.color = Color.blue;
	}
	
	//return to white color after mouse is not hovering over text
	void OnMouseExit()
	{
		renderer.material.color = new Color(.99f, .77f, .97f, 1);
	}
	
	void OnMouseUp()
	{
		if (isGoToMenuButton)
		{
			Application.LoadLevel(0); 	//load main menu
		}
		
		else if (isQuitButton) 
		{
			Application.Quit(); 		//quit game
		}
	}
}
