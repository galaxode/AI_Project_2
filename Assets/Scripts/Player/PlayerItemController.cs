using UnityEngine;
using System.Collections;

public class PlayerItemController : MonoBehaviour 
{
	private int wordCount;
	public string[] wordPrize = new string[] {"Ardent", "Enigma", "Fastidious", "Decipher" }; //these are words you can "learn" or "collect"
	public GUIText wordCountText;
	public GUIText wordText;
	public bool atLeastOneGemCollected = false;

	public GameObject gemTrackerObject;		//Object that will hold whatever is holding the script for keeping track of gems
	private GemTracker gemTracker;			//Same as above

	public GUIText currentHealthText;
	public int maxHealth;
	public int currentHealth;

	
					

	void Start()
	{
		gemTracker = gemTrackerObject.GetComponent<GemTracker>();		//This is to allows us to keep track of gems
		wordCount = 0;
		SetWordCountText();
		SetWordText();
		SetCurrentHealthText();

	}

	void Update()
	{
		if (gemTracker.GetAmountOfGems() == 0)
		{
			if (wordCount == 0)
				Application.LoadLevel(4);
		}
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
			gemTracker.RemoveGem();			//Update gem tracker by removing a Gem
			wordCount++;
			atLeastOneGemCollected = true;
			SetWordCountText ();
			SetWordText();
		}
		if (other.gameObject.tag == "Food") //make food disappear, increment counter, set text
		{
			other.gameObject.SetActive(false);
			currentHealth++;
			SetCurrentHealthText();

		}
		if (other.gameObject.tag == "EnemyAmmo")
		{
			Destroy(other.gameObject);
			currentHealth--;
			if (currentHealth <= 0)
			{
				currentHealth = 0;
				SetCurrentHealthText();
				Application.LoadLevel(3);
			}
			else
				SetCurrentHealthText();
		}
		if (other.gameObject.tag == "MagicalGate")
		{

		}

	}

	/**
	* This method sets the word count text to be displayed on the game screen
	*/
	void SetWordCountText()
	{
		wordCountText.text = "Words: " + wordCount.ToString();
	}

	/**
	* This method sets the word text showing the word collected to be displayed on the game screen
	* based on how many word gems have been collected
	*/
	void SetWordText()
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
	* This method sets the health text showing current health
	*/
	void SetCurrentHealthText()
	{
		currentHealthText.text = "Health: " + currentHealth.ToString();
	}


}
