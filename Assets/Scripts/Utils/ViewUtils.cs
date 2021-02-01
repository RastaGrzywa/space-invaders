
using UnityEngine;

public class ViewUtils
{

    public static float MinWorldXPosition(Camera camera)
    {
        return camera.ViewportToWorldPoint(new Vector3(0.1f, 0f,0f)).x;
    }
    
    public static float MaxWorldXPosition(Camera camera)
    {
        return camera.ViewportToWorldPoint(new Vector3(0.9f, 0f,0f)).x;
    }
}
