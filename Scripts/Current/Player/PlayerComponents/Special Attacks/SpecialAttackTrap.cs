using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialAttackTrap : MonoBehaviour, BaseSpecialAttack {

    public float damage;
    public int secondsToTakeDamage;

    public BaseSpecialAttack Create()
    {
        return this;
    }

    public IEnumerator Execute(GameObject obj)
    {
        AT_Controller controller = obj.GetComponent<AT_Controller>();
        float attackDamage = controller.GetAttackDamage();
        while (secondsToTakeDamage > 0)
        {
            controller.SetAttackDamage(damage);
            secondsToTakeDamage--;
            yield return new WaitForSeconds(1.0f);
        }
        controller.SetAttackDamage(attackDamage);
    }
}
