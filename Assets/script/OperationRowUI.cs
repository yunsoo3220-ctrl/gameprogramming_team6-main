using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class OperationRowUI : MonoBehaviour
{
    public TextMeshProUGUI operationNameText;
    public TextMeshProUGUI regionNameText;
    public Slider progressSlider;
    public TextMeshProUGUI percentText;

    public void SetInfo(string operationName, string regionName)
    {
        if (operationNameText != null)
            operationNameText.text = operationName;

        if (regionNameText != null)
            regionNameText.text = regionName;

        UpdateProgress(0f);
    }

    public void UpdateProgress(float progress01)
    {
        progress01 = Mathf.Clamp01(progress01);

        if (progressSlider != null)
            progressSlider.value = progress01;

        if (percentText != null)
            percentText.text = Mathf.RoundToInt(progress01 * 100f) + "%";
    }
}