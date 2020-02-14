using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AT_DefenderPlayerController : AT_Controller, Blocking, Attacking
{
    public float RollDistance { get { return rollDistance; } }
    public Transform MyTransform;
    public Rigidbody MyRigidbody;
    public CapsuleCollider MyCapsuleCollider { get; set; }
	public BlockObject blockObject;

    private static AT_DefenderPlayerController instance;
    public static AT_DefenderPlayerController Instance
    {
        get
        {
            if (!instance) { instance = GameObject.FindObjectOfType<AT_DefenderPlayerController>(); }
            return instance;
        }
    }

    // Use this for initialization
    public override void Start()
    {
        base.Start();
        IsDead = false;
        MyCapsuleCollider = GetComponent<CapsuleCollider>();
    }

    protected override void HandleActionInput()
    {
        if (!animator.IsAttacking)
        {
            /* Defender roll is not yet implemented, animations need to be added.
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
            */

            if (!animator.IsBlocking)
            {
                if (Input.GetKeyDown(KeyCode.Mouse0) || Input.GetKey(KeyCode.Mouse0))
                {
                    Attack(1);
                    animator.Attack(1);                    
                }
                    /* Defender has one attack for now, this can be uncommented when the other 
                     * attacks and combos are implemented.
                    // tapping or holding (Mouse0 = left click)
                    if (Input.GetKeyDown(KeyCode.Mouse0) || Input.GetKey(KeyCode.Mouse0))
                    {
                        // ComboHandler() handles the consecutive attacks and attack speed timers
                        hasCooldownTimerStarted = true;
                        if (isConsecutiveAttack)
                        {
                            currentConsecutiveTimer++;
                            currentComboState++;
                            if (currentComboState >= 4)
                            {
                                int attack = Random.Range(4, 6);
                                animator.Attack(attack);
                            }
                            else
                            {
                                int attack = Random.Range(1, 4);
                                animator.Attack(attack);
                            }
                        }
                        else
                        {
                            int attack = Random.Range(1, 3);
                            animator.Attack(attack);
                        }
                    }
                    */
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

    public void Attack(int attack)
    {
        leftWeapon.gameObject.SetActive(true);
        rightWeapon.gameObject.SetActive(true);
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
