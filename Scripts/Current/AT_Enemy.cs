using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class AT_Enemy : AT_Entity
{

    public GameObject target;
    public float movementSpeed = 10f;
    public float startRunningDistance;
    public float attackRange = 10f;
    public float attackDistance = 20f;
    public bool canAttack = true;
    public int experiencePoints;

    public NavMeshAgent agent;

    public StateMachine stateMachine;

    // Use this for initialization
    public virtual void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    public abstract void SwitchingFollowingTarget();

    public abstract void Idle();

    public abstract void WalkingAndRunningHandler();

    public abstract void AttackHandler();

    public override void TakeDamage(float damage)
    {
        if (DEBUG)
        {
            Debug.Log("Hit/nDamage taken: " + damage);
        }

        health -= damage;
        if (health <= 0)
            Die();
    }

    public override void Die()
    {
        base.Die();
        //animator.Die();
        if (deathParticle != null)
            deathParticle.Play();
        target.GetComponent<AT_Controller>().SetExperiencePoints(experiencePoints);
        Destroy(gameObject);
    }

    public void TakeDamage(int damage, AT_Entity.SpecialAttack specialAttack)
    {
        // switch special attack
    }

    public void Explode()
    {

    }

    public GameObject GetTarget()
    {
        return this.target;
    }

    /// <summary>
    /// This is called as the players switch to update the current target for the players.
    /// </summary>
    /// <param name="target">The new player.</param>
    public void SetTarget(GameObject target)
    {
        this.target = target;
    }

    public float GetAttackDistance()
    {
        return this.attackDistance;
    }

    public NavMeshAgent GetAgent()
    {
        return this.agent;
    }

    public StateMachine GetStateMachine()
    {
        return this.stateMachine;
    }

    /// <summary>
    /// Used when chasing the player to determine if they should attack.
    /// </summary>
    /// <returns></returns>
    public bool IsTargetWithinAttackDistance()
    {
        return (transform.position - target.transform.position).magnitude < attackDistance;
    }

    /// <summary>
    /// After chasing the player this is used to keep attacking if the player moves 
    /// and not have the enemy jolt towards them.
    /// </summary>
    /// <returns></returns>
    public bool IsTargetWithinAttackRange()
    {
        return (transform.position - target.transform.position).magnitude < attackDistance + attackRange;
    }

    public void MoveTowardsTargetPosition()
    {
        Vector3 direction = target.transform.position - transform.position;
        direction.Normalize();
        direction *= moveSpeed * Time.deltaTime;
        transform.position += direction;
    }

    public IEnumerator WaitForNextAttack()
    {
        isAttacking = true;
        float time = 0f;
        while (time < 1.5f)
        {
            time += Time.deltaTime;
            yield return null;
        }
        isAttacking = false;
    }
}
