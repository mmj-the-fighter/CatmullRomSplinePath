using System.Collections;
using UnityEngine;

public class PolyLinePath : InterpolatedPath
{
    public PolyLinePath() { }
    public PolyLinePath(PathManager pc) : base(pc) { }
    public override Vector3 GetInterpolatedPathPoint(float t)
    {
        int n;
        n = pathManagerInstance.PathPointsCount();
        if (n < 2)
            return Vector3.zero;
        float delta = 1.0f / n;
        int p0 = (int) (t / delta);
        p0 = Mathf.Clamp(p0, 0, n - 1);
        int p1 = p0 + 1;
        p1 = Mathf.Clamp(p1, 0, n - 1);
        float lt = (t - delta * (float)p0) / delta;
        return pathManagerInstance.PathPointLerp(p0, p1, lt);
    }

    public override Vector3 GetTangent(float t)
    {
        int n;
        n = pathManagerInstance.PathPointsCount();
        if (n < 2)
            return Vector3.zero;
        float delta = 1.0f / n;
        float index = t / delta;
        int p0 = (int)Mathf.Floor(index);
        p0 = Mathf.Clamp(p0, 0, n - 2);
        int p1 = (int)Mathf.Ceil(index+1);
        p1 = Mathf.Clamp(p1, 0, n - 1);
        Vector3 tangent = pathManagerInstance.GetPoint(p1) - pathManagerInstance.GetPoint(p0);
        //if (tangent == Vector3.zero)
        //{
        //    Vector3 v1 = pathManagerInstance.GetPoint(p1);
        //    Vector3 v0 = pathManagerInstance.GetPoint(p0);
        //    Debug.Log("p0:" + v0.x + " " + v0.y + " " + v0.z + "p1:" + v1.x + " " + v1.y + " " + v1.z);
        //}
        
        return tangent;
    }
}
