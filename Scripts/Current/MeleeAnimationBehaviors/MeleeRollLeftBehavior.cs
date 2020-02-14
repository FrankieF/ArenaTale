using UnityEngine;
using System.Collections;

public class MeleeRollLeftBehavior : StateMachineBehaviour {

	 // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
        AT_MeleePlayerController.Instance.GetCharacterController().center = new Vector3(0, 1.59f, 0);
        AT_MeleePlayerController.Instance.GetCharacterController().height = 2.48f;
	   
	}

	// OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
	override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
			AT_MeleePlayerController.Instance.MyTransform.Translate(
						Vector3.left * Time.deltaTime * (AT_MeleePlayerController.Instance.RollDistance), Space.World);
	}

	// OnStateExit is called when a transition ends and the state machine finishes evaluating this state
	override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		AT_MeleePlayerController.Instance.MyRigidbody.velocity = Vector3.zero;
        AT_MeleePlayerController.Instance.GetCharacterController().center = new Vector3(0, 2.55f, 0);
        AT_MeleePlayerController.Instance.GetCharacterController().height = 4.89f;
	}

	// OnStateMove is called right after Animator.OnAnimatorMove(). Code that processes and affects root motion should be implemented here
	//override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}

	// OnStateIK is called right after Animator.OnAnimatorIK(). Code that sets up animation IK (inverse kinematics) should be implemented here.
	//override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}
}
