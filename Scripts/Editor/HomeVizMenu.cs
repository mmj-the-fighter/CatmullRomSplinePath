using System.Collections;
using UnityEditor;
using UnityEngine;

public class HomeVizMenu : MonoBehaviour {

    [MenuItem("GameObject/HomeViz/CameraController/Linear")]
    static void CreateLinearPathCamera()
    {
        GameObject go = new GameObject("LinearPathCameraController");
        go.transform.position = Vector3.zero;
        go.AddComponent(typeof(LinearPathCameraController));
    }

    [MenuItem("GameObject/HomeViz/CameraController/CatmullRom")]
    static void CreateCatmullRomSplinePathCamera()
    {
        GameObject go = new GameObject("CatmullRomSplineCameraController");
        go.transform.position = Vector3.zero;
        go.AddComponent(typeof(CatmullRomSplinePathCameraController));
    }

    [MenuItem("GameObject/HomeViz/Create FPSCounter")]
    static void CreateKeyFPSCounter()
    {
        GameObject go = new GameObject("FPSCounter");
        go.transform.position = Vector3.zero;
        go.AddComponent(typeof(FPSCounter));
    }

    [MenuItem("GameObject/HomeViz/Create KeyListener")]
    static void CreateKeyListener()
    {
        GameObject go = new GameObject("KeyListener");
        go.transform.position = Vector3.zero;
        go.AddComponent(typeof(KeyListener));
    }
}
