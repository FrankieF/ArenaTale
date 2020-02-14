using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AT_Entity : MonoBehaviour {

    // used for debug statements in the class
    public bool DEBUG = false;

    protected CharacterController CharacterController;

    public float health = 100f;
    public float healthRegenSpeed = 1f;
    public float moveSpeed = 25f;
    public float attackSpeed = .9f;
    // handles the player attacks currently
    [SerializeField] protected float attackDamage = 10f;
    public float critical = .05f;
    public float criticalAttackDamage = 2f;
    public float blockHealth = 0;
    public float blockHealthRegeneration = 0;
    public float maxBlockHealth;
    public bool canBlock = true;
    public bool isTakingDamage;
    public bool IsDead { get; set; }

    public Animator MyAnimator;
    public Vector3 previousRotation;
    public float currentComboState = 0;
    public float currentCooldownTimer;
    // this is Increase equal to attackSpeed
    public float currentConsecutiveTimer = 1.1f;
    public bool hasCooldownTimerStarted = false;
    public bool isConsecutiveAttack = false;
    public bool isAttacking = false;

    public enum SpecialAttack
    {
        Critical, Explosion, Fire, Ice, Poison, Regeneration, Trap
    }

    /** Sounds and Particles **/
    public AudioSource envirHitSound;
    public ParticleSystem envirHitParticle;
    public AudioSource attackingSound;
    public ParticleSystem attackingParticle;
    public ParticleSystem deathParticle;

    public virtual void TakeDamage(float damage)
    {
        
    }

    public virtual void Die()
    {
        
    }

    public float GetHealth()
    {
        return this.health;
    }

    /// <summary>
    /// Sets the health.
    /// </summary>
    /// <param name="newHealth">New health.</param>
    public void SetHealth(float newHealth)
    {
        health = newHealth;
    }

    /// <summary>
    /// Handles the attack to decrease enemy health. It also indicates when an upgrade can be activated
    /// </summary>
    public virtual void AttackingEnemy()
    {
        if ((Input.GetKeyDown(KeyCode.Mouse0)))
        {
            //          if (miniBoss1Controller.CanTakeDamage)
            //          {
            //              miniBoss1Controller.CurrentBossHealth -= attackDamage;
            //
            //              if (activeUpgrade.canUseUpgrade && activeUpgrade.firstActivation)
            //              {
            //                  activeUpgrade.ExecuteUpgrade();
            //                  activeUpgrade.activateUpgrade = true;
            //              }
            //          }
        }
    }

    public void Regenerate(float hp)
    {

    }

    /// <summary>
    /// Returns the attack damage.
    /// </summary>
    /// <returns>The attack damage.</returns>
    public float GetAttackDamage()
    {
        return this.attackDamage;
    }

    /// <summary>
    /// Increases the attack damage.
    /// </summary>
    /// <param name="damage">Damage to increase attack by.</param>
    public void SetAttackDamage(float damage)
    {
        // Put error checking code here
        this.attackDamage += damage;
    }

    /// <summary>
    /// Returns the critical chance.
    /// </summary>
    /// <returns>The attack damage.</returns>
    public float GetCritical()
    {
        return this.critical;
    }

    /// <summary>
    /// Increases the critical chance.
    /// </summary>
    /// <param name="critical">Damage to increase attack by.</param>
    public void SetCritical(float critical, float damage)
    {
        // Put error checking code here
        this.critical += critical;
        this.criticalAttackDamage = damage;
    }

    /// <summary>
    /// Gets the attack speed.
    /// </summary>
    /// <returns>The attack speed.</returns>
    public float GetAttackSpeed()
    {
        return this.attackSpeed;
    }

    /// <summary>
    /// Increases the attack speed.
    /// </summary>
    /// <param name="speed">Speed by which attack speed is multipled by.</param>
    public void SetAttackSpeed(float speed)
    {
        // Put error checking code here
        this.attackSpeed *= speed;
    }

    /// <summary>
    /// Healths the maximum.
    /// </summary>
    /// <returns>The maximum.</returns>
    public float HealthMaximum()
    {
        return this.health;
    }

    /// <summary>
    /// Increases the health maximum.
    /// </summary>
    /// <param name="health">Health to increase health by.</param>
    public void SetHealthMaximum(float health)
    {
        // Put error checking code here
        this.health += health;
    }

    /// <summary>
    /// Gets the health regen speed.
    /// </summary>
    /// <returns>The health regen speed.</returns>
    public float GetHealthRegenSpeed()
    {
        return this.healthRegenSpeed;
    }

    /// <summary>
    /// Increases the health regen speed.
    /// </summary>
    /// <param name="speed">Speed to increase health regenerations speed by.</param>
    public void SetHealthRegenSpeed(float speed)
    {
        // Put error checking code here
        this.healthRegenSpeed += speed;
    }

    /// <summary>
    /// Gets the block H.
    /// </summary>
    /// <returns>The block H.</returns>
    public float GetBlockHP()
    {
        return this.blockHealth;
    }

    /// <summary>
    /// Increases the block H.
    /// </summary>
    /// <param name="health">Health to increase blocking HP by.</param>
    public void SetBlockHP(float health)
    {
        // Put error checking code here
        this.blockHealth += health;
    }

    /// <summary>
    /// Gets the block health regen.
    /// </summary>
    /// <returns>The block health regen.</returns>
    public float GetBlockHealthRegen()
    {
        return this.blockHealthRegeneration;
    }

    /// <summary>
    /// Increases the block health regen.
    /// </summary>
    /// <param name="speed">Speed to increase blocking HP regeneration by.</param>
    public void SetBlockHealthRegen(float speed)
    {
        // Put error checking code here
        this.blockHealthRegeneration += speed;
    }

    public float GetMovementSpeed()
    {
        return this.moveSpeed;
    }

    public void SetMovementSpeed(float movementSpeed)
    {
        this.moveSpeed = movementSpeed;
    }

    /// <summary>
    /// Plays the sound effect and particle for hitting the enivornment.
    /// Should be called as an animation event to have sound/particle play at the right time.
    /// </summary>
    public void OnEnvironmentHit()
    {
        Debug.Log("Particle/Sound playing for hitting environment");
        if (envirHitSound != null)
            envirHitSound.Play();
        if (envirHitParticle != null)
            envirHitParticle.Play();
    }

    /// <summary>
    /// Plays the sound effect and particle for attacking.
    /// Should be called as an animation event to have sound/particle play at the right time.
    /// </summary>
    public void OnAttacking()
    {
        Debug.Log("Particle/Sound playing for attacking");
        if (attackingSound != null)
            attackingSound.Play();
        if (attackingParticle != null)
            attackingParticle.Play();
    }

    public void ResetCharacterAnimations()
    {
        MyAnimator.SetBool("Moving", false);
        MyAnimator.SetBool("Block", false);
        MyAnimator.SetInteger("Attack", 0);
        MyAnimator.SetFloat("Speed", 0);
    }
}
