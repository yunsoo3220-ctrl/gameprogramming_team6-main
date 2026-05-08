using UnityEngine;
using UnityEngine.UI;

public class BackgroundScroll : MonoBehaviour
{
    public RawImage rawImage;

    public float verticalSpeed = 0.03f;
    public float horizontalSpeed = 0f;

    void Awake()
    {
        if (rawImage == null)
        {
            rawImage = GetComponent<RawImage>();
        }
    }

    void Update()
    {
        if (rawImage == null) return;

        Rect uv = rawImage.uvRect;

        uv.x += horizontalSpeed * Time.deltaTime;
        uv.y -= verticalSpeed * Time.deltaTime;

        rawImage.uvRect = uv;
    }
}
