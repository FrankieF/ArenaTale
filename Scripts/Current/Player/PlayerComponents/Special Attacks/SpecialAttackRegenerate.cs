using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialAttackRegenerate : MonoBehaviour, BaseSpecialAttack {

    public float regeneration;
    public int secondsToTakeRegenerate;

    public BaseSpecialAttack Create()
    {
        return this;
    }

    public IEnumerator Execute(GameObject obj)
    {
        while (secondsToTakeRegenerate > 0)
        {
            obj.GetComponent<AT_Controller>().Regenerate(regeneration);
            secondsToTakeRegenerate--;
            yield return new WaitForSeconds(1.0f);
        }
    }
}
