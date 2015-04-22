using UnityEngine;
using System.Collections;

/**
 * Handles AI line of sight detection of player
 * using a raycast cone.
 * 
 * "Player" tag MUST be applied to target.
 */
public class RaycastCone : MonoBehaviour
{
	public float fieldOfView = 60f;
	public float viewRange = 15f;

	// Reference to the target object
	public GameObject player;

	// Is the player in the vision area of the AI?
	bool playerInSight = false;

	void Awake()
	{
		player = player == null ? GameObject.FindGameObjectWithTag( "Player" ) : null;
	}

	void Update()
	{
		if(player != null)
			playerInSight = IsTargetVisible();
	}

	bool IsTargetVisible()
	{
		RaycastHit hit;

		Vector3 rayDirection = player.transform.position - transform.position;

		// If player is within field of view
		if((Vector3.Angle(rayDirection, transform.forward)) < fieldOfView / 2)
		{
			// Perform a raycast to check if view is obstructed
			if(Physics.Raycast(transform.position, rayDirection, out hit, viewRange))
			{
				if (hit.collider.gameObject == player)
					return true;
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