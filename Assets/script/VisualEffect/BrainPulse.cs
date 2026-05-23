using UnityEngine;
using UnityEngine.UI;

public class BrainPulse : MonoBehaviour
{
    [Header("Target Image")]
    public Image targetImage;

    [Header("Alpha Pulse")]
    public float minAlpha = 0.82f;
    public float maxAlpha = 1f;
    public float alphaSpeed = 0.5f;

    [Header("Scale Pulse")]
    public float scaleAmount = 0.015f;
    public float scaleSpeed = 0.4f;

    private Color originalColor;
    private Vector3 originalScale;

    void Start()
    {
        if (targetImage == null)
            targetImage = GetComponent<Image>();

        originalColor = targetImage.color;
        originalScale = transform.localScale;
    }

    void Update()
    {
        // 알파 점멸
        float alpha = Mathf.Lerp(
            minAlpha,
            maxAlpha,
            (Mathf.Sin(Time.time * alphaSpeed) + 1f) / 2f
        );

        targetImage.color = new Color(
            originalColor.r,
            originalColor.g,
            originalColor.b,
            alpha
        );

        // 미세 Scale 변화
        float scale = 1f + Mathf.Sin(Time.time * scaleSpeed) * scaleAmount;

        transform.localScale = originalScale * scale;
    }
}
