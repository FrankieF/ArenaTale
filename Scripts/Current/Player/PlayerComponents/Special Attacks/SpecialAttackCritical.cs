using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialAttackCritical : MonoBehaviour, BaseSpecialAttack {

    public float critical;
    public float damage;
    public int secondsForCrit;

    public BaseSpecialAttack Create()
    {
        return this;
    }

    public IEnumerator Execute(GameObject obj)
    {
        float _critical = obj.GetComponent<AT_Controller>().GetCritical();
        float _damage = obj.GetComponent<AT_Controller>().GetAttackDamage();
        obj.GetComponent<AT_Controller>().SetCritical(critical, damage);
        yield return new WaitForSeconds(secondsForCrit);
        obj.GetComponent<AT_Controller>().SetCritical(_critical, _damage);
    }
}
