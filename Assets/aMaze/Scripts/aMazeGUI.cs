using UnityEngine;
using System.Collections;

public class aMazeGUI : MonoBehaviour
{
	aMaze maze;
	
	// Use this for initialization
	void Start ()
	{
		GameObject gMaze = GameObject.Find("Maze");
		
		if (gMaze)
			maze = gMaze.GetComponent<aMaze>();
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}
	
	public void OnGUI()
	{
		if (maze && maze.Generating)
		{
			GUILayout.Label("Generating maze... " + (((float)maze.VisitedCells / (float)maze.TotalCells) * 100f).ToString("#") + "%");
			return;
		}
			
		GUILayout.BeginArea(new Rect(0,0,Screen.width,Screen.height));
		GUILayout.BeginVertical();
		
		if (maze)
		{
			GUILayout.BeginHorizontal();
			GUILayout.Label("Maze Size X: ");
			maze.mazeSizeX = int.Parse(GUILayout.TextField("" + maze.mazeSizeX, GUILayout.Width(50)));
			GUILayout.FlexibleSpace();
			GUILayout.EndHorizontal();
			
			GUILayout.BeginHorizontal();
			GUILayout.Label("Maze Size Z: ");
			maze.mazeSizeZ = int.Parse(GUILayout.TextField("" + maze.mazeSizeZ, GUILayout.Width(50)));
			GUILayout.FlexibleSpace();
			GUILayout.EndHorizontal();
			
			GUILayout.BeginHorizontal();
			GUILayout.Label("Cell count: " + (maze.mazeSizeX * maze.mazeSizeZ));
			GUILayout.FlexibleSpace();
			GUILayout.EndHorizontal();
			GUILayout.BeginHorizontal();
			if (GUILayout.Button("Generate"))
			{
				maze.Generate();	
			}
			GUILayout.FlexibleSpace();
			GUILayout.EndHorizontal();
		}
		
		GUILayout.FlexibleSpace();
		
		GUILayout.Label("WASD/arrow keys to move camera");
		GUILayout.Label("Hold left mouse down to look around");
		
		GUILayout.EndVertical();
		GUILayout.EndArea();
		
		
	}
}
