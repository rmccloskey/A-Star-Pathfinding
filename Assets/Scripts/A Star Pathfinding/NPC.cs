using UnityEngine;
using System.Collections;
using System;

public class NPC : MonoBehaviour 
{
	Animator m_Animator;

	public Transform target;

	public Transform[] targets;

	public float speed = 3;

	public Vector3[] path;

	Pathfinding pathfinder;

	TablePathfinding table_pathfinder;

	public float radius = 1f;

	int nextWaypoint = 0;

	// This sets whether or not the NPC follows the path back.
	// Off by default.
	public bool loop = false;

	RaycastCone vision;

	public enum AI_Calculator
	{
		A_Star,
		Table
	};
	// Be careful using Table-Based. It works most times, but (I think) if there are
	// a lot of calculations in quick succession, it may kill your computer.
	public AI_Calculator aiType = AI_Calculator.A_Star;

	void Start()
	{
		m_Animator = GetComponent<Animator>();

		vision = GetComponentInChildren<RaycastCone>();

		pathfinder = GameObject.FindObjectOfType<Pathfinding>();

		table_pathfinder = GameObject.FindObjectOfType<TablePathfinding>();

		foreach(Animator anim in GetComponentsInChildren<Animator>())
		{
			if(anim != m_Animator)
			{
				m_Animator.avatar = anim.avatar;
				Destroy( anim );
				break;
			}
				
		}

		path = CalculateNewPath( target );
	}

	// This is just to automagically adjust the position of nodes so that the NPC
	// won't go below the ground while following the path. It's probably not even
	// necessary as a RigidBody could do the job, but I did it anyway.
	void CheckGroundLevelAtAllNodes()
	{
		float nodeRadius = GameObject.FindObjectOfType<Grid>().nodeRadius;
		for(int i = 0; i < path.Length; i++)
		{
			path[i].y += nodeRadius;
		}
	}

	Vector3[] CheckGroundLevelAtAllNodes(Vector3[] path)
	{
		float nodeRadius = GameObject.FindObjectOfType<Grid>() != null ? GameObject.FindObjectOfType<Grid>().nodeRadius : 0.5f;
 
		for(int i = 0; i < path.Length; i++)
		{
			path[i].y += nodeRadius;
		}

		return path;
	}

	void Update()
	{
		if (vision.targetInSight)
		{
			try
			{
				Vector3 pos = target.position;

				float distance = Vector3.Distance( pos, vision.lastTargetSighting.position );

				//if(distance > 1)
				//{

				path = CalculateNewPath( vision.lastTargetSighting );
				//}
				speed = 1f;
			}
			catch(System.NullReferenceException e)
			{
				;
			}
			
		}
		else
		{
			if(speed == 1)
			{
				path = CalculateNewPath( targets[UnityEngine.Random.Range( 0, targets.Length )] );
			}
			
			speed = 0.5f;
		}
			

		if (path.Length > 0)
			MoveTowardsDestination();

		if(Input.GetKeyUp(KeyCode.R))
		{
			path = CalculateNewPath( targets[UnityEngine.Random.Range( 0, targets.Length )] );
		}
	}

	Vector3[] CalculateNewPath(Transform target)
	{
		this.target = target;

		Vector3[] path;

		switch(aiType)
		{
			case AI_Calculator.A_Star:
				path = pathfinder.FindPath( transform.position, target.position );
				break;
			case AI_Calculator.Table:
				int startNode = table_pathfinder.GetIndexOfClosestNode( transform );
				int targetNode = table_pathfinder.GetIndexOfClosestNode( target );

				if (startNode > -1 && targetNode > -1)
					path = table_pathfinder.FindPath( startNode, targetNode );
				else
					path = new Vector3[0];
				break;
			default:
				path = new Vector3[0];
				break;
		}

		nextWaypoint = 0;
		path = CheckGroundLevelAtAllNodes(path);

		return path;
	}

	void MoveTowardsDestination()
	{
		if (nextWaypoint < path.Length)//IsInsideRadiusOf( path[path.Length - 1] ))
		{
			// Simplify movement by altering Y axis
			Vector3 wp = path[nextWaypoint];
			wp.y = transform.position.y;

			Vector3 direction = wp - transform.position;

			transform.position = Vector3.MoveTowards( transform.position, wp, speed * Time.deltaTime );

			// Smooth transition between rotations - Similar to transform.LookAt(),
			// but doesn't instantly swap rotation value
			var targetRotation = Quaternion.LookRotation( wp - transform.position );
			transform.rotation = Quaternion.Slerp( transform.rotation, targetRotation, 3 * Time.deltaTime );

			m_Animator.SetFloat( "Speed", speed );

			if (IsInsideRadiusOf(wp ))
			{
				nextWaypoint++;
			}
		}
		else
		{
			if(loop && path.Length > 1)
			{
				Array.Reverse( path );
				nextWaypoint = 0;
			}
			else
				m_Animator.SetFloat( "Speed", 0 );
		}
			
	}

	bool IsInsideRadiusOf(Vector3 position)
	{
		return Vector3.SqrMagnitude( transform.position - position ) < this.radius * this.radius;
	}

	public void OnDrawGizmos()
	{

		if (path != null)
		{
			for (int i = nextWaypoint; i < path.Length; i++)
			{
				Gizmos.color = Color.black;
				Gizmos.DrawWireCube( path[i], new Vector3(0.25f, 0.25f, 0.25f) );

				if (i == nextWaypoint)
				{
					Gizmos.DrawLine( transform.position, path[i] );
				}
				else
				{
					Gizmos.DrawLine( path[i - 1], path[i] );
				}
			}
		}
	}
}
