using UnityEngine;
using System.Collections;

public class PlayerItemController : MonoBehaviour 
{
	private int wordCount;
	public string[] wordPrize = new string[] {"Ardent", "Enigma", "Fastidious", "Decipher" };
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

	void setWordCountText()
	{
		wordCountText.text = "Words: " + wordCount.ToString();
	}

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

	void setFoodCountText()
	{
		foodCountText.text = "Food: " + foodCount.ToString();
	}
}
