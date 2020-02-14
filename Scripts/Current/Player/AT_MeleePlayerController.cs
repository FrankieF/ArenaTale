using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AT_MeleePlayerController : AT_Controller, Blocking, Attacking
{
    public float RollDistance { get { return rollDistance; } }

    public Transform MyTransform;

    public Rigidbody MyRigidbody;

    public CapsuleCollider MyCapsuleCollider { get; set; }

    public Transform PlayerWeaponLocation { get { return playerWeaponLocation; } set { playerWeaponLocation = value; } }

    public BlockObject blockObject;

    private static AT_MeleePlayerController instance;

    public static AT_MeleePlayerController Instance
    {
        get
        {
            if (!instance)
            {
                instance = GameObject.FindObjectOfType<AT_MeleePlayerController>();
            }
            return instance;
        }
    }

    // Use this for initialization
    public override void Start()
    {
        base.Start();
        IsDead = false;
        MyAnimator = GetComponent<Animator>();
        //startingPlayerWeaponLocation = playerWeapon.GetComponent<Transform>();
        //playerWeaponLocation = startingPlayerWeaponLocation;

        //startingPlayerWeaponBox = playerWeapon.GetComponent<BoxCollider>();
        //playerWeaponBox = startingPlayerWeaponBox;
    }

    protected override void HandleActionInput()
    {
        if (!animator.IsAttacking)
        {
            // MeleeRollLeftBehavior.cs will handle the rest of this animation
            if (Input.GetKeyDown(KeyCode.Q))
            {
                animator.Roll("Left");
            }
            // MeleeRollRightBehavior.cs will handle the rest of this animation
            if (Input.GetKeyDown(KeyCode.E))
            {
                animator.Roll("Right");
            }

            if (!animator.IsBlocking)
            {
                // tapping or holding (Mouse0 = left click)
                if (Input.GetKeyDown(KeyCode.Mouse0) || Input.GetKey(KeyCode.Mouse0))
                {
                    int attack = 0;
                    // ComboHandler() handles the consecutive attacks and attack speed timers
                    hasCooldownTimerStarted = true;
                    if (isConsecutiveAttack)
                    {
                        currentConsecutiveTimer++;
                        currentComboState++;
                        if (currentComboState >= 4)
                        {
                            attack = Random.Range(4, 6);
                            Attack(attack);
                            animator.Attack(attack);
                        }
                        else
                        {
                            attack = Random.Range(1, 4);
                            Attack(attack);
                            animator.Attack(attack);
                        }
                    }
                    else
                    {
                        attack = Random.Range(1, 3);
                        Attack(attack);
                        animator.Attack(attack);
                    }
                }
            }
            if (canBlock)
            {
                if (Input.GetKeyDown(KeyCode.Mouse1))
                {
                    animator.Block();
                }
                else if (Input.GetKeyUp(KeyCode.Mouse1))
                {
                    animator.StopBlock();
                }
            }
        }
    }

    public override void ComboHandler()
    {
        base.ComboHandler();
    }

    public void OnTriggerEnter(Collider other)
    {
        
    }

    public override void AttackingEnemy()
    {
        base.AttackingEnemy();
    }

    public void Attack(int attack)
    {
        switch (attack)
        {
            case 2:
                leftWeapon.gameObject.SetActive(true);
                break;
            case 1:
            case 3:
                rightWeapon.gameObject.SetActive(true);
                break;
            case 4:
            case 5:
                leftWeapon.gameObject.SetActive(true);
                rightWeapon.gameObject.SetActive(true);
                break;
        }
    }

    public void AttackFinished()
    {
        leftWeapon.gameObject.SetActive(false);
        rightWeapon.gameObject.SetActive(false);
        animator.IsAttacking = false;
    }

    public void BlockBroken()
    {
        StartCoroutine(RegenerateBlockHealth());
    }

    public IEnumerator RegenerateBlockHealth()
    {
        float time = 0f;
        while (blockHealth < maxBlockHealth)
        {
            time += Time.deltaTime;
            blockHealth = Mathf.Lerp(blockHealth, maxBlockHealth, time * .5f / blockHealthRegeneration);
            yield return null;
        }
        canBlock = true;
    }
}