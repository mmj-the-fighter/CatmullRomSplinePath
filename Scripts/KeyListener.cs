
using UnityEngine;
using System.Collections;

public class KeyListener : MonoBehaviour
{
    public FPSCounter fps;
    public LinearPathCameraController lpc;
    public CatmullRomSplinePathCameraController crspc;
    public CameraController c;
    bool showHelp = false;
    string[] helpMessages = {
        "\" H \": Toggle Help",
        "\" Esc \": Quit",
        "\" F1 \": Toggle AutoCam Linear",
        "\" F2 \": Toggle AutoCam CatMullRomSpline",
        "\" F3 \": Toggle FPS display",
        "\" WASD \\ Arrow keys \": to navigate",
        "\" Mouse \": to loook around"
    };

    float lastInputTime = 0;
    float pollIntervel = 0.6f;
    bool isInputDisabled = false;
    // Update is called once per frame
    void Awake()
    {
        //if (fps == null)
        //    Debug.LogError("fps null");
        //if (lpc == null)
        //    Debug.LogError("lpc null");
        //if (crspc == null)
        //    Debug.LogError("crspc null");


    }
    void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            Application.Quit();
            return;
        }
        if (isInputDisabled && ((Time.realtimeSinceStartup - lastInputTime) < pollIntervel))
            return;
        isInputDisabled = false;
        if (Input.GetKey(KeyCode.F1))
        {
            //LINEAR
            if(crspc!=null)
            crspc.StopCamera();
            if(lpc!=null)
            lpc.ToggleAutoCam();
            isInputDisabled = true;
            lastInputTime = Time.realtimeSinceStartup;
        }
        else if (Input.GetKey(KeyCode.F2))
        {
            //CATMULLROMSPLINE
            if(lpc!=null)
            lpc.StopCamera();
            if (crspc != null)
            crspc.ToggleAutoCam();
            isInputDisabled = true;
            lastInputTime = Time.realtimeSinceStartup;
        }
        else if (Input.GetKey(KeyCode.F3))
        {
            if(fps!=null)
            fps.ToggleDisplay();
            isInputDisabled = true;
            lastInputTime = Time.realtimeSinceStartup;
        }
        else if (Input.GetKey(KeyCode.H))
        {
            showHelp = !showHelp;
            isInputDisabled = true;
            lastInputTime = Time.realtimeSinceStartup;
        }
    }





    float vspacing = 40.0f;
    float x = 0.0f;
    float y = 10.0f;

    void OnGUI()
    {
        if (!showHelp)
            return;
        GUI.skin.label.alignment = TextAnchor.UpperCenter;
        for (int i = 0; i < helpMessages.Length; i++)
        {
            GUI.Label(new Rect(x, y + i * vspacing, Screen.width, vspacing - 1f), helpMessages[i]);
        }
    }
}
