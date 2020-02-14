using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolObject : MonoBehaviour {

    public Vector3 direction;

	public virtual void OnObjectReuse(){}

    public void SetDirection(Vector3 target)
    {
        direction = target.normalized;
    }

    protected void OnDestroy()
    {
        gameObject.SetActive(false);
    }
}
