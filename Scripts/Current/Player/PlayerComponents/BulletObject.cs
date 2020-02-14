using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletObject : PoolObject {

    [SerializeField] private float movementSpeed = 25f;

    // Update is called once per frame
    private void FixedUpdate()
    {
        //transform.localScale += Vector3.one * Time.deltaTime * 3;
        transform.Translate(Vector3.forward * Time.deltaTime * movementSpeed);
    }

    public override void OnObjectReuse()
    {
        transform.localScale = Vector3.one;
    }
}
