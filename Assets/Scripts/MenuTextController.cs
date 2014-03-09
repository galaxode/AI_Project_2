using UnityEngine;
using System.Collections;

public class MenuTextController : MonoBehaviour 
{

	public bool isQuitButton = false;
	public bool isStartButton = false;
	public bool isChooseLevel = false;
	public bool isLevel1 = false;
	public bool isLevel2 = false;
	
	private bool showLevelChoices = false;
	
	GameObject startMenu;
	GameObject level1;
	GameObject level2;
	
	void Start()
	{
		startMenu = GameObject.FindWithTag("StartMenu");
		level1 = GameObject.FindWithTag("Level1");
		level2 = GameObject.FindWithTag("Level2");
	}
	
	void FixedUpdate()
	{
		
		if (showLevelChoices)
		{
			startMenu.transform.Translate(0f, 0.6f, 0f);	//raise the start menu wall up
			
			level1.renderer.enabled = true;					//show level 1 choice
			level2.renderer.enabled = true;					//show level 2 choice
			
			showLevelChoices = false;						//prevent the start menu from moving again
		}
	}
	
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
		else if (isStartButton)
		{
			Application.LoadLevel(1); 	//load first level
		}
		else if (isChooseLevel)			
		{
			showLevelChoices = true;	//show the level choices in update method
		}
		else if (isLevel1)			
		{
			Application.LoadLevel(1);	//load level 1
		}
		else if (isLevel2)
		{
			Application.LoadLevel(2);	//load level 2
		}
		
		
	}
	
}
