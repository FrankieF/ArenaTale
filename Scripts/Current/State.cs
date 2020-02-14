using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class State : MonoBehaviour
{
    public abstract void OnStatetEnter(GameObject gameObject = null);

    public abstract void Execute(GameObject gameObject = null);

    public abstract void OnStateExit(GameObject gameObject = null);
}
