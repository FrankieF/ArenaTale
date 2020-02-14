using UnityEngine;
using System.Collections;

public class AT_Animator : MonoBehaviour
{
    public AT_Controller playerController;
    public AT_Motor motor;

    public enum Movement
    {
        Stationary,
        Forward
    }

    public enum CharacterState
    {
        Idle,
        Running
    }

    public enum CharacterActionState
    {
        NoAction,
        Sliding,
        RollingRight,
        RollingLeft,
        Attack,
        Dead,
        Block,
        BlockDamaged,
        Hit,
        ActionLocked
    }

    public CharacterState LastState;
    public Vector3 intialPosition = Vector3.zero;
    private Quaternion intialRotation = Quaternion.identity;

    public Movement MoveDirection;

    public CharacterState State;

    public CharacterActionState ActionState;

    public bool IsDead;

    public bool IsActionLocked;

    public bool IsBlocking;

    public bool IsMoving;

    public bool IsRolling;

    public bool IsAttacking;

    public bool IsHit;

    public float time = 0f;
    public float hitTime = .5f;


    void Awake()
    {
        intialPosition = transform.position;
        intialRotation = transform.rotation;
        playerController = GetComponent<AT_Controller>();
        motor = GetComponent<AT_Motor>();

    }

    void Update()
    {
        DetermineCurrentState();
        ProcessCurrentStates();
    }

    public void DetermineCurrentMoveDirection()
    {
        if (!IsRolling)
        {
            var moving = false;

            if (motor.MoveVector.z > 0 || motor.MoveVector.z < 0 ||
                motor.MoveVector.x > 0 || motor.MoveVector.x < 0)
                moving = true;

            if (moving)
            {
                MoveDirection = Movement.Forward;
                IsMoving = true;
            }
            else
            {
                MoveDirection = Movement.Stationary;
                IsMoving = false;
            }
        }
    }

    void DetermineCurrentState()
    {
        if (ActionState == CharacterActionState.Dead)
            return;

        if (ActionState != CharacterActionState.Sliding)
        {
            switch (MoveDirection)
            {
                case Movement.Stationary:
                    State = CharacterState.Idle;
                    break;
                case Movement.Forward:
                    State = CharacterState.Running;
                    break;
            }
        }
    }

    void ProcessCurrentStates()
    {
        switch (State)
        {
            case CharacterState.Idle:
                Idle();
                break;
            case CharacterState.Running:
                Running();
                break;
        }

        switch (ActionState)
        {
            case CharacterActionState.Sliding:
                Sliding();
                break;
            case CharacterActionState.RollingLeft:
                RollingLeft();
                break;
            case CharacterActionState.RollingRight:
                RollingRight();
                break;
            case CharacterActionState.Attack:
                Attacking();
                break;
            case CharacterActionState.Block:
                Blocking();
                break;
            case CharacterActionState.BlockDamaged:
                BlockDamage();
                break;
            case CharacterActionState.Hit:
                HitAction();
                break;
            case CharacterActionState.Dead:
                Dead();
                break;
            case CharacterActionState.ActionLocked:
                ActionLocking();
                break;
        }

    }

    #region Character State Methods

    void Idle()
    {
        //playerController.MyAnimator.SetBool("Moving", false);
    }

    void Running()
    {
        //playerController.MyAnimator.SetBool("Moving", true);
    }

    void Using()
    {
        if (!GetComponent<Animation>().isPlaying)
        {
            State = CharacterState.Idle;
        }
    }

    void Sliding()
    {
        if (!motor.IsSliding)
        {
            State = CharacterState.Idle;
        }
    }

    void ActionLocking()
    {

    }

    void Blocking()
    {
        if (!IsBlocking)
        {
            ActionState = CharacterActionState.NoAction;
        }
    }

    void BlockDamage()
    {
        time += Time.deltaTime;
        if (time > hitTime)
        {
           // if (playerController.canBlock)
           // {
                playerController.MyAnimator.SetBool("BlockDamaged", false);
          //      ActionState = CharacterActionState.Block;
          //  }
          //  else
        //    {
                playerController.MyAnimator.SetBool("BlockBroken", false);
                ActionState = CharacterActionState.NoAction;
        //    }
            time = 0f;
        }
    }

    void RollingLeft()
    {
        if (!IsRolling)
        {
            ActionState = CharacterActionState.NoAction;
        }
        else
        {
            Vector3 direction = -playerController.transform.right * playerController.GetRollSpeed() * Time.deltaTime * playerController.GetRollDistance();
            playerController.GetCharacterController().Move(direction);
        }
    }

    void RollingRight()
    {
        if (!IsRolling)
        {
            ActionState = CharacterActionState.NoAction;
        }
        else
        {
            Vector3 direction = playerController.transform.right * playerController.GetRollSpeed() * Time.deltaTime * playerController.GetRollDistance();
            playerController.GetCharacterController().Move(direction);
        }
    }

    void Attacking ()
    {
        if (!IsAttacking)
        {
            ActionState = CharacterActionState.NoAction;
            playerController.MyAnimator.SetInteger("Attack", 0);
        }
    }

    void HitAction()
    {
        if (!IsHit)
        {
            playerController.MyAnimator.SetBool("Hit", false);
            ActionState = CharacterActionState.NoAction;
        }
    }

    void Dead()
    {
        ActionState = CharacterActionState.Dead;
    }

    #endregion

    #region Start Action Methods

    public void Slide()
    {
        ActionState = CharacterActionState.Sliding;
    }

    public void Run(float speed)
    {
        playerController.MyAnimator.SetFloat("Speed", speed);
    }

    public void Attack(int attack)
    {
        playerController.MyAnimator.SetInteger("Attack", attack);
        ActionState = CharacterActionState.Attack;
        IsAttacking = true;
    }

    public void Block()
    {
        ActionState = CharacterActionState.Block;
        IsBlocking = true;
        playerController.MyAnimator.SetBool("Block", true);
    }

    public void StopBlock()
    {
        ActionState = CharacterActionState.NoAction;
        IsBlocking = false;
        playerController.MyAnimator.SetBool("Block", false);
    }

    public void BlockDamaged()
    {
        ActionState = CharacterActionState.BlockDamaged;
        playerController.MyAnimator.SetBool("BlockDamaged",true);
        playerController.MyAnimator.SetBool("Block", false);
    }

    public void BlockBroken()
    {
        ActionState = CharacterActionState.BlockDamaged;
        playerController.MyAnimator.SetBool("BlockBroken",true);
        playerController.MyAnimator.SetBool("Block", false);
        IsBlocking = false;
        ((Blocking)playerController).BlockBroken();
    }

    public void Hit()
    {
        if (!IsHit)
        {
            ActionState = CharacterActionState.Hit;
            playerController.MyAnimator.SetBool("Hit",true);
            IsHit = true;
        }
    }

    public void Roll(string direction)
    {
        string roll = "Roll" + direction;
        playerController.MyAnimator.SetTrigger(roll);
        IsRolling = true;
        ActionState = direction.Equals("Left") ? CharacterActionState.RollingLeft : CharacterActionState.RollingRight;
        
    }

    public void ActionLocked()
    {
        ActionState = CharacterActionState.ActionLocked;
        IsActionLocked = true;
    }

    public void Die()
    {
        // Intialize everything we need to die
        IsDead = true;
        Dead();
    }

    public void Reset()
    {
        // Reset character so we can play again
        IsDead = false;
        transform.position = intialPosition;
        transform.rotation = intialRotation;
        State = CharacterState.Idle;
    }

    #endregion
}