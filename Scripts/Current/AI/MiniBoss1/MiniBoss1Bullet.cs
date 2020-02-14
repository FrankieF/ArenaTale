using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniBoss1Bullet : PoolObject
{

    public float movementSpeed = 25f;
    public int damage = 10;

    private void FixedUpdate()
    {
        transform.position += direction * movementSpeed * Time.deltaTime;
    }

    public override void OnObjectReuse()
    {
        transform.localScale = Vector3.one;

    }

    public void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.CompareTag("Player"))
        {
            collider.gameObject.GetComponent<AT_Controller>().TakeDamage(damage);
            Destroy(gameObject);
        }
    }
}
