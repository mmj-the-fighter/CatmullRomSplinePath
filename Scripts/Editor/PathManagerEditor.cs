using UnityEngine;
using UnityEditor;
using System.Collections;

[ExecuteInEditMode]
[CustomEditor(typeof(PathManager))]
public class PathManagerEditor : Editor
{
    PathManager PathManager;
    public static int count = 0;
    string pointtoremove = "0";
    string minht = "0";
    string oldminht = "0";
    CatmullRomSplinePath crsp = new CatmullRomSplinePath();
    PolyLinePath lp = new PolyLinePath();
    InterpolatedPath path = null;

    [SerializeField]
    private PATHTYPE pathType = PATHTYPE.CATMULLROM_SPLINE;
    [SerializeField]
    private PATHTYPE prevPathType;
    PathManagerEditor()
    {
        pathType = PATHTYPE.CATMULLROM_SPLINE;
        prevPathType = PATHTYPE.CATMULLROM_SPLINE;
        path = crsp;
    }
    void OnEnable()
    {
        PathManager = (PathManager)target;
        crsp.SetPathManager(PathManager);
        lp.SetPathManager(PathManager);
    }

    public override void OnInspectorGUI()
    {
        if (PathManager == null) return;
        EditorGUILayout.BeginVertical();
        pathType = (PATHTYPE)EditorGUILayout.EnumPopup("Display:", PathManager.displayMethod);
        if (pathType != prevPathType)
        {
            switch (pathType)
            {
                case PATHTYPE.POLY_LINE:
                    path = lp;
                    break;
                case PATHTYPE.CATMULLROM_SPLINE:
                    path = crsp;
                    break;
                default:
                    path = lp;
                    break;
            }
            PathManager.SetPathType(pathType);
            prevPathType = pathType;
        }
        path.SetPathManager(PathManager);
        EditorGUILayout.PrefixLabel("Minimum Height");
        minht = EditorGUILayout.TextField(minht, GUILayout.ExpandWidth(false));
        if (minht.CompareTo(oldminht) != 0)
        {
            float fval;
            if (float.TryParse(minht, out fval))
            {
                PathManager.minHeight = fval;
            }
            oldminht = minht;
        }

        EditorGUILayout.PrefixLabel("Tool Color");
        PathManager.lineColor = EditorGUILayout.ColorField(PathManager.lineColor, GUILayout.ExpandWidth(false));

        if (GUILayout.Button("Duplicate Endpoints"))
        {
            PathManager.DuplicateEndPoints();
        }

        if (GUILayout.Button("Add New Point"))
        {
            PathManager.AddNewPoint();
        }
        int i = 0;
        foreach (Vector3 v in PathManager.points)
        {
            EditorGUILayout.Vector3Field(i.ToString(), v);
            i++;
        }
        if (GUILayout.Button("Remove All Points"))
        {
            PathManager.RemoveAllPoints();
        }
        GUILayout.BeginHorizontal("box");
        pointtoremove = GUILayout.TextField(pointtoremove, 25);
        if (GUILayout.Button("Remove Point " + pointtoremove))
        {
            int val = -1;
            if (int.TryParse(pointtoremove, out val))
            {
                if (val >= 0 && val < PathManager.points.Count)
                {
                    PathManager.RemovePoint(val);
                }
            }
        }
        GUILayout.EndHorizontal();


        EditorGUILayout.EndVertical();
        if (GUI.changed)
        {
            EditorUtility.SetDirty(PathManager);
        }
    }

    void OnSceneGUI()
    {
        bool dontdrawlines = false;
        if (pathType == PATHTYPE.CATMULLROM_SPLINE && PathManager.PathPointsCount() < 6)
        {
            dontdrawlines = true;
        }
        else if (pathType == PATHTYPE.POLY_LINE && PathManager.PathPointsCount() < 2)
        {
            dontdrawlines = true;
        }


        if (!dontdrawlines)
        {
            Vector3 prev = path.GetInterpolatedPathPoint(0);
            for (float i = 0.0f; i < 1.0f; i += 0.05f)
            {
                Vector3 next = path.GetInterpolatedPathPoint(i);
                Handles.DrawLine(prev, next);
                prev = next;
            }
        }

        int k = 0;
        foreach (Vector3 v in PathManager.points)
        {
            Handles.CubeCap(k, v, Quaternion.identity, 2);
            PathManager.points[k++] = Handles.PositionHandle(v, Quaternion.identity);
        }
    }
}
