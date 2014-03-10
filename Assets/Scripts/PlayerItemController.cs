using UnityEngine;
using System.Collections;

public class PlayerItemController : MonoBehaviour 
{
	private int wordCount;
	public string[] wordPrize = new string[] {"Ardent", "Enigma", "Fastidious", "Decipher" }; //these are words you can "learn" or "collect"
	public GUIText wordCountText;
	public GUIText wordText;

	private int foodCount;
	public GUIText foodCountText;

	void Start()
	{
		wordCount = 0;
		setWordCountText();
		setWordText();
		foodCount = 0;
		setFoodCountText();
	}

	/**
	* This method creates a trigger that causes an item to be collected,
	* increments a counter, and sets text showing info relating to what you collected
	* @param other the collectible item is being collided with
	*/
	void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.tag == "WordGem") //make word gems disappear, increment counter, set text
		{
			other.gameObject.SetActive(false);
			wordCount++;
			setWordCountText ();
			setWordText();
		}
		if (other.gameObject.tag == "Food") //make food disappear, increment counter, set text
		{
			other.gameObject.SetActive (false);
			foodCount++;
			setFoodCountText();

		}
	}

	/**
	* This method sets the word count text to be displayed on the game screen
	*/
	void setWordCountText()
	{
		wordCountText.text = "Words: " + wordCount.ToString();
	}

	/**
	* This method sets the word text showing the word collected to be displayed on the game screen
	* based on how many word gems have been collected
	*/
	void setWordText()
	{	
		if (wordCount < 1) 
		{
			wordText.text = "No Words";
		} 
		else 
		{
			for (int i = 0; i < wordCount; i++) 
			{
				wordText.text = wordPrize [i];
			}
		}
	}
	
	/**
	* This method sets the food count text showing how many food items collected
	* It might turn into the players health meter later on
	*/
	void setFoodCountText()
	{
		foodCountText.text = "Food: " + foodCount.ToString();
	}
}
