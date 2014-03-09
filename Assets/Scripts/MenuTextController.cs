using UnityEngine;
using System.Collections;

public class MenuTextController : MonoBehaviour 
{

	public bool isQuitButton = false;

	//change color of text when you hover mouse over text
	void OnMouseEnter()
	{
		renderer.material.color = Color.cyan;
	}

	//return to white color after mouse is not hovering over text
	void OnMouseExit()
	{
		renderer.material.color = Color.white;
	}

	void OnMouseUp()
	{
		if (isQuitButton) 
		{
			Application.Quit(); 		//quit game
		} 
		else 
		{
			Application.LoadLevel(1); 	//load level
		}
	}
}
