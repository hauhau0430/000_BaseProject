using UnityEngine;

[ExecuteAlways]
public class AspectSettings : MonoBehaviour
{
    [SerializeField]
    private Camera[] targetCameras;

    [SerializeField]
    private Vector2 aspectVex;

    // Update is called once per frame
    void Update()
    {
        var screenAspect = Screen.width / (float)Screen.height;
        var targetAspect = aspectVex.x / aspectVex.y;

        var magRate = targetAspect / screenAspect;

        var ViewportRect = new Rect(0, 0, 1, 1);

        if (magRate < 1)
        {
            ViewportRect.width = magRate;
            ViewportRect.x = 0.5f - ViewportRect.width * 0.5f;
        }
        else
        {
            ViewportRect.height = 1 / magRate;
            ViewportRect.y = 0.5f - ViewportRect.height * 0.5f;
        }
        
        foreach(var camera in targetCameras)
        {
            camera.rect = ViewportRect;
        }
    }
}
