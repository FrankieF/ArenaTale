using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour
{
    private static CameraController c = null;
    public static CameraController GetInstance
    {
        get
        {
            return c == null ? c = GameObject.Find("Main Camera").GetComponent<CameraController>() : c;
        }
    }

    public Transform target;
    public float moveDamping = 3f;
    public float yDistance = 20f;
    public Bounds bounds;

    void Start()
    {
        StartCoroutine(FollowPlayer());
    }

    IEnumerator FollowPlayer()
    {
        while (true)
        {
            bounds = GetBounds();
            var targetPosition = bounds.center;
            targetPosition.y += yDistance;
            target.position = Vector3.Lerp(target.position, targetPosition, moveDamping * Time.deltaTime);
            yield return null;
        }
    }

    void OnDrawGizmos()
    {
        var bounds = GetBounds();
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(bounds.center, bounds.size);
    }

    Bounds GetBounds()
    {
        var bounds = new Bounds();
        bounds = new Bounds(CameraTarget.transforms[0].position, new Vector3(0, 2, 0));
        for (int i = 1; i < CameraTarget.transforms.Count; i++)
            bounds.Encapsulate(CameraTarget.transforms[i].position);
        return bounds;
    }
}
