using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialAttackIce : MonoBehaviour, BaseSpecialAttack {

    public int damage;
    public int secondsToTakeDamage;

    public BaseSpecialAttack Create()
    {
        return this;
    }

    public IEnumerator Execute(GameObject obj)
    {
        while (secondsToTakeDamage > 0)
        {
            obj.GetComponent<AT_Enemy>().TakeDamage(damage, AT_Entity.SpecialAttack.Ice);
            secondsToTakeDamage--;
            yield return new WaitForSeconds(1.0f);
        }
    }
}
