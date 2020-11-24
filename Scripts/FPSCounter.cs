using UnityEngine;
using System.Collections;

public class FPSCounter : MonoBehaviour {
    public bool showFPS = true;
	public float refreshInterval = 0.5f;
	public int fps = 0;
	int maxFPS;
	string fpsStr;
	Rect fpsBox = new Rect(0, 0, 100, 50);
	string[] presetFPSStrings = null;
	
    	//Pre cache possible fps integer strings instead of making them on the fly
	void PrepareFPSStrings(int n) {
        		maxFPS = n;
		presetFPSStrings = new string[n+2];
		for(int i=0; i <= n; i++) {
			presetFPSStrings[i] = i.ToString() + " FPS";
		}
		presetFPSStrings[n+1] = "Good FPS";
	}
	
    	//Set an fps string from cache
	void SetFPSString(int framesPerSec) {
		if(framesPerSec < 0)
			framesPerSec = 0;
		else if(framesPerSec > maxFPS) {
			framesPerSec = maxFPS+1;
		}
		fpsStr = presetFPSStrings[framesPerSec];
	}
	
	void Awake() {
		PrepareFPSStrings(360);
	}
	void Start() {
		StartCoroutine( FPS() );
	}

    	public void ToggleDisplay() {
        		showFPS = !showFPS;
    	}
    
    	void OnGUI() {
        		if(showFPS) {
			SetFPSString(fps);
            			GUI.Label( fpsBox, fpsStr);
        		}
    	}
	
	private IEnumerator FPS() {
		for(;;) {
			int lastFrameCount = Time.frameCount;
			float lastTime = Time.realtimeSinceStartup;
            			yield return new WaitForSeconds(refreshInterval);
			float timeSpan = Time.realtimeSinceStartup - lastTime;
			int frameCount = Time.frameCount - lastFrameCount;
			fps = Mathf.RoundToInt(frameCount / timeSpan);
		}
	}
}