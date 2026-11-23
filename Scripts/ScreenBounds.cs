using UnityEngine;

public static class ScreenBounds
{
    static Camera cam; // we keep the camera in memory (cache) because Camera.main is slow

    static Camera Cam
    {
        get
        {
            if (cam == null) cam = Camera.main;
            return cam;
        }
    }

    // Orthographic Size = half of the screen height in world units
    public static float HalfHeight => Cam.orthographicSize;

    // Width = height * aspect  
    public static float HalfWidth => Cam.orthographicSize * Cam.aspect;

    public static void Wrap(Transform t, float margin = 0.5f)
    {
        Vector3 p = t.position;
        float hw = HalfWidth + margin;
        float hh = HalfHeight + margin;

        if (p.x > hw) p.x = -hw;
        else if (p.x < -hw) p.x = hw;

        if (p.y > hh) p.y = -hh;
        else if (p.y < -hh) p.y = hh;

        t.position = p;
    }
}
