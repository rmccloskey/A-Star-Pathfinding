using UnityEngine;
using System.Collections;

public class TablePathTester : MonoBehaviour 
{

	TableGrid grid;
	TablePathfinding pathfinder;


	public Transform target;

	Vector3[] path = new Vector3[0];

	[Range(0, 12)]
	public int startNode = 0;
	[Range( 0, 12 )]
	public int targetNode = 5;

	void Awake()
	{
		grid = GetComponent<TableGrid>();
		pathfinder = GetComponent<TablePathfinding>();
	}

	void Start()
	{
		startNode = pathfinder.GetIndexOfClosestNode( GameObject.FindGameObjectWithTag( "Player" ).transform );
		targetNode = pathfinder.GetIndexOfClosestNode( target );
		path = pathfinder.FindPath( startNode, targetNode );
	}

	public void OnDrawGizmos()
	{
		Gizmos.color = Color.magenta;

		if(path.Length > 1)
		{
			for (int i = 0; i < path.Length - 1; i++ )
			{
				Gizmos.DrawLine( path[i], path[i + 1] );
			}
		}
	}
}
