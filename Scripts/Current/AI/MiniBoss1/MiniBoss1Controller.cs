using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MiniBoss1Controller : AT_Enemy
{
    // allows for the boss's health to be set in the editor
    public float maxBossHealth = 100f;
    public AudioSource takingHitSound;
    public ParticleSystem takingRangedHitParticle;

    public float idleTimer;
    public float idleDuration;
    public float blockingHealthRate;

    public bool isIdle = true;
    public bool isWalking = false;
    public bool isRunning = false;
    public bool isBlocking = false;
    public bool isBlockingSecond = false;
    public bool isBlockingThird = false;
    
    public bool IsAttackOneCombo { get; set; }
    public bool IsAttackLeft { get; set; }
    public bool IsAttackRight { get; set; }
    public bool IsAttackTwoCombo { get; set; }
    public bool IsRandomAttacking { get; set; }
    public bool IsShootingLeft { get; set; }
    public bool TakesDamage { get; set; }
    public bool CanTakeDamage { get; set; } //Indicates if the enemy can currently take damage
    public bool IsHeavyHit { get; set; }
    public bool HasCooldownTimerStarted { get; set; }
    public int ShootingCount { get; set; }
    public int RandomAttack1Value { get; set; }
    public int AttackLeftComboState { get; set; }
    public int AttackRightComboState { get; set; }

    public float MaxBossHealth
    {
        get { return maxBossHealth; }
        set
        {
            this.maxBossHealth = value;
            // if we use hp bars, BarScript bar, bar.MaxValue = maxBossHealth;
        }
    }

    private static MiniBoss1Controller instance;
    public static MiniBoss1Controller Instance
    {
        get
        {
            if (!instance) { instance = GameObject.FindObjectOfType<MiniBoss1Controller>(); }
            return instance;
        }
    }

    // Use this for initialization
    public override void Start ()
    {
        base.Start();
        ShootingCount = 1;
        AttackLeftComboState = 1;
        AttackRightComboState = 1;
        IsRandomAttacking = false;
        IsShootingLeft = true;
        MyAnimator = GetComponent<Animator>();
        agent.speed = movementSpeed;
        this.MaxBossHealth = maxBossHealth;
        target = GameObject.Find("Melee"); // Change to game manager
        stateMachine = GetComponent<StateMachine>();
        stateMachine.SetValues(ChaseState.Instance, null, this.gameObject);
    }
	
	/// <summary>
    /// Anything that is going to be checked often is put in update
    /// </summary>
	private void Update ()
    {
        stateMachine.Execute();
        //Idle();
        CooldownTimerHandler();
        //BlockHandler();
       // if (!isBlocking) { AttackHandler(); }
	}

    /// <summary>
    /// Any physics stuff is handled in fixed update
    /// </summary>
    private void FixedUpdate()
    {
        //SwitchingFollowingTarget();
        Vector3 direction = target.transform.position - transform.position;
        direction.Normalize();
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), Time.deltaTime * 10f);
    }

    /// <summary>
    /// if the AI is not moving
    /// </summary>
    public override void Idle()
    { 
        if (isIdle)
        {
            MyAnimator.SetBool("isWalking", false);
            idleDuration = UnityEngine.Random.Range(1, 10); // using UnityEngine's random function
            MyAnimator.SetTrigger("idleTurning");
            idleTimer += Time.deltaTime;
            if (idleTimer >= idleDuration)
            {
                isIdle = false;
                isWalking = true;
                MyAnimator.ResetTrigger("idleTurning");
                MyAnimator.SetBool("isWalking", true);
            }
        }
        
    }

    /// <summary>
    /// if the AI is moving towards the player
    /// </summary>
    public override void SwitchingFollowingTarget()
    {
        if (isWalking || isRunning)
        {
                WalkingAndRunningHandler();
                agent.SetDestination(target.transform.position);
        }

    }

    /// <summary>
    /// Transitions the AI from walking to running
    /// </summary>
    public override void WalkingAndRunningHandler()
    {
        Debug.Log("movement speed: " + movementSpeed);
        float distance = Vector3.Distance(target.transform.position, transform.position);
        if (distance <= startRunningDistance)
        {
            MyAnimator.SetBool("isWalking", false);
            MyAnimator.SetBool("isRunning", true);
            isWalking = false;
            isRunning = true;
            agent.speed = movementSpeed * 2f;
        }
        if (distance > startRunningDistance)
        {
            MyAnimator.SetBool("isWalking", true);
            MyAnimator.SetBool("isRunning", false);
            isWalking = true;
            isRunning = false;
            agent.speed = movementSpeed;
        }
    }

    /// <summary>
    /// Starts the attack based off of what current animation they are in
    /// </summary>
    public override void AttackHandler()
    {
        float distance = Vector3.Distance(target.transform.position, transform.position);
        if (distance <= attackRange)
        {
            isAttacking = true;

            if (isIdle)
            {
                MyAnimator.SetTrigger("idleStandUp");
                ComboHandler();
            }
            if (isWalking)
            {
                MyAnimator.SetTrigger("walkStandUp");
                ComboHandler();
            }
            if (isRunning)
            {
                MyAnimator.SetTrigger("runStandUp");
                ComboHandler();
            }

        } 

        if (isAttacking && distance > attackRange)
        {
            isAttacking = false;
            Idle();
        }
       
    }

    /// <summary>
    /// Handles the combos for the AI which calls the MiniBoss1/Behaviors/Scripts
    /// </summary>
    private void ComboHandler()
    {
        //if (RandomAttack1Value < randomAttack1)
        //{
        //    MyAnimator.SetTrigger("Attack1");
        //}

        if (!IsRandomAttacking && IsShootingLeft && ShootingCount % 4 != 0)
        {
            MyAnimator.SetTrigger("attackLeftArm");
        }

        if (!IsRandomAttacking && !IsShootingLeft && ShootingCount % 4 != 0)
        {
            MyAnimator.SetTrigger("attackRightArm");
        }

        if (!IsRandomAttacking && ShootingCount % 4 == 0)
        {
            MyAnimator.SetTrigger("Attack2");
        }
    }

    /// <summary>
    /// Handles the blocking animations for the AI
    /// </summary>
    private void BlockHandler()
    {

        //if the boss is not blocking yet and if his health is between the right range of health % then start blocking
        //if (!isBlocking && !isBlockingSecond && !isBlockingThird)
        //{
        //    if (((CurrentBossHealth / MaxBossHealth) <= .7f) && ((CurrentBossHealth / MaxBossHealth) > .4f))
        //    {
        //        MyAnimator.ResetTrigger("BlockDamaged");
        //        MyAnimator.SetTrigger("Block");
        //        isBlocking = true;
        //    }
        //}
        //if the boss is not blocking yet and if his health is between the right range of health % then start blocking
        //if (isBlocking && !isBlockingSecond && !isBlockingThird)
        //{
        //    if (((CurrentBossHealth / MaxBossHealth) <= .4f) && ((CurrentBossHealth / MaxBossHealth) > .1f))
        //    {
        //        MyAnimator.ResetTrigger("BlockDamaged");
        //        MyAnimator.SetTrigger("Block");
        //        isBlockingSecond = true;
        //    }
        //}
        //if the boss is not blocking yet and if his health is between the right range of health % then start blocking
        //if (isBlocking && isBlockingSecond && !isBlockingThird)
        //{
        //    if (((CurrentBossHealth / MaxBossHealth) <= .1f) && ((CurrentBossHealth / MaxBossHealth) > 0))
        //    {
        //        MyAnimator.ResetTrigger("BlockDamaged");
        //        MyAnimator.SetTrigger("Block");
        //        isBlockingThird = true;
        //    }
        //}
        // if the boss takes damage then trigger the animation
        if (TakesDamage)
        {
            if (isBlocking && !isBlockingSecond && !isBlockingThird)
            {
                MyAnimator.SetTrigger("BlockDamaged");
            }
            if (isBlocking && isBlockingSecond && !isBlockingThird)
            {
                MyAnimator.SetTrigger("BlockDamaged");
            }
            if (isBlocking && isBlockingSecond && isBlockingThird)
            {
                MyAnimator.SetTrigger("BlockDamaged");
            }
        }
        // if the boss takes a heavy hit then break his block and trigger animation
        if (IsHeavyHit)
        {
            if (isBlocking && !isBlockingSecond && !isBlockingThird)
            {
                MyAnimator.SetTrigger("BlockBreak");
                MyAnimator.ResetTrigger("BlockDamaged");
                MyAnimator.ResetTrigger("Block");
            }
            if (isBlocking && isBlockingSecond && !isBlockingThird)
            {
                MyAnimator.SetTrigger("BlockBreak");
                MyAnimator.ResetTrigger("BlockDamaged");
                MyAnimator.ResetTrigger("Block");
            }
            if (isBlocking && isBlockingSecond && isBlockingThird)
            {
                MyAnimator.SetTrigger("BlockBreak");
                MyAnimator.ResetTrigger("BlockDamaged");
                MyAnimator.ResetTrigger("Block");
            }
        }
    }

    private void CooldownTimerHandler()
    {
        // Starts the attackspeed cooldown
        if (HasCooldownTimerStarted)
        {
            currentCooldownTimer -= Time.deltaTime;
            if (currentCooldownTimer <= 0)
            {
                HasCooldownTimerStarted = false;
                currentCooldownTimer = 1f;
            }
        }
    }

    public void GetShootingPosition()
    { }

}
