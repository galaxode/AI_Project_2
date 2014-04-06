using UnityEngine;
using System.Collections;

public class NPC1Health : MonoBehaviour 
{
	
	public int maxHealth;
	public int healthMeter;
	public int ammoDamage;
	

	public int GetHealth()
	{
		return healthMeter;
	}

	public void Damage(int damageAmount)
	{
		healthMeter -= damageAmount;

		if (healthMeter <= 0)
			this.gameObject.SetActive(false);  //deactivate if healthMeter is depleted
	}

	public void Heal(int healAmount)
	{
		if (healthMeter < maxHealth)
			{	
				healthMeter = healthMeter + healAmount;
				if (healthMeter >= maxHealth)
					healthMeter = maxHealth;
			}
	}

	//Damage NPC1 if collide with Ammo.
	//However I ran into a bug because he would lose life every time he tried to attack and his ammo would disappear
	private void OnTriggerEnter(Collider col)
	{
		if(col.gameObject.tag == "PlayerAmmo")
		{
			Destroy(col.gameObject);
			
			Damage(ammoDamage);			//damage 1 if collide with ammo
		}
	}
	
}
