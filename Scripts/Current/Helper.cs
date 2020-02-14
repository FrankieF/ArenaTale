using UnityEngine;

public static class Helper 
{
    public struct ClipPlanePoints
    {
        public Vector3 UpperLeft;
        public Vector3 UpperRight;
        public Vector3 LowerLeft;
        public Vector3 LowerRight;      
    }

    public static float ClampAngles(float angle, float min, float max)
    {
        do
        {
            if (angle < -360)
                angle += 360;
            if (angle > 360)
                angle -= 360;
        } while (angle < -360 || angle > 360);

        return Mathf.Clamp(angle, min, max);
    }

    public static ClipPlanePoints ClipPlaneAtNear(Vector3 pos)
    {
        var clipPlanePoints = new ClipPlanePoints();

        if(Camera.main == null)
            return clipPlanePoints;

        var transform = Camera.main.transform;
        var halfFOV = (Camera.main.fieldOfView * .5f) * Mathf.Deg2Rad;
        var aspect = Camera.main.aspect;
        var distance = Camera.main.nearClipPlane;
        var height = distance * Mathf.Tan(halfFOV);
        var width = height * aspect;

        clipPlanePoints.LowerRight = pos + transform.right * width;
        clipPlanePoints.LowerLeft -= transform.up * height;
        clipPlanePoints.LowerRight += transform.forward * distance;

        clipPlanePoints.LowerLeft = pos - transform.right * width;
        clipPlanePoints.LowerLeft-= transform.up * height;
        clipPlanePoints.LowerLeft += transform.forward * distance;

        clipPlanePoints.UpperRight = pos + transform.right * width;
        clipPlanePoints.UpperRight+= transform.up * height;
        clipPlanePoints.UpperRight += transform.forward * distance;

        clipPlanePoints.UpperLeft= pos - transform.right * width;
        clipPlanePoints.UpperLeft+= transform.up * height;
        clipPlanePoints.UpperLeft += transform.forward * distance;

        return clipPlanePoints;
    }

    public static Vector3 AddVectorPostivieY(Vector3 current, Vector3 target)
    {
        if (target.y < 0)
            target = new Vector3(target.x, target.y * -1, target.z);
        return current + target;
    }
}
