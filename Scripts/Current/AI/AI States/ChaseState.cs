using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseState : State
{ 
    private static ChaseState state;
    public Vector3 target;
    public static ChaseState Instance
    {
        get
        {
            return state == null ? GameObject.Find("Manager").GetComponent<ChaseState>() : state;
        }
    }

    private float seconds = .3f;

    public override void OnStatetEnter(GameObject gameObject)
    {
        AT_Enemy enemy = gameObject.GetComponent<AT_Enemy>();
        enemy.MyAnimator.SetBool("Walking", true);
        enemy.GetAgent().isStopped = false;
        //StartCoroutine(Chase(enemy));
    }

    public override void Execute(GameObject gameObject)
    {
        AT_Enemy enemy = gameObject.GetComponent<AT_Enemy>();
        if (enemy.IsTargetWithinAttackDistance())
        {
            StopCoroutine(Chase(enemy));
            enemy.GetAgent().velocity = Vector3.zero;
            enemy.GetAgent().isStopped = true;
            enemy.GetStateMachine().SetState(AttackState.Instance);
        } else
        {
            enemy.MoveTowardsTargetPosition();
        }
    }

    public override void OnStateExit(GameObject gameObject)
    {
        AT_Enemy enemy = gameObject.GetComponent<AT_Enemy>();
        enemy.MyAnimator.SetBool("Walking", false);
    }

    private IEnumerator Chase(AT_Enemy enemy)
    {
        while (true)
        {
            target = enemy.GetTarget().transform.position;
            enemy.MoveTowardsTargetPosition();
            //enemy.GetAgent().speed = enemy.GetMovementSpeed();
            //enemy.GetAgent().SetDestination(enemy.GetTarget().transform.position);
            //yield return new WaitForSeconds(seconds);
            yield return null;
        }
    }

}
