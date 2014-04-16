using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class aCell : MonoBehaviour
{
	public bool[] walls = new bool[4];
	
	int x = -1;
	int y = -1;
	
	// Use this for initialization
	void Start ()
	{
	
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}
	
	public int X
	{
		get { return x; }
		set 
		{
			x = value;
			name +=  "_" + x + "_" + y;
		}
	}

	public int Y
	{
		get { return y; }
		set 
		{
			y = value;
			name +=  "_" + x + "_" + y;
		}
	}
	
}
