using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Character {

	private float health;
	private float healthRegenSpeed;
	private float movementSpeed;
	private float rollDistance;
	private float attackSpeed; // handles the player attacks currently
	private float attackDamage;
	private float blockHealth;
	private float blockHealthRegeneration;
	private float attributePoints;
	private const int STATS = 7;
	private int[] statLevels = new int[STATS];
	private float attackDamageCost, attackSpeedCost, healthCost, healthRegenCost, 
	trapDamageCost, blockHealthCost, blockHealthRegenCost;
	private float attackDamageIncrease, attackSpeedIncrease, healthIncrease, healthRegenIncrease, 
	trapDamageIncrease, blockHealthIncrease, blockHealthRegenIncrease;
	private float attackDamageMultiplier, attackSpeedMultipler, healthMultipler, 
	healthRegenMultipler, trapDamageMultipler, blockHealthMultipler, blockHealthRegenMultipler;

	public void SaveCharacterData(PlayerController pc)
	{
		this.health = pc.GetHealth ();
		this.healthRegenSpeed = pc.GetHealthRegenSpeed ();
		this.movementSpeed = pc.GetMovementSpeed ();
		this.rollDistance = pc.GetRollDistance ();
		this.attackSpeed = pc.GetAttackSpeed ();
		this.attackDamage = pc.GetAttackDamage ();
		this.blockHealth = pc.GetBlockHP ();
		this.blockHealthRegeneration = pc.GetBlockHealthRegen ();
		this.attributePoints = pc.GetAttributePoints ();
		this.statLevels = pc.GetStatLevels ();
		this.attackDamageCost = pc.GetPlayerAttributes ().attackDamageCost;
		this.attackSpeedCost = pc.GetPlayerAttributes ().attackSpeedCost;
		this.healthCost = pc.GetPlayerAttributes ().healthCost;
		this.healthRegenCost = pc.GetPlayerAttributes ().healthRegenCost; 
		this.trapDamageCost = pc.GetPlayerAttributes ().trapDamageCost;
		this.blockHealthCost = pc.GetPlayerAttributes ().blockHealthCost;
		this.blockHealthRegenCost = pc.GetPlayerAttributes ().blockHealthRegenCost;
		this.attackDamageIncrease = pc.GetPlayerAttributes ().attackDamageIncrease;
		this.attackSpeedIncrease = pc.GetPlayerAttributes ().attackSpeedIncrease;
		this.healthIncrease = pc.GetPlayerAttributes ().healthIncrease;
		this.healthRegenIncrease = pc.GetPlayerAttributes ().healthRegenIncrease;
		this.trapDamageIncrease = pc.GetPlayerAttributes ().trapDamageIncrease;
		this.blockHealthIncrease = pc.GetPlayerAttributes ().blockHealthIncrease;
		this.blockHealthRegenIncrease = pc.GetPlayerAttributes ().blockHealthRegenIncrease;
		this.attackDamageMultiplier = pc.GetPlayerAttributes ().attackDamageMultiplier;
		this.attackSpeedMultipler = pc.GetPlayerAttributes ().attackSpeedMultipler;
		this.healthMultipler = pc.GetPlayerAttributes ().healthMultipler;
		this.healthRegenMultipler = pc.GetPlayerAttributes ().healthRegenMultipler;
		this.trapDamageMultipler = pc.GetPlayerAttributes ().trapDamageMultipler;
		this.blockHealthMultipler = pc.GetPlayerAttributes ().blockHealthMultipler;
		this.blockHealthRegenMultipler = pc.GetPlayerAttributes ().blockHealthRegenMultipler;
	}

	public void LoadPlayerData(PlayerController pc)
	{
		/* Zero out all the stats before assigning values.
		 * The player controller setters use += and this would 
		 * to the current value, not load a new value in.
		 */
		pc.ZeroOutStatsForLoad ();
		pc.SetHealth(health);
		pc.SetHealthRegenSpeed (healthRegenSpeed);
		pc.SetMovementSpeed (movementSpeed);
		pc.SetRollDistance (rollDistance);
		pc.SetAttackSpeed (attackSpeed);
		pc.SetAttackDamage (attackDamage);
		pc.SetBlockHP (blockHealth);
		pc.SetBlockHealthRegen (blockHealthRegeneration);
		pc.SetAttributePoints (attributePoints);
		pc.SetStatsLevels (statLevels);
		pc.GetPlayerAttributes ().attackDamageCost = this.attackDamageCost;
		pc.GetPlayerAttributes ().attackSpeedCost = this.attackSpeedCost;
		pc.GetPlayerAttributes ().healthCost = this.healthCost;
		pc.GetPlayerAttributes ().healthRegenCost = this.healthRegenCost; 
		pc.GetPlayerAttributes ().trapDamageCost = this.trapDamageCost;
		pc.GetPlayerAttributes ().blockHealthCost = this.blockHealthCost;
		pc.GetPlayerAttributes ().blockHealthRegenCost = this.blockHealthRegenCost;
		pc.GetPlayerAttributes ().attackDamageIncrease = this.attackDamageIncrease;
		pc.GetPlayerAttributes ().attackSpeedIncrease = this.attackSpeedIncrease;
		pc.GetPlayerAttributes ().healthIncrease = this.healthIncrease;
		pc.GetPlayerAttributes ().healthRegenIncrease = this.healthRegenIncrease;
		pc.GetPlayerAttributes ().trapDamageIncrease = this.trapDamageIncrease;
		pc.GetPlayerAttributes ().blockHealthIncrease = this.blockHealthIncrease;
		pc.GetPlayerAttributes ().blockHealthRegenIncrease = this.blockHealthRegenIncrease;
		pc.GetPlayerAttributes ().attackDamageMultiplier = this.attackDamageMultiplier;
		pc.GetPlayerAttributes ().attackSpeedMultipler = this.attackSpeedMultipler;
		pc.GetPlayerAttributes ().healthMultipler = this.healthMultipler;
		pc.GetPlayerAttributes ().healthRegenMultipler = this.healthRegenMultipler;
		pc.GetPlayerAttributes ().trapDamageMultipler = this.trapDamageMultipler;
		pc.GetPlayerAttributes ().blockHealthMultipler = this.blockHealthMultipler;
		pc.GetPlayerAttributes ().blockHealthRegenMultipler = this.blockHealthRegenMultipler;

	}

}
