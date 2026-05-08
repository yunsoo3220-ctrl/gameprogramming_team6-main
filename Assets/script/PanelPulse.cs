using UnityEngine;
using UnityEngine.UI;

public class PanelPulse : MonoBehaviour
{
    [Header("Target Image")]
    public Image targetImage;

    [Header("Pulse Settings")]
    public float minAlpha = 0.85f;
    public float maxAlpha = 1.0f;
    public float pulseSpeed = 1.5f;

    private Color originalColor;

    void Start()
    {
        if (targetImage == null)
            targetImage = GetComponent<Image>();

        originalColor = targetImage.color;
    }

    void Update()
    {
        float alpha = Mathf.Lerp(
            minAlpha,
            maxAlpha,
            (Mathf.Sin(Time.time * pulseSpeed) + 1f) / 2f
        );

        targetImage.color = new Color(
            originalColor.r,
            originalColor.g,
            originalColor.b,
            alpha
        );
    }
}