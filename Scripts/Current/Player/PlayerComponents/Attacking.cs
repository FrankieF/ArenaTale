using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Attacking
{
    void Attack(int attack = 1);
    void AttackFinished();
}
