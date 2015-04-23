using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class TablePathfinding : MonoBehaviour
{
	#region Table
	int[,] nodes = new int[,] {
	  //  A   B   C   D   E   F   G   H   I   J   K   L   M
		{ 0,  1,  1,  1,  1,  1,  1,  1,  1,  1,  1,  1,  1 }, // A
		// ---------------------------------------------------
		{ 0,  1,  2,  2,  4,  4,  4,  4,  4,  4,  4,  4,  4 }, // B
		// ---------------------------------------------------
		{ 1,  1,  2,  3,  3,  3,  3,  3,  3,  3,  3,  3,  3 }, // C
		// ---------------------------------------------------
		{ 2,  2,  2,  3,  4,  4,  6,  6,  6,  6,  6,  6,  6 }, // D
		// ---------------------------------------------------
		{ 1,  1,  1,  3,  4,  5,  5,  5,  5,  5,  5,  5,  5 }, // E
		// ---------------------------------------------------
		{ 4,  4,  4,  4,  4,  5,  6,  7,  7,  8,  8,  8,  8 }, // F
		// ---------------------------------------------------
		{ 3,  3,  3,  3,  3,  5,  6,  5,  5,  5,  5,  5,  5 }, // G
		// ---------------------------------------------------
		{ 5,  5,  5,  5,  5,  5,  5,  7,  8,  5,  5,  5,  5 }, // H
		// ---------------------------------------------------
		{ 7,  7,  7,  7,  7,  7,  7,  7,  8,  7,  7,  7,  7 }, // I
		// ---------------------------------------------------
		{ 5,  5,  5,  5,  5,  5,  5,  5,  5,  9,  10, 11, 11 }, // J
		// ---------------------------------------------------
		{ 9,  9,  9,  9,  9,  9,  9,  9,  9,  9,  10, 9,  9 }, // K
		// ---------------------------------------------------
		{ 9,  9,  9,  9,  9,  9,  9,  9,  9,  9,  9,  11, 12 }, // L
		// ---------------------------------------------------
		{ 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 11, 12 } // M
	};
	#endregion

	TableGrid grid;

	void Awake()
	{
		grid = GetComponent<TableGrid>();
	}

	public Vector3[] FindPath(int startNode, int targetNode)
	{
		List<Vector3> waypoints = new List<Vector3>();

		int currentNode = startNode;

		waypoints.Add( grid.nodes[currentNode].position );

		while(currentNode != targetNode)
		{
			currentNode = nodes[currentNode, targetNode];

			waypoints.Add( grid.nodes[currentNode].position );
		}

		return SimplifyPath( waypoints );
	}

	public int GetIndexOfClosestNode(Transform position)
	{
		float minDistance = 9999f;
		int indexOfClosest = -1;

		for(int i = 0; i < grid.nodes.Length; i++)
		{
			float distance = Mathf.Abs(Vector3.Distance( position.position, grid.nodes[i].position ));
			if(distance < minDistance)
			{
				minDistance = distance;
				indexOfClosest = i;
			}
		}

		return indexOfClosest;
	}

	Vector3[] SimplifyPath(List<Vector3> path)
	{
		return path.ToArray();
	}
}
