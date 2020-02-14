using UnityEngine;
using System.Collections;

public class AT_Motor : MonoBehaviour
{
    public AT_Animator animator;
    public AT_Controller controller;

    public float TerminalVelocity = 20f;
    public float SlideThreshold = 0.6f;
    public float MaxControllableSlideMagnitude = 0.4f;
    public float Gravity = 21f;

    private Vector3 slideDirection;

    public Vector3 MoveVector { get; set; }
    public float VerticalVelocity { get; set;}
    public bool IsSliding { get; set; }

    public void Awake()
    {
        animator = GetComponent<AT_Animator>();
        controller = GetComponent<AT_Controller>();
    }

    public void UpdateMotor()
    {
        SnapAlignCharacterWithCamera();
        ProcessMotion();
    }

    public void ResetMotor()
    {
        VerticalVelocity = MoveVector.y;
        MoveVector = Vector3.zero;
    }

    void ProcessMotion()
    {
        // Transforms to world space
        if (!animator.IsDead)
            MoveVector = transform.TransformDirection(MoveVector);
        else
        {
            MoveVector = new Vector3(0.0f, MoveVector.y, 0.0f);
        }

        // Normalizes the Vector if the magnitude if greater than 1
        if (MoveVector.magnitude > 1)
            MoveVector = Vector3.Normalize(MoveVector);

        // Apply sliding if applicable
        ApplySlide();
        MoveVector *= MoveSpeed();
        // Reapply VerticalVelocity MoveVector.y
        MoveVector = new Vector3(MoveVector.x, VerticalVelocity, MoveVector.z);
        ApplyGravity();
        controller.GetCharacterController().Move(MoveVector * Time.deltaTime);
        controller.cameraTarget.transform.position = controller.transform.position;
    }

    void ApplyGravity()
    {
        if(MoveVector.y > -TerminalVelocity)
            MoveVector = new Vector3(MoveVector.x, MoveVector.y - Gravity * Time.deltaTime, MoveVector.z);

        if(controller.GetCharacterController().isGrounded && MoveVector.y < -1)
            MoveVector = new Vector3(MoveVector.x, -1, MoveVector.z);
    }



    void ApplySlide()
    {
        if (!controller.GetCharacterController().isGrounded)
            return;

        slideDirection = Vector3.zero;

        RaycastHit hitInfo;

        if (Physics.Raycast(transform.position + Vector3.up, Vector3.down, out hitInfo))
        {
            if (hitInfo.normal.y < SlideThreshold)
            {
                slideDirection = new Vector3(hitInfo.normal.x, -hitInfo.normal.y, hitInfo.normal.z);
                if (!IsSliding)
                    //TP_Animator.Instance.Slide();

                    IsSliding = true;
            }
            else
                IsSliding = false;
        }

        if (slideDirection.magnitude < MaxControllableSlideMagnitude)
            MoveVector += slideDirection;
        else
        {
            MoveVector = slideDirection;
        }
    }

    void SnapAlignCharacterWithCamera()
    {
        if (animator.ActionState != AT_Animator.CharacterActionState.RollingLeft &&
            animator.ActionState != AT_Animator.CharacterActionState.RollingRight)
        {
            var targetPosition = Camera.main.WorldToScreenPoint(transform.position);
            var direction = Input.mousePosition - targetPosition;
            transform.rotation = Quaternion.Euler(transform.eulerAngles.x,
                Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg,
                transform.eulerAngles.z);
        }
    }

    float MoveSpeed()
    {
        var moveSpeed = 0f;
        if (animator.IsMoving || IsSliding)
            moveSpeed = 10f;
        return moveSpeed;
    }
}