using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class aMaze : MonoBehaviour
{
	public enum Direction { None = -1, North, East, South, West };
	
	public bool debug;
	
	public GameObject[] cellPrefabs;
	
	bool generating;
	
	GameObject[,] maze;
	
	public int mazeSizeX = 10;
	public int mazeSizeZ = 10;
	
	public float cellSizeX = 1f;
	public float cellSizeZ = 1f;
	
	public bool centreMaze;
	
	int totalCells;
	int visitedCells;
	
	// Use this for initialization
	void Start ()
	{
		Generate();
	}
	
	// Update is called once per frame
	void Update()
	{
		
	}
	
	public void Generate()
	{
		if (cellPrefabs.Length == 0)
		{
			Debug.LogError("Maze.Generate() " + name + " has no cell prefabs assigned!");
			return;
		}
		
		StartCoroutine(DoGenerate());
	}
	
	IEnumerator DoGenerate()
	{
		generating = true;

		foreach(Transform child in transform)
			Destroy(child.gameObject);
		
		if (debug)
			Debug.Log ("Maze.DoGenerate() starting...");
		
		maze = new GameObject[mazeSizeX,mazeSizeZ];
		
		Vector3 centreOffset = new Vector3(-((mazeSizeX * cellSizeX) * 0.5f) + (cellSizeX * 0.5f), 0f, ((mazeSizeZ * cellSizeZ) * 0.5f) - (cellSizeZ * 0.5f));
		
		for (int y = 0; y < mazeSizeZ; y++)
		{
			for (int x = 0; x < mazeSizeX; x++)
			{
				Vector3 cellPos = transform.position + new Vector3(x * cellSizeX, 0f, -y * cellSizeZ);
				
				if (centreMaze)
					cellPos += centreOffset;
				
				GameObject cellPrefab = cellPrefabs[0];
				
				if (cellPrefabs.Length > 1)
				{
					cellPrefab = cellPrefabs[Random.Range(0, cellPrefabs.Length)];
				}
				
				GameObject gCell = Instantiate(cellPrefab, cellPos, Quaternion.identity) as GameObject;
				
				maze[x,y] = gCell;
				
				gCell.transform.parent = transform;
				
				aCell cell = gCell.GetComponent<aCell>();
				
				if (!cell)
					cell = gCell.AddComponent<aCell>();
				
				cell.X = x;
				cell.Y = y;
			}
		}
		
		if (debug)
			Debug.Log ("Maze.DoGenerate() created maze of " + (mazeSizeX*mazeSizeZ) + " cells");
		
		List<aCell> cellStack = new List<aCell>();
		
		totalCells = mazeSizeX * mazeSizeZ;
		aCell currentCell = maze[0,0].GetComponent<aCell>();
		visitedCells = 1;
		
		while (visitedCells < totalCells)
		{
			if (debug)
				Debug.Log ("Maze.DoGenerate() processing cell " + currentCell.name + " (" + visitedCells + "/" + totalCells + ")");
			
			List<aCell> neighbors = GetUnvisitedNeighbors(currentCell);
			
			if (debug)
				Debug.Log ("Maze.DoGenerate() got " + neighbors.Count + " unvisited neighbors");
			
			if (neighbors.Count > 0)
			{
				aCell neighbor = neighbors[Random.Range(0, neighbors.Count)];
				Direction c_dir = GetDirectionToNeighbor(currentCell, neighbor);
				currentCell.walls[(int)c_dir] = true;
				Direction n_dir = GetDirectionToNeighbor(neighbor, currentCell);
				neighbor.walls[(int)n_dir] = true;
				cellStack.Insert(0, currentCell);
				currentCell = neighbor;
				visitedCells++;
			}
			else
			{
				currentCell = cellStack[0];
				cellStack.RemoveAt(0);
			}
			
			yield return null;	
		}
		
		List<GameObject> statics = new List<GameObject>();
		
		foreach (GameObject gCell in maze)
		{
			aCell cell = gCell.GetComponent<aCell>();
			
			if (cell.walls[(int)Direction.North]) { Transform wall = gCell.transform.FindChild("North"); Destroy(wall.gameObject); }
			if (cell.walls[(int)Direction.East])  { Transform wall = gCell.transform.FindChild("East"); Destroy(wall.gameObject); }
			if (cell.walls[(int)Direction.South]) { Transform wall = gCell.transform.FindChild("South"); Destroy(wall.gameObject); }
			if (cell.walls[(int)Direction.West])  { Transform wall = gCell.transform.FindChild("West"); Destroy(wall.gameObject); }
			
			statics.Add(cell.gameObject);

			yield return null;
		}
		
		StaticBatchingUtility.Combine(statics.ToArray(), transform.gameObject);
		
		if (debug)
			Debug.Log ("Maze.DoGenerate() DONE!");
		
		generating = false;
	}
	
	public bool Generating
	{
		get { return generating; }	
	}
	
	public int TotalCells
	{
		get { return totalCells; }	
	}

	public int VisitedCells
	{
		get { return visitedCells; }	
	}

	public int MazeWidth
	{
		get { return mazeSizeX; }	
		set { mazeSizeX = value; }	
	}

	public int MazeHeight
	{
		get { return mazeSizeZ; }	
		set { mazeSizeZ = value; }	
	}
	
	public aCell GetCell(int x, int y)
	{
		if (maze == null)
			return null;
		
		if (x < 0 || x >= mazeSizeX || y < 0 || y >= mazeSizeZ)
			return null;
		
		return maze[x,y].GetComponent<aCell>();
	}
	
	public aCell GetNeighbor(aCell cell, Direction dir)
	{
		if (!cell)
			return null;
		
		int n_x = cell.X;
		int n_y = cell.Y;
		
		if (dir == Direction.North) n_y--;
		if (dir == Direction.East)  n_x++;
		if (dir == Direction.South) n_y++;
		if (dir == Direction.West)  n_x--;
		
		if (n_x >= 0 && n_x < mazeSizeX && n_y >= 0 && n_y < mazeSizeZ)
			return GetCell(n_x, n_y);
		
		return null;
	}
	
	public List<aCell> GetUnvisitedNeighbors(aCell cell)
	{
		List<aCell> neighbors = new List<aCell>();
		
		if (!cell)
			return neighbors;		

		for(int i = 0; i < 4; i++)
		{
			aCell n = GetNeighbor(cell, (Direction)i);
			
			if (n != null)
			{
				if (AllWallsIntact(n))
				{
					neighbors.Add(n);
				}
			}
		}
		
		return neighbors;
	}
	
	public Direction GetDirectionToNeighbor(aCell cell, aCell neighbor)
	{
		if (!cell || !neighbor)
			return Direction.None;
		
		if (GetNeighbor(cell, Direction.North) == neighbor) return Direction.North;	
		else if (GetNeighbor(cell, Direction.East) == neighbor) return Direction.East;	
		else if (GetNeighbor(cell, Direction.South) == neighbor) return Direction.South;	
		else if (GetNeighbor(cell, Direction.West) == neighbor) return Direction.West;	
		
		return Direction.None;
	}
	
	public bool AllWallsIntact(aCell cell)
	{
		if (!cell)
			return true;
		
		foreach(bool wall in cell.walls)
		{
			if (wall)
				return false;
		}
		
		return true;	
	}
}
