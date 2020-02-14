using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AT_RangerPlayerController : AT_Controller
{
    public float RollDistance { get { return rollDistance; } }
    public Transform MyTransform;
    public Rigidbody MyRigidbody;
    public CapsuleCollider MyCapsuleCollider { get; set; }
    public Transform PlayerWeaponLocation { get { return playerWeaponLocation; } set { playerWeaponLocation = value; } }

    private static AT_RangerPlayerController instance;
    public static AT_RangerPlayerController Instance
    {
        get
        {
            if (!instance) { instance = GameObject.FindObjectOfType<AT_RangerPlayerController>(); }
            return instance;
        }
    }

    // Use this for initialization
    public override void Start()
    {
        base.Start();
        IsDead = false;
        MyCapsuleCollider = GetComponent<CapsuleCollider>();

        //startingPlayerWeaponLocation = playerWeapon.GetComponent<Transform>();
        //playerWeaponLocation = startingPlayerWeaponLocation;
    }

    protected override void HandleActionInput()
    {
        if (!animator.IsAttacking)
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                animator.Roll("Left");
            }
            if (Input.GetKeyDown(KeyCode.E))
            {
                animator.Roll("Right");
            }

            // tapping or holding (Mouse0 = left click)
            if ((Input.GetKeyDown(KeyCode.Mouse0)) || (Input.GetKey(KeyCode.Mouse0)))
            {
                if (isConsecutiveAttack)
                {
                    currentConsecutiveTimer++;
                    currentComboState++;
                    if (currentComboState >= 4)
                    {
                        int attack = Random.Range(3,4);
                        animator.Attack(attack);
                    }
                    else
                    {
                        // Checks the current combo counter from the Ranger to decide which arm to spawn the bullet on
                        bool left = (currentComboState % 4) != 0;
                        int attack = left ? 1 : 0;
                        animator.Attack(attack);
                    }
                }
                else
                {
                    bool left = (currentComboState % 4) != 0;
                    int attack = left ? 1 : 0;
                    animator.Attack(attack);
                }
            }
        }
    }

    public override void ComboHandler()
    {
        base.ComboHandler();
    }

	public float GetCurrentComboState ()
	{
		return currentComboState;
	}

    public void OnTriggerEnter(Collider other)
    {
        
    }
}
