using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerAttributes : MonoBehaviour {

	/** The cost associated with each upgrade. **/
	[SerializeField] public float attackDamageCost, attackSpeedCost, healthCost, healthRegenCost, 
	trapDamageCost, blockHealthCost, blockHealthRegenCost;
	/** The amount each skill increases for an upgrade. **/
	[SerializeField] public float attackDamageIncrease, attackSpeedIncrease, healthIncrease, healthRegenIncrease, 
	trapDamageIncrease, blockHealthIncrease, blockHealthRegenIncrease;
	/** The rate at which the cost increases for each skill upgrade. **/
	[SerializeField] public float attackDamageMultiplier, attackSpeedMultipler, healthMultipler, 
	healthRegenMultipler, trapDamageMultipler, blockHealthMultipler, blockHealthRegenMultipler;


}
