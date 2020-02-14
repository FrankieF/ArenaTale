using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEvents : MonoBehaviour {

    public AT_Controller controller;
    public bool testing = false;

    public void Attack()
    {
        if (!testing)
        {
            ((Attacking)controller).AttackFinished();
        }
    }

    public void Roll()
    {
        if (!testing)
        {
            controller.animator.IsRolling = false;
        }
    }

    public void Fire()
    {
        if (!testing)
        {
            GetComponent<BulletManager>().Shoot();
        }
    }

    public void Fire2(int left)
    {
        if (!testing)
        {
            if (left > 0)
                GetComponent<BulletManager>().Shoot2(true);
            else
                GetComponent<BulletManager>().Shoot2(false);
        }
    }

    public void Fire3()
    {
        if (!testing)
        {
            GetComponent<BulletManager>().Shoot3();
        }
    }

    public void Hit()
    {
        //controller.animator.IsHit = false;
    }
}
