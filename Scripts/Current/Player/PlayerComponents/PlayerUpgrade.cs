using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUpgrade : MonoBehaviour {

	/** Singleton Instance for the class. **/
	private static PlayerUpgrade playerUpgrades = null;

	public static PlayerUpgrade GetInstance 
	{
		get { 
			playerUpgrades = playerUpgrades != null ? playerUpgrades : playerUpgrades = GameObject.Find (gameLogic).GetComponent<PlayerUpgrade> ();
			return playerUpgrades;
		}
	}

	/** Put the name of the GameObject that has this script attached. **/
	public static string gameLogic = "LevelSelectMenu";

	/// <summary>
	/// Upgrades a specificed skill on the player and Sets the cost of upgrading that skill again.
	/// </summary>
	/// <param name="pc">The player controller.</param>
	/// <param name="skill">The skill to be upgraded.</param>
	public bool UpgradeSkill (AT_Controller pc, string skill) 
	{
		/** Checks if the player has enough skill points to purchase the upgrade and applies the changes.
		 * If the player does not have enough points, returns false **/

		bool isUpgraded = false;
		switch (skill) {
		case "AttackDamage":
			if (pc.GetAttributePoints() >= pc.attributes.attackDamageCost) {
				pc.SetAttributePoints (pc.attributes.attackDamageCost * -1);
				pc.SetAttackDamage (pc.attributes.attackDamageIncrease);
				pc.attributes.attackDamageCost *= pc.attributes.attackDamageMultiplier;
                    pc.SetWeaponDamage(pc.GetAttackDamage());
				isUpgraded = true;
			}
			break;
		case "AttackSpeed":
			if (pc.GetAttributePoints() >= pc.attributes.attackSpeedCost) {
				pc.SetAttributePoints (pc.attributes.attackSpeedCost * -1);
				pc.SetAttackSpeed (pc.attributes.attackSpeedIncrease);
				pc.attributes.attackSpeedCost *= pc.attributes.attackSpeedMultipler;
				isUpgraded = true;
			}
			break;
		case "Health":
			if (pc.GetAttributePoints() >= pc.attributes.healthCost) {
				pc.SetAttributePoints (pc.attributes.healthCost * -1);
				pc.SetHealthMaximum (pc.attributes.healthIncrease);
				pc.attributes.healthCost *= pc.attributes.healthMultipler;
				isUpgraded = true;
			}
			break;
		case "HealthRegen":
			if (pc.GetAttributePoints() >= pc.attributes.healthRegenCost) {
				pc.SetAttributePoints (pc.attributes.healthRegenCost * -1);
				pc.SetHealthRegenSpeed (pc.attributes.healthRegenIncrease);
				pc.attributes.healthRegenCost *= pc.attributes.healthRegenMultipler;
				isUpgraded = true;
			}
			break;
		case "TrapDamage":
			if (pc.GetAttributePoints() >= pc.attributes.trapDamageCost) {
				pc.SetAttributePoints (pc.attributes.trapDamageCost * -1);
				isUpgraded = true;
			}
			break;
		case "BlockHealth":
			if (pc.GetAttributePoints() >= pc.attributes.blockHealthCost) {
				pc.SetAttributePoints (pc.attributes.blockHealthCost * -1);
				pc.SetBlockHP (pc.attributes.blockHealthIncrease);
				pc.attributes.blockHealthCost *= pc.attributes.blockHealthMultipler;
				isUpgraded = true;
			}
			break;
		case "BlockHealthRegen":
			if (pc.GetAttributePoints() >= pc.attributes.blockHealthRegenCost) {
				pc.SetAttributePoints (pc.attributes.blockHealthRegenCost * -1);
				pc.SetBlockHealthRegen (pc.attributes.blockHealthRegenIncrease);
				pc.attributes.blockHealthRegenCost *= pc.attributes.blockHealthRegenMultipler;
				isUpgraded = true;
			}
			break;
		default:
			Debug.LogError ("Error: " + skill + " not found!");
			isUpgraded = false;
			break;
		}
		return isUpgraded;
	}


}
