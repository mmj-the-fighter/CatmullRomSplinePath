//based on: https://www.codeproject.com/Articles/30838/Overhauser-Catmull-Rom-Splines-for-Camera-Animatio
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PathManager : MonoBehaviour {
    public Color lineColor = Color.yellow;
    [SerializeField]
    public PATHTYPE displayMethod = PATHTYPE.CATMULLROM_SPLINE;
    public float minHeight = 0.0f;
    public List<Vector3> points = new List<Vector3>();
    CatmullRomSplinePath crsp = new CatmullRomSplinePath();
    PolyLinePath lp = new PolyLinePath();
    InterpolatedPath path;
    public bool dontdraw = false;
    
    void Awake()
    {
        SetPathType(PATHTYPE.CATMULLROM_SPLINE);
    }
    public void AddNewPoint()
    {
        points.Add(new Vector3(0, minHeight, 0));
    }

    public bool DuplicateEndPoints()
    {
        if (points.Count < 2)
            return false;
        else
        {
            int lasti = points.Count - 1;
            Vector3 first = new Vector3(points[0].x, points[0].y, points[0].z);
            Vector3 last = new Vector3(points[lasti].x, points[lasti].y, points[lasti].z); 
            points.Insert(0,first);
            points.Add(last);
        }
        return true;
    }

    public Vector3 GetPoint(int index)
    {
        return points[index];
    }

    public int PathPointsCount()
    {
        return points.Count;
    }

    public void RemoveAllPoints()
    {
        points.Clear();
    }

    public void RemovePoint(int index)
    {
        points.RemoveAt(index);
    }

    public void SetPathType(PATHTYPE pt)
    {
        switch (pt)
        {
            case PATHTYPE.POLY_LINE:
                displayMethod = PATHTYPE.POLY_LINE;
                path = lp;
                break;
            case PATHTYPE.CATMULLROM_SPLINE:
                displayMethod = PATHTYPE.CATMULLROM_SPLINE;
                path = crsp;
                break;
            default: 
                path = lp;
                displayMethod = PATHTYPE.POLY_LINE;
                break;
        }
    }

    void OnDrawGizmosSelected()
	{
        if (path == null)
            return;
        path.SetPathManager((PathManager)this);
        if (points.Count < 6)
        {
            return;
        }

        Gizmos.DrawWireSphere(points[0], 1.0f);
        Gizmos.color = lineColor;
        Vector3 prev = path.GetPoint(0);
        for (float i = 0.0f; i < 1.0f; i += 0.05f)
        {
            Vector3 next = path.GetInterpolatedPathPoint(i);
            if (!dontdraw)
                Gizmos.DrawLine(prev, next);
            prev = next;
        }
	}

    public Vector3 PathPointLerp(int index0, int index1, float t)
    {
        return Vector3.Lerp(points[index0], points[index1], t);
    }

    public Vector3 PathPointCatmullRomSplineInterp(int index0, int index1, int index2, int index3, float t)
    {
        return CatmullRom(points[index0],
            points[index1],
            points[index2],
            points[index3],
            t);
    }

    public void PathPointCatmullRomSplineInterpDebugOutput(
        int index0, 
        int index1, 
        int index2, 
        int index3, 
        float t,
        float lt)
    {
        
        Debug.Log("p0 " + index0 + 
            " " + "p1 " + index1 +
            " " + "p2 " + index2 +
            " " + "p3 " + index3 +
            " " + " t " + t +
            " " + " lt " + lt);
    }

    private Vector3 CatmullRom(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t)
    {
        double t2 = t * t;
        double t3 = t2 * t;
        double b0 = 0.5 * (-t3 + 2 * t2 - t);
        double b1 = 0.5 * (3 * t3 - 5 * t2 + 2);
        double b2 = 0.5 * (-3 * t3 + 4 * t2 + t);
        double b3 = 0.5 * (t3 - t2);
        Vector3 res = p0 * (float)b0 + p1 * (float)b1 + p2 * (float)b2 + p3 * (float)b3;
        return res;
    }


    public Vector3 PathTangentCatmullRomSplineInterp(int index0, int index1, int index2, int index3, float t)
    {
        return CatmullRomTangent(points[index0],
            points[index1],
            points[index2],
            points[index3],
            t);
    }
    private Vector3 CatmullRomTangent(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t)
    {
        
        float t2 = t * t;
        float b0 = 0.5f * (-3*t2 + 4 * t - 1);
        float b1 = 0.5f * (9 * t2 - 10 * t);
        float b2 = 0.5f * (-9 * t2 + 8 * t + 1);
        float b3 = 0.5f * (3*t2 - 2*t);
        Vector3 res = p0 * b0 + p1 * b1 + p2 * b2 + p3 * b3;
        return res;
    }
}
