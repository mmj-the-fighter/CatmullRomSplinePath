using UnityEngine;
using System.Collections;
 
public abstract class InterpolatedPath
{
    protected PathManager pathManagerInstance;

    public InterpolatedPath()
    {
    }

    public InterpolatedPath(PathManager pc)
    {
        pathManagerInstance = pc;
    }

    public Vector3 GetPoint(int index)
    {
        return pathManagerInstance.GetPoint(index);
    }

    public void SetPathManager(PathManager pc)
    {
        pathManagerInstance = pc;
    }

    public virtual Vector3 GetInterpolatedPathPoint(float t)
    {
        return Vector3.zero;
    }

    public virtual Vector3 GetTangent(float t)
    {
        return Vector3.zero;
    }

}
