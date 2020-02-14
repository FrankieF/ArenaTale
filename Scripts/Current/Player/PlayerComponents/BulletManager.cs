using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletManager : MonoBehaviour {

	[SerializeField] private GameObject leftArmPosition, rightArmPosition;
    public GameObject owner;

    public void Shoot()
    {
        // Checks the current combo counter from the Ranger to decide which arm to spawn the bullet on
		bool left = ((AT_RangerPlayerController.Instance.GetCurrentComboState() % 4) != 0);
		Vector3 position = left  ? leftArmPosition.transform.position : rightArmPosition.transform.position;
		GamePoolManager.Instance.GetBulletToShoot().Reuse(position, owner.transform.forward, transform.rotation);
    }

    public void Shoot2(bool left)
    {
        Vector3 position = left ? leftArmPosition.transform.position : rightArmPosition.transform.position;
        GamePoolManager.Instance.GetBulletToShoot().Reuse(position, owner.transform.forward, transform.rotation);
    }

    public void Shoot3()
    {
        GamePoolManager.Instance.GetBulletToShoot().Reuse(leftArmPosition.transform.position, owner.transform.forward, transform.rotation);
        GamePoolManager.Instance.GetBulletToShoot().Reuse(rightArmPosition.transform.position, owner.transform.forward, transform.rotation);

    }

}
