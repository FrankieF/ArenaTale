using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface BaseSpecialAttack {

    BaseSpecialAttack Create();
    IEnumerator Execute(GameObject obj);
	
}
