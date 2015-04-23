using UnityEngine;
using System.Collections;

public class Health : MonoBehaviour 
{
	[HideInInspector]
	public float health = 100f;
	public float maxHealth = 100f;

	void Awake()
	{
		health = maxHealth;
	}

	void Update()
	{
		ClampHealth( maxHealth );
	}

	public void Heal(float amount)
	{
		health += amount;
	}

	public void ReceiveDamage(float amount)
	{
		health -= amount;
	}

	void ClampHealth(float max)
	{
		Mathf.Clamp( health, 0, maxHealth );
	}
}
