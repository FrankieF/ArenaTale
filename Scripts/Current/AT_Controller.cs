using UnityEngine;
using System.Collections;

public class AT_Controller : AT_Entity
{

    public AT_Animator animator;
    public AT_Motor motor;
    public float rollDistance = 6f;
    public float rollSpeed = 2f;
    public GameObject cameraTarget;
    public int experiencePoints;

    public Transform playerWeaponLocation;
    public BoxCollider startingPlayerWeaponBox;
    public PlayerWeapon leftWeapon;
    public PlayerWeapon rightWeapon;

    protected ActiveUpgrade activeUpgrade;

    /** Player Attributes **/
    public float attribtePoints = 5;
    public int[] statLevels = { 5, 5, 5, 5, 5, 5, 5 };
    public PlayerAttributes attributes;
    public ActiveUpgrade[] upgrades;

    void Awake()
    {
        CharacterController = GetComponent("CharacterController") as CharacterController;
    }

    public virtual void Start()
    {
        activeUpgrade = GetComponent<ActiveUpgrade>();
        SetWeaponDamage(attackDamage);
    }

    void Update()
    {
        motor.ResetMotor();

        if (animator.ActionState != AT_Animator.CharacterActionState.ActionLocked)
        {
            GetLocomotionInput();
            HandleActionInput();
            ComboHandler();
        }
        else if (animator.IsDead)
        {
            if (Input.anyKeyDown)
                animator.Reset();
        }

        motor.UpdateMotor();
    }

    protected virtual void HandleActionInput()
    {

    }

    public virtual void ComboHandler()
    {
        // Starts the attackspeed cooldown
        if (hasCooldownTimerStarted)
        {
            currentCooldownTimer -= Time.deltaTime;
            if (currentCooldownTimer <= 0)
            {
                hasCooldownTimerStarted = false;
                currentCooldownTimer = attackSpeed;
            }
        }

        // Checks if the player is doign consecutive attacks
        currentConsecutiveTimer -= Time.deltaTime;
        if (currentConsecutiveTimer >= 0)
        {
            isConsecutiveAttack = true;
        }
        else if (currentConsecutiveTimer < 0)
        {
            currentComboState = 0;
            currentConsecutiveTimer = 1.1f;
            isConsecutiveAttack = false;
        }
    }

    void GetLocomotionInput()
    {
        var vertical = Input.GetAxis("Vertical");
        var horizontal = Input.GetAxis("Horizontal");
        Vector3 direction = new Vector3();
        if (vertical > 0)
            direction += Vector3.forward;
        else if (vertical < 0)
            direction += Vector3.back;
        if (horizontal > 0)
            direction += Vector3.right;
        else if (horizontal < 0)
            direction += Vector3.left;
        motor.MoveVector += transform.InverseTransformDirection(direction);
        float speed =  vertical != 0 ? Mathf.Abs(vertical) : Mathf.Abs(horizontal);
        animator.Run(speed);
        animator.DetermineCurrentMoveDirection();
    }

    public override void TakeDamage(float damage)
    {
        if (!isTakingDamage)
        {
            isTakingDamage = true;
            bool takeDamage = true;

            if (animator.IsBlocking)
            {
                blockHealth -= damage;
                if (blockHealth > 0)
                {
                    takeDamage = false;
                    animator.BlockDamaged();
                }
                else
                {
                    animator.BlockBroken();
                    canBlock = false;
                }
            }
            if (DEBUG)
                Debug.LogFormat("{0} Taking damage: {1}, damage taken: {2}", name, takeDamage, damage);
            if (takeDamage)
            {
                health -= damage;
               // animator.Hit();
            }
            isTakingDamage = false;
        }
    }

    /// <summary>
    /// Sets all the stats to zero before loading new stats in.
    /// </summary>
    public void ZeroOutStatsForLoad()
    {
        health = 0f;
        healthRegenSpeed = 0f;
        moveSpeed = 0f;
        rollDistance = 0f;
        attackSpeed = 0f;
        attackDamage = 0;
        blockHealth = 0;
        blockHealthRegeneration = 0;
        attribtePoints = 0;
    }

    public CharacterController GetCharacterController()
    {
        return this.CharacterController;
    }

    public float GetRollDistance()
    {
        return this.rollDistance;
    }

    public void SetRollDistance(float rollDistance)
    {
        this.rollDistance = rollDistance;
    }

    public float GetRollSpeed()
    {
        return this.rollSpeed;
    }

    public void SetRollSpeed(float rollSpeed)
    {
        this.rollSpeed = rollSpeed;
    }

    public int GetExperiencePoints()
    {
        return this.experiencePoints;
    }

    public void SetExperiencePoints(int experience)
    {
        if (DEBUG)
            Debug.Log("Adding experience points: " + experience);
        if (experience < 0)
            Debug.LogError("ERROR: Negative experience points added.");
        experiencePoints += experience;
    }

    /// <summary>
    /// Gets the attribute points.
    /// </summary>
    /// <returns>The attribute points.</returns>
    public float GetAttributePoints()
    {
        return this.attribtePoints;
    }

    /// <summary>
    /// Sets the attribute points.
    /// </summary>
    /// <param name="number">The amount of attribute points to add.</param>
    public void SetAttributePoints(float number)
    {
        this.attribtePoints += number;
    }

    /// <summary>
    /// The array representing each level of the stat; level as in how many times it has been upgraded.
    /// </summary>
    /// <returns>The stat levels.</returns>
    public int[] GetStatLevels()
    {
        return this.statLevels;
    }

    /// <summary>
    /// Changes a level of a stat.
    /// </summary>
    /// <param name="stat">Stat - The state to change..</param>
    /// <param name="level">Level - The new level of the stat.</param>
    public void SetStatLevel(int stat, int level)
    {
        if (level < 0)
            Debug.Log("Stats cannot go below zero!");
        else if (stat < 0 || stat >= statLevels.Length)
            Debug.Log("The stat you are trying to change is out of bounds!");
        else
            statLevels[stat] = level;
    }

    public void SetStatsLevels(int[] stats)
    {
        if (stats.Length == statLevels.Length)
            this.statLevels = stats;
    }

    public void SetWeaponDamage(float damage)
    {
        leftWeapon.damage = damage;
        rightWeapon.damage = damage;
    }
}
