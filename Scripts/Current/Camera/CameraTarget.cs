using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTarget : MonoBehaviour
{
    public static List<Transform> transforms = new List<Transform>();

    void OnEnable()
    {
        if (!transforms.Contains(transform))
            transforms.Add(transform);
    }

    void OnDisable()
    {
        if (transforms.Contains(transform))
            transforms.Remove(transform);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position, 1.0f);
    }
}
