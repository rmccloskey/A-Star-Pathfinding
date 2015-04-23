using UnityEngine;
using System.Collections.Generic;
using System;
using System.Collections;

public class Pathfinding : MonoBehaviour
{
	Grid grid;

	void Awake()
	{
		grid = GetComponent<Grid>();
	}

	public Vector3[] FindPath( Vector3 startPos, Vector3 targetPos )
	{
		bool pathSuccess = false;
		Vector3[] waypoints = new Vector3[0];

		Node startNode = grid.NodeFromWorldPoint( startPos );
		Node targetNode = grid.NodeFromWorldPoint( targetPos );

		// The list of nodes which need to be evaluated
		List<Node> openSet = new List<Node>();
		// The list nodes which have already been evaluated
		HashSet<Node> closedSet = new HashSet<Node>();

		// The starting node needs to be evaluated, so it is added to the open set
		openSet.Add( startNode );

		while (openSet.Count > 0)
		{
			// The current node will be set to the node with the lowest cost
			Node currentNode = openSet[0];
			for (int i = 1; i < openSet.Count; i++)
			{
				// If cost of openSet[i] < cost of currentNode, currentNode is
				// set to openSet[i]. If fCost is equal, the heuristic cost is
				// evaluated, and if hCost of openSet[i] is lower, then currentNode
				// is set to openSet[i].
				if (openSet[i].fCost < currentNode.fCost || openSet[i].fCost == currentNode.fCost && openSet[i].hCost < currentNode.hCost)
				{
					currentNode = openSet[i];
				}
			}

			// current node has been evaluated, so it is removed from 
			// list of nodes which need evaluation, and added to list
			// of nodes which have already been evaluated.
			openSet.Remove( currentNode );
			closedSet.Add( currentNode );

			// If currentNode is the target node, path has been found
			// and loop is ended.
			if (currentNode == targetNode)
			{
				pathSuccess = true;
				break;
			}

			// Checks all of the neighbours of the current node
			foreach (Node neighbour in grid.GetNeighbours( currentNode ))
			{
				// If neighbour is not walkable or has already been evaluated
				// skip to the next neighbour node
				if (!neighbour.walkable || closedSet.Contains( neighbour ))
				{
					continue;
				}

				// Calculate the new movement cost to this neighbour
				int newMovementCostToNeighbour = currentNode.gCost + GetDistance( currentNode, neighbour );
				// If new cost is lower than originally calculated cost,
				// or the neighbour has not been evaluated
				if (newMovementCostToNeighbour < neighbour.gCost || !openSet.Contains( neighbour ))
				{
					// Set gCost and hCost
					neighbour.gCost = newMovementCostToNeighbour;
					neighbour.hCost = GetDistance( neighbour, targetNode );
					// Parent this neighbour to the current node
					neighbour.parent = currentNode;

					// If this neighbour has not been marked as requiring
					// evaluation, then do so now
					if (!openSet.Contains( neighbour ))
						openSet.Add( neighbour );
				}
			}
		}

		// If path has successfully been evaluated,
		// reverse this path
		if (pathSuccess)
			waypoints = RetracePath( startNode, targetNode );

		return waypoints;
	}

	/**
	 * Traces the path by finding parent of each node in the path,
	 * and adding it to the path.
	 * 
	 * Path is then reversed to correct direction and returned.
	 */
	Vector3[] RetracePath( Node startNode, Node endNode )
	{
		List<Node> path = new List<Node>();
		Node currentNode = endNode;

		while (currentNode != startNode)
		{
			path.Add( currentNode );
			currentNode = currentNode.parent;
		}

		Vector3[] waypoints = SimplifyPath( path );
		Array.Reverse( waypoints );

		return waypoints;

		//grid.path = path;

	}

	Vector3[] SimplifyPath(List<Node> path)
	{
		List<Vector3> waypoints = new List<Vector3>();
		Vector2 directionOld = Vector2.zero;

		for (int i = 1; i < path.Count; i++)
		{
			Vector2 directionNew = new Vector2( path[i - 1].gridX - path[i].gridX, path[i - 1].gridY - path[i].gridY );
			if (directionNew != directionOld)
			{
				waypoints.Add( path[i].worldPosition );
			}
			directionOld = directionNew;
		}
		return waypoints.ToArray();
	}

	/**
	 * Find the distance between two nodes.
	 */
	int GetDistance( Node nodeA, Node nodeB )
	{
		int dstX = Mathf.Abs( nodeA.gridX - nodeB.gridX );
		int dstY = Mathf.Abs( nodeA.gridY - nodeB.gridY );

		if (dstX > dstY)
			return 14 * dstY + 10 * (dstX - dstY);
		return 14 * dstX + 10 * (dstY - dstX);
	}
}