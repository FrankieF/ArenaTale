using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/** This class is the blocking object used by the melee/defender characters. **/

public class BlockObject : MonoBehaviour {
	[SerializeField] private float health;
	[SerializeField] private float maxHeath;
	[SerializeField] private float rechargeRate;
	[SerializeField] private float waitTime;

	/** Used to check if the health is recharging to avoid starting multiple recharges. **/
	private bool isHealthRecharging = false;

	/** This is a reference to the player to send damage **/
	[SerializeField] PlayerController player;


	public void OnBlock (float damage, bool isHeavyAttack)
	{
		float damageTaken = damage;
		if (health > 0) 
		{
			if (isHeavyAttack)
			{
				health = 0;
			} else {
				//float remainingHealth = health - damage;
				health -= damage;
				if (health < 0) {
					damageTaken = damage - health;
					health = 0;
				}
			}
		}
		// Send player damage here
		// player.Damage(damageTaken)?
		if (!isHealthRecharging)
			StartCoroutine ("RegenerateBlockHealth");

	}

	public IEnumerator RegenerateBlockHealth ()
	{
		isHealthRecharging = true;
		float t = 0f, time = 0f;
		while (health < maxHeath) {
			time += Time.deltaTime;
			t = time / rechargeRate;
			health = Mathf.Lerp (health, maxHeath, t);
			yield return new WaitForSeconds(waitTime);
		}
		health = maxHeath;
		isHealthRecharging = false;
	}

	public void OnMaxHealthChange (float amount)
	{
		if (amount > 0)
			maxHeath += amount;
		else
			Debug.Log ("Decreasing Max block health is not allowed");
	}

	public void ChangeRechargeRate (float newRate)
	{
		if (newRate > 0)
			rechargeRate = newRate;
		else
			Debug.Log ("Block health recharge cannot be negative!");
	}

}
