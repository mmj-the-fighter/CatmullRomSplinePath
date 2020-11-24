//based on: https://www.codeproject.com/Articles/30838/Overhauser-Catmull-Rom-Splines-for-Camera-Animatio
using UnityEngine;
using System.Collections;

public class CatmullRomSplinePath : InterpolatedPath {

    public CatmullRomSplinePath() { }
    public CatmullRomSplinePath(PathManager pc) : base(pc) {}
    public override Vector3 GetInterpolatedPathPoint(float t)
    {
        int n;
        n = pathManagerInstance.PathPointsCount();
        float delta = 1.0f / (float)n;
        int p = (int)(t / delta);
        int p0 = Mathf.Clamp(p - 1, 0, n - 1);
        int p1 = Mathf.Clamp(p, 0, n - 1);
        int p2 = Mathf.Clamp(p + 1, 0, n - 1);
        int p3 = Mathf.Clamp(p + 2, 0, n - 1);     
        float lt = (t - delta * (float)p) / delta;
        //pathManagerInstance.PathPointCatmullRomSplineInterpDebugOutput(p0, p1, p2, p3, t, lt);
        return pathManagerInstance.PathPointCatmullRomSplineInterp(p0, p1, p2, p3, lt);
    }

    /*
    #define BOUNDS(pp) { 	\
						if (pp < 0) \
							pp = 0; \
						else if (pp >= (int)vp.size()-1) \
							pp = vp.size() - 1; } 
     * 
     * */

    public override Vector3 GetTangent(float t)
    {
        int n;
        n = pathManagerInstance.PathPointsCount();
        float delta = 1.0f / (float)n;
        int p = (int)(t / delta);
        int p0 = Mathf.Clamp(p - 1, 0, n - 1);
        int p1 = Mathf.Clamp(p, 0, n - 1);
        int p2 = Mathf.Clamp(p + 1, 0, n - 1);
        int p3 = Mathf.Clamp(p + 2, 0, n - 1);
        float lt = (t - delta * (float)p) / delta;
        return pathManagerInstance.PathTangentCatmullRomSplineInterp(p0, p1, p2, p3, lt);
    }
}
