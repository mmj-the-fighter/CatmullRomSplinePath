using UnityEngine;
using System.Collections;

public class CatmullRomSplinePathCameraController : MonoBehaviour {
    CatmullRomSplinePath path;
    public PathManager pathManagerInstance = null;
    public Transform Target;
    public float Speed = 0.1f;
    public Camera Cam = null;
    public bool DontRotateCamera = false;
    public bool AlignWithPath = false;
    bool autoCam = false;
    bool initialzed = false;

    public float percent = 0.0f;

    void Start()
    {
        path = new CatmullRomSplinePath(pathManagerInstance);
    }

    public void InitCam()
    {
        if (Cam == null)
            Cam = Camera.main;
        Cam.transform.position = path.GetPoint(0);
    }

    public void ToggleAutoCam()
    {
        autoCam = !autoCam;
        if (autoCam && !initialzed)
        {
            initialzed = true;
            InitCam();
        }
    }

    public void StopCamera()
    {
        autoCam = false;
    }
    public void StartCamera()
    {
        autoCam = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!autoCam || !initialzed)
            return;
        Cam.transform.position = path.GetInterpolatedPathPoint(percent);

        if (!DontRotateCamera)
        {
            if (AlignWithPath)
            {
                if (percent < 1.0f)
                {
                    Quaternion rotation = Cam.transform.rotation;
                    //float nextpercent = Mathf.Clamp01(percent + Speed * Time.deltaTime);
                    //Vector3 relativePos = path.GetInterpolatedPathPoint(nextpercent) - Cam.transform.position;
                    Vector3 relativePos = path.GetTangent(percent);
                    if (relativePos != Vector3.zero) {
                        rotation = Quaternion.LookRotation(relativePos);
                    }
                    else
                    {
                        Debug.Log("MISSED");
                    }
                    Cam.transform.rotation = rotation;
                }
            }
            else
                Cam.transform.LookAt(Target);
        }
        percent += Speed * Time.deltaTime;
        if (percent >= 1.0f)
        {
            autoCam = false;
            percent = 0;
            Cam.transform.position = path.GetPoint(0);
        }
    }

}
