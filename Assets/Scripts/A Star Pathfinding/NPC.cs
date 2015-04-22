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

	public float radius = 1f;

	int nextWaypoint = 0;

	// This sets whether or not the NPC follows the path back.
	// Off by default.
	public bool loop = false;

	void Start()
	{
		m_Animator = GetComponent<Animator>();

		pathfinder = GameObject.FindObjectOfType<Pathfinding>();

		foreach(Animator anim in GetComponentsInChildren<Animator>())
		{
			if(anim != m_Animator)
			{
				m_Animator.avatar = anim.avatar;
				Destroy( anim );
				break;
			}
				
		}

		path = pathfinder.FindPath( transform.position, target.position );
		CheckGroundLevelAtAllNodes();
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

	void Update()
	{
		if (path.Length > 0)
			MoveTowardsDestination();

		if(Input.GetKeyUp(KeyCode.R))
		{
			target = targets[UnityEngine.Random.Range( 0, targets.Length )];
			path = pathfinder.FindPath( transform.position, target.position );
		}
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

			//direction = direction.normalized * speed * Time.deltaTime;

			//transform.position += direction;

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
		Gizmos.color = Color.green;
		Gizmos.DrawWireSphere( transform.position, radius );

		if (path != null)
		{
			for (int i = nextWaypoint; i < path.Length; i++)
			{
				Gizmos.color = Color.black;
				Gizmos.DrawWireCube( path[i], Vector3.one );

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
