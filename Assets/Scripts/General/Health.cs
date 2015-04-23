using UnityEngine;
using System.Collections;

public class Health : MonoBehaviour 
{
	public float health = 100f;
	public float maxHealth = 100f;

	void Awake()
	{
		health = maxHealth;
	}

	void Update()
	{
		ClampHealth( maxHealth );

		if (Input.GetKeyUp( KeyCode.K ))
			ReceiveDamage( 76 );
	}

	public void Heal(float amount)
	{
		health += amount;
	}

	public void ReceiveDamage(float amount)
	{
		float prevHealth = health;
		health -= amount;
		if (prevHealth > 25 && health <= 25)
			GetComponent<NPC>().GoToHealthPack();
	}

	void ClampHealth(float max)
	{
		health = Mathf.Clamp( health, 0, maxHealth );
	}
}
