using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSwitcher : MonoBehaviour {

	[SerializeField] private GameObject melee;
    public GameObject meleeTargets;
	[SerializeField] private GameObject defender;
    public GameObject defenderTargets;
    [SerializeField] private GameObject ranger;
    public GameObject rangerTargets;
    private GameObject currentTargets;
    [SerializeField] private float switchingCooldown = 2f;

	private bool hasCooldownStarted = false;
	public bool isMeleeDead = false;
	public bool isDefenderDead = false;
	public bool isRangerDead = false;

	/** This is used for upgrading player attributes. This allows the current player to be passed to the script to upgrade. **/
	private static GameObject currentPlayer;
	[SerializeField] float meleeAttributeBuffer;
	[SerializeField] float defenderAttributeBuffer;
	[SerializeField] float rangerAttributeBuffer;
	private float currentBuffer;

	/** Handles regenerating character health when not currently active **/
	[SerializeField] private float meleeHealth;
	[SerializeField] private float meleeMaxHealth;
	[SerializeField] private float defenderHealth;
	[SerializeField] private float defenderMaxHealth;
	[SerializeField] private float rangerHealth;
	[SerializeField] private float rangerMaxHealth;
	[SerializeField] private float healthRegenRate;

    public static Vector3 GetCurrentPlayerPosition()
    {
        return currentPlayer.transform.position;
    }

	// Currently start as the melee player
	private void Start()
	{
		currentPlayer = melee;
        currentTargets = meleeTargets;
        melee.SetActive(true);
		defender.SetActive(false);
        defenderTargets.SetActive(false);
		ranger.SetActive(false);
        rangerTargets.SetActive(false);
	}

	private void Update()
	{
		DeadManager();
		SwitchingCooldownHandler();
		SwitchingPlayers();
    }

	private void SwitchingCooldownHandler()
	{
		if (hasCooldownStarted)
		{
			switchingCooldown -= Time.deltaTime;
			if (switchingCooldown <= 0)
			{
				hasCooldownStarted = false;
				switchingCooldown = 2f;
			}
		}
	}

	private void SwitchingPlayers()
	{
        // If switching to Melee Character
        if (Input.GetKeyDown(KeyCode.Alpha1) && currentPlayer != melee && !hasCooldownStarted && !isMeleeDead)
            OnCharacterSwitch("Melee");

        // If switching to Defender Character
        if (Input.GetKeyDown(KeyCode.Alpha2) && currentPlayer != defender && !hasCooldownStarted && !isDefenderDead)
            OnCharacterSwitch("Defender");

        // If switching to Ranger Character
        if (Input.GetKeyDown(KeyCode.Alpha3) && currentPlayer != ranger && !hasCooldownStarted && !isRangerDead)
            OnCharacterSwitch("Ranger");

	}

	private void DeadManager()
	{
		if (AT_MeleePlayerController.Instance)
		{
			if (AT_MeleePlayerController.Instance.IsDead) { isMeleeDead = true; }
		}
		if (AT_DefenderPlayerController.Instance)
		{
			if (AT_DefenderPlayerController.Instance.IsDead) { isDefenderDead = true; }
		}
		if (AT_RangerPlayerController.Instance)
		{
			if (AT_RangerPlayerController.Instance.IsDead) { isRangerDead = true; }
		}

	}

	/** Use the events below to update the attributes of each character,
	 * at different events throughout the game **/

	public void OnCharacterSwitch (string character) 
	{   
        switch (character) {
		case "Melee":
                melee.SetActive(true);
                StopCoroutine(RegenerateMeleeHealth());
                hasCooldownStarted = true;
                AT_MeleePlayerController.Instance.SetAttributePoints(meleeAttributeBuffer);
                AT_MeleePlayerController.Instance.SetHealth(meleeHealth);
                meleeAttributeBuffer = 0;
                meleeTargets.SetActive(true);
                if (currentPlayer == defender)
                {
                    
                    AT_MeleePlayerController.Instance.MyTransform.localPosition = 
                                            Helper.AddVectorPostivieY(AT_MeleePlayerController.Instance.MyTransform.localPosition, 
                                            defender.transform.localPosition);
                    defenderHealth = AT_DefenderPlayerController.Instance.GetHealth();
                    AT_DefenderPlayerController.Instance.ResetCharacterAnimations();
                    defender.SetActive(false);
                    defenderTargets.SetActive(false);
                    StartCoroutine(RegenerateDefenderHealth());
                }
                else
                {
                    AT_MeleePlayerController.Instance.MyTransform.localPosition = 
                                            Helper.AddVectorPostivieY(AT_MeleePlayerController.Instance.MyTransform.localPosition,
                                            ranger.transform.localPosition);
                    rangerHealth = AT_RangerPlayerController.Instance.GetHealth();
                    AT_RangerPlayerController.Instance.ResetCharacterAnimations();
                    ranger.SetActive(false);
                    rangerTargets.SetActive(false);
                    StartCoroutine(RegenerateRangerHealth());
                }
                currentPlayer = melee;
			break;
		case "Defender":
                defender.SetActive(true);
                StopCoroutine(RegenerateDefenderHealth());
                hasCooldownStarted = true;
                AT_DefenderPlayerController.Instance.SetAttributePoints(defenderAttributeBuffer);
                AT_DefenderPlayerController.Instance.SetHealth(defenderHealth);
                defenderAttributeBuffer = 0;
                defenderTargets.SetActive(true);
                if (currentPlayer == melee)
                {
                    AT_DefenderPlayerController.Instance.MyTransform.localPosition =
                                            Helper.AddVectorPostivieY(AT_DefenderPlayerController.Instance.MyTransform.localPosition,
                                            melee.transform.localPosition);
                    meleeHealth = AT_MeleePlayerController.Instance.GetHealth();
                    AT_MeleePlayerController.Instance.ResetCharacterAnimations();
                    melee.SetActive(false);
                    meleeTargets.SetActive(false);
                    StartCoroutine(RegenerateMeleeHealth());
                }
                else
                {
                    AT_DefenderPlayerController.Instance.MyTransform.localPosition =
                                            Helper.AddVectorPostivieY(AT_DefenderPlayerController.Instance.MyTransform.localPosition,
                                            ranger.transform.localPosition);
                    rangerHealth = AT_RangerPlayerController.Instance.GetHealth();
                    AT_RangerPlayerController.Instance.ResetCharacterAnimations();
                    ranger.SetActive(false);
                    rangerTargets.SetActive(false);
                    StartCoroutine(RegenerateRangerHealth());
                }
                currentPlayer = defender;
                break;
		case "Ranger":
                ranger.SetActive(true);
                StopCoroutine(RegenerateRangerHealth());
                hasCooldownStarted = true;
                AT_RangerPlayerController.Instance.SetAttributePoints(rangerAttributeBuffer);
                AT_RangerPlayerController.Instance.SetHealth(rangerHealth);
                rangerAttributeBuffer = 0;
                rangerTargets.SetActive(true);
                if (currentPlayer == defender)
                {
                    AT_RangerPlayerController.Instance.MyTransform.localPosition =
                                            Helper.AddVectorPostivieY(AT_RangerPlayerController.Instance.MyTransform.localPosition,
                                            defender.transform.localPosition);
                    defenderHealth = AT_DefenderPlayerController.Instance.GetHealth();
                    AT_DefenderPlayerController.Instance.ResetCharacterAnimations();
                    defender.SetActive(false);
                    defenderTargets.SetActive(false);
                    StartCoroutine(RegenerateDefenderHealth());
                }
                else
                {
                    AT_RangerPlayerController.Instance.MyTransform.localPosition =
                                            Helper.AddVectorPostivieY(AT_RangerPlayerController.Instance.MyTransform.localPosition,
                                            melee.transform.localPosition);
                    meleeHealth = AT_MeleePlayerController.Instance.GetHealth();
                    AT_MeleePlayerController.Instance.ResetCharacterAnimations();
                    melee.SetActive(false);
                    meleeTargets.SetActive(false);
                    StartCoroutine(RegenerateMeleeHealth());
                }
                currentPlayer = ranger;
                break;
		}
        //GameManager.BroadcastPlayerSwitch(currentPlayer);
    }

	public void OnLevelComplete ()
	{
		melee.SetActive (true);
		melee.GetComponent<AT_Controller> ().SetAttributePoints (meleeAttributeBuffer);
		meleeAttributeBuffer = 0;
		melee.SetActive (false);
		ranger.SetActive (true);
		ranger.GetComponent<AT_Controller> ().SetAttributePoints (rangerAttributeBuffer);
		rangerAttributeBuffer = 0;
		ranger.SetActive (false);
		defender.SetActive (true);
		defender.GetComponent<AT_Controller> ().SetAttributePoints (defenderAttributeBuffer);
		defenderAttributeBuffer = 0;
		defender.SetActive (false);
		isMeleeDead = false;
		isRangerDead = false;
		isDefenderDead = false;
	}

	public void OnEnemyKilled (float attributePoints)
	{
		switch (currentPlayer.name) {
		case "Melee":
			currentPlayer.GetComponent<AT_Controller> ().SetAttributePoints (attributePoints);
			defenderAttributeBuffer += attributePoints;
			rangerAttributeBuffer += attributePoints;
			break;
		case "Ranger":
			currentPlayer.GetComponent<AT_Controller> ().SetAttributePoints (attributePoints);
			meleeAttributeBuffer += attributePoints;
			defenderAttributeBuffer += attributePoints;
			break;
		case "Defender":
			currentPlayer.GetComponent<AT_Controller> ().SetAttributePoints (attributePoints);
			meleeAttributeBuffer += attributePoints;
			rangerAttributeBuffer += attributePoints;
			break;
		}
	}

	public IEnumerator RegenerateMeleeHealth ()
	{
		float t = 0f, time = 0f;
		while (meleeHealth < meleeMaxHealth) {
			t = time / healthRegenRate;
			meleeHealth = Mathf.Lerp (meleeHealth, meleeMaxHealth, t);
			yield return null;
		}
		meleeHealth = meleeMaxHealth;
	}

	public IEnumerator RegenerateDefenderHealth ()
	{
		float t = 0f, time = 0f;
		while (defenderHealth < defenderMaxHealth) {
			time += Time.deltaTime;
			t = time / healthRegenRate;
			defenderHealth = Mathf.Lerp (defenderHealth, defenderMaxHealth, t);
			yield return null;
		}
		defenderHealth = defenderMaxHealth;
	}

	public IEnumerator RegenerateRangerHealth ()
	{
		float t = 0f, time = 0f;
		while (rangerHealth < rangerMaxHealth) {
			time += Time.deltaTime;
			t = time / healthRegenRate;
			rangerHealth = Mathf.Lerp (rangerHealth, rangerMaxHealth, t);
			yield return null;
		}
		rangerHealth = rangerMaxHealth;
	}
}
