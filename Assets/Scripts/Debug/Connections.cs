using UnityEngine;
using System.Collections;
[ExecuteInEditMode]
public class Connections : MonoBehaviour 
{
	void OnDrawGizmos()
	{
		Gizmos.color = Color.blue;
		for(int i = 0; i < transform.childCount; i++)
		{
			Transform child = transform.GetChild( i );
			AdjacentWaypoints w = child.GetComponent<AdjacentWaypoints>();

			for(int j = 0; j < w.adjacentWaypoints.Length; j++)
			{
				Gizmos.DrawLine( child.position, w.adjacentWaypoints[j].position );
			}
		}
	}
}
