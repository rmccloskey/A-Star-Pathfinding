using UnityEngine;
using System.Collections;

/**
 * Handles AI line of sight detection of target
 * using a raycast cone.
 */
public class RaycastCone : MonoBehaviour
{
	public float fieldOfView = 60f;
	public float viewRange = 15f;

	// Reference to the target object
	public GameObject target;

	// Is the target in the vision area of the AI?
	public bool targetInSight = false;

	public Transform lastTargetSighting;

	void Awake()
	{
		target = target == null ? GameObject.FindGameObjectWithTag( "Player" ) : null;
	}

	void Update()
	{
		if(target != null)
			targetInSight = IsTargetVisible();
	}

	bool IsTargetVisible()
	{
		RaycastHit hit;

		Vector3 rayDirection = target.transform.position - transform.position;

		// If target is very close to player and unobstructed
		if(Physics.Raycast(transform.position, rayDirection, out hit, 1f))
		{
			if(hit.collider.gameObject == target)
			{
				lastTargetSighting = hit.transform;
				return true;
			}
		}

		// If target is within field of view
		if((Vector3.Angle(rayDirection, transform.forward)) < fieldOfView / 2)
		{
			// Perform a raycast to check if view is obstructed
			if(Physics.Raycast(transform.position, rayDirection, out hit, viewRange))
			{
				if (hit.collider.gameObject == target)
				{
					lastTargetSighting = hit.transform;
					return true;
				}	
			}
		}

		return false;
	}

	void OnDrawGizmos()
	{
		Gizmos.color = Color.blue;
		
		float halfFOV = fieldOfView / 2;

		Quaternion leftRayRotation = Quaternion.AngleAxis( -halfFOV, Vector3.up );
		Quaternion rightRayRotation = Quaternion.AngleAxis( halfFOV, Vector3.up );

		Vector3 leftRayDirection = leftRayRotation * transform.forward;
		Vector3 rightRayDirection = rightRayRotation * transform.forward;

		Gizmos.DrawRay( transform.position, leftRayDirection * viewRange );
		Gizmos.DrawRay( transform.position, rightRayDirection * viewRange );
	}
}