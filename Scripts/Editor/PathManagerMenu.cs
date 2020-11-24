using UnityEngine;
using System.Collections;
using UnityEditor;

public class PathCreatorMenu : MonoBehaviour
{

    [MenuItem("GameObject/HomeViz/CreatePath/PolyLine")]
    static void CreatePolyLinePathManager()
    {
        GameObject go = new GameObject("PolyLinePath");
        go.transform.position = Vector3.zero;
        go.AddComponent(typeof(PathManager));
        go.GetComponent<PathManager>().SetPathType(PATHTYPE.POLY_LINE);
    }

    [MenuItem("GameObject/HomeViz/CreatePath/CatmullRomSpline")]
    static void CreateCatmullRomSplinePathManager()
    {
        GameObject go = new GameObject("CatmullRomSplinePath");
        go.transform.position = Vector3.zero;
        go.AddComponent(typeof(PathManager));
        go.GetComponent<PathManager>().SetPathType(PATHTYPE.CATMULLROM_SPLINE);
    }
}
