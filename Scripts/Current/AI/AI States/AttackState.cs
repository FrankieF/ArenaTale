using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : State
{
    private static AttackState state;

    public static AttackState Instance
    {
        get
        {
            return state == null ? GameObject.Find("Manager").GetComponent<AttackState>() : state;
        }
    }

    public override void OnStatetEnter(GameObject gameObject = null)
    {
        
    }

    public override void Execute(GameObject gameObject = null)
    {
        AT_Enemy enemy = gameObject.GetComponent<AT_Enemy>();
        if (!enemy.IsTargetWithinAttackDistance() && !enemy.isAttacking)
        {
            enemy.GetStateMachine().SetState(ChaseState.Instance);
        } else
        {
            if (!enemy.isAttacking)
            {
                enemy.MyAnimator.SetTrigger("Attack");
                int attack = Random.Range(1, 4);
                enemy.MyAnimator.SetTrigger("Attack" + attack);
                enemy.StartCoroutine("WaitForNextAttack");
            }
        }
    }

    public override void OnStateExit(GameObject gameObject = null)
    {
        AT_Enemy enemy = gameObject.GetComponent<AT_Enemy>();
        enemy.StopCoroutine("WaitForNextAttack");
    }

}
