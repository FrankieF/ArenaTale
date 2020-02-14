using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttack1Behavior : StateMachineBehaviour {

	 // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {

		MeleePlayerController.Instance.PlayerWeaponLocation.localPosition = new Vector3(0, 2.5f, 1.8f);
		MeleePlayerController.Instance.PlayerWeaponBox.enabled = true;
		MeleePlayerController.Instance.PlayerWeaponBox.size = new Vector3(3f, 4f, 3f);


	}

	// OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
	//override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}

	// OnStateExit is called when a transition ends and the state machine finishes evaluating this state
	override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {

		MeleePlayerController.Instance.PlayerWeaponBox.enabled = false;


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
