using UnityEngine;
using System.Collections;

public class HealthPack : MonoBehaviour 
{

	void OnTriggerEnter( Collider other )
	{
		if (other.GetComponent<Health>() != null)
			other.GetComponent<Health>().Heal( 100 );
	}
}
