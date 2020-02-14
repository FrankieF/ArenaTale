using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ActiveUpgrade : MonoBehaviour {
    private const int SIZE = 7; // modify the size whenever adding or deleting a new active upgrade
    private string selectedUpgrade; // stores an upgrade when its repective button is clicked
    private ColorBlock setColor; // stores and updates the color for activeUpgradeButton

    public string[] activeUpgrades = new string[] {"Fire","Ice","Poison","Trap",
        "Regeneration","Explosion","Critical"};  // stores current and future upgrades
    public ColorBlock[] upgradeColors = new ColorBlock[SIZE]; // stores all colors for each of the upgrades
    public string currentUpgrade; // stores the currently equipped upgrade
    public Button[] upgradeButtons = new Button[SIZE]; // stores the button for each upgrade
    public Button activeUpgradeButton;  // main button that indicates the selected upgrade
    public Text upgradeText; // displays the text for the currently equipped upgrade
    public int index; // stores the index for the current upgrade

    public bool activateUpgrade = false; // Indicates when an upgrade is taking effect on an enemy
    public bool canUseUpgrade = true; // Indicates if player is able to use an upgrade
    public bool stopUpgrade = false; // Indicates the timer when to stop using the upgrade
    public bool firstActivation = true; // Allows the upgrade effects to run once and for a specific amount of time

    public float cooldownTimer = 5.0f; // Time the upgrade takes effect on an enemy
    public float waitTime = 10.0f; // Time the player must wait to use an upgrade again
    public float upgradeDamage = 2.0f; // Adjusts the upgrade damage effect over time

    // Use this for initialization
    void Start () {
        // Initialize all upgrade button colors and add a listener to know when the upgrade button was clicked 
        for (int i=0;i<upgradeButtons.Length;i++) {
           // upgradeColors[i] = upgradeButtons[i].colors;
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Keypad1))
            OnKeyPress(upgradeButtons[0].name);
        if (Input.GetKeyDown(KeyCode.Keypad2))
            OnKeyPress(upgradeButtons[1].name);
        if (Input.GetKeyDown(KeyCode.Keypad3))
            OnKeyPress(upgradeButtons[2].name);
        if (Input.GetKeyDown(KeyCode.Keypad4))
            OnKeyPress(upgradeButtons[3].name);
        if (Input.GetKeyDown(KeyCode.Keypad5))
            OnKeyPress(upgradeButtons[4].name);
        if (Input.GetKeyDown(KeyCode.Keypad6))
            OnKeyPress(upgradeButtons[5].name);
        if (Input.GetKeyDown(KeyCode.Keypad7))
            OnKeyPress(upgradeButtons[6].name);
        CoolDown();
        WaitForUpgrade();
        AttackVisuals();
    }

    // Indicate the selected upgrade name when a click occurs to its respective button
    void OnKeyPress(string upgrade)
    {
        selectedUpgrade = upgrade;
        SwitchUpgrade(selectedUpgrade);
    }

    /// <summary>
	/// Passes an index number according to the current upgrade
	/// </summary>
    /// /// <param name="upgrade">Indicates which upgrade is currently selected.</param>
    public void SwitchUpgrade(string upgrade)
    {
        switch (upgrade)
        {
            case "FireUpgradeButton":
                SetActiveUpgrade(0);
                break;
            case "IceUpgradeButton":
                SetActiveUpgrade(1);
                break;
            case "PoisonUpgradeButton":
                SetActiveUpgrade(2);
                break;
            case "TrapUpgradeButton":
                SetActiveUpgrade(3);
                break;
            case "RegenerationUpgradeButton":
                SetActiveUpgrade(4);
                break;
            case "ExplosionUpgradeButton":
                SetActiveUpgrade(5);
                break;
            case "CriticalUpgradeButton":
                SetActiveUpgrade(6);
                break;
            default:
                break;
        }
    }

    /// <summary>
	/// Change the text and color for the Active Upgrade Button according to the selected upgrade
	/// </summary>
    /// /// <param name="x">Obtains and sets all colors and text according to the index of the selected upgrade.</param>
    public void SetActiveUpgrade(int x)
    {
        index = x;
        currentUpgrade = activeUpgrades[x];
        upgradeText.text = "Active Upgrade: " + currentUpgrade.ToUpper();
        SetColor(index,"normal", Color.black);
    }

    /// <summary>
    /// Timer that allows upgrade's effects for a specific amount of time
    /// </summary>
    public void CoolDown()
    {
        if (canUseUpgrade && activateUpgrade)
        {
            cooldownTimer -= Time.deltaTime;

            if (cooldownTimer <= 0)
            {
                SetColor(index,"disabled",Color.black);
                currentUpgrade = "";
                canUseUpgrade = false;
                activateUpgrade = false;
                stopUpgrade = true;
                cooldownTimer = 5.0f;
                CancelInvoke();
            }
        }
    }

    /// <summary>
    /// Timer that stops an upgrade from being used for a specific amount of time
    /// </summary>
    public void WaitForUpgrade()
    {
        if (stopUpgrade)
        {
            waitTime -= Time.deltaTime;

            if (waitTime <= 0)
            {
                SetColor(index,"normal",Color.black);
                currentUpgrade = activeUpgrades[index];
                canUseUpgrade = true;
                stopUpgrade = false;
                firstActivation = true;
                waitTime = 10.0f;
            }
        }
    }

    /// <summary>
    /// Executes an upgrade's effect over time (executes every second)
    /// </summary>
    public void ExecuteUpgrade()
    {
        if ((currentUpgrade=="Fire") && canUseUpgrade && firstActivation)
        {
            InvokeRepeating("UpgradeAttack", 0.0f, 1.0f);
            firstActivation = false;
        }
    }

    /// <summary>
    /// Decreases the enemy health by a set amount of damage
    /// </summary>
    public void UpgradeAttack()
    {
    }

    /// <summary>
    /// Displays all visual effects on the UI, enemy and player depending upon the upgrade
    /// </summary>
    public void AttackVisuals()
    {
        if (activateUpgrade && !firstActivation)
        {
            Color tempColor01 = upgradeColors[index].highlightedColor;
            Color tempColor02 = upgradeColors[index].pressedColor;
            Color altColor = Color.Lerp(tempColor01, tempColor02, Mathf.PingPong(Time.time*2,1));
            SetColor(index,currentUpgrade,altColor);
            Color matColor = Color.Lerp(Color.black, tempColor01, Mathf.PingPong(Time.time*2,1));
        }
    }

    /// <summary>
    /// Updates the UI button colors based on the type of upgrade and if it's currently taking effect
    /// <param name="i">index for the current upgrade.</param>
    /// <param name="color">set button color to any of these: normal,disabled or "upgrade name".</param>
    /// <param name="special">a specific color that can be used in a button.</param>
    /// </summary>
    public void SetColor(int i,string color,Color specialColor)
    {
        setColor.fadeDuration = 0.2f;
        if(color=="normal")
        {
            setColor.normalColor = upgradeColors[i].normalColor;
            setColor.disabledColor = upgradeColors[i].normalColor;
        }
        if(color=="disabled")
        {
            setColor.normalColor = upgradeColors[i].disabledColor;
            setColor.disabledColor = upgradeColors[i].disabledColor;
        }
        if(color=="Fire")
        {
            setColor.normalColor = specialColor;
            setColor.disabledColor = specialColor;
        }
        setColor.highlightedColor = upgradeColors[i].highlightedColor;
        setColor.pressedColor = upgradeColors[i].pressedColor;
        setColor.colorMultiplier = 1;
        activeUpgradeButton.colors = setColor;
    }
}
