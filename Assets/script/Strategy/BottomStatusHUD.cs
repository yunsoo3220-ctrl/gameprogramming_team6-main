using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class BottomStatusHUD : MonoBehaviour
{
    public static BottomStatusHUD Instance;

    [Header("Text")]
    public TextMeshProUGUI regionNameText;

    [Header("Sliders")]
    public Slider controlSlider;
    public Slider intelSlider;
    public Slider severitySlider;

    [Header("Preview Text")]
    public TextMeshProUGUI controlPreviewText;
    public TextMeshProUGUI intelPreviewText;
    public TextMeshProUGUI severityPreviewText;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        RefreshSelectedRegion();
        ClearPreview();
    }

    public void RefreshSelectedRegion()
    {
        if (District.currentSelected == null)
        {
            if (regionNameText != null)
                regionNameText.text = "NONE";

            SetValues(0, 0, 0);
            return;
        }

        District d = District.currentSelected;

        if (regionNameText != null)
            regionNameText.text = d.gameObject.name;

        SetValues(d.control, d.intel, d.severity);
    }

    public void SetValues(int control, int intel, int severity)
    {
        if (controlSlider != null)
            controlSlider.value = control / 100f;

        if (intelSlider != null)
            intelSlider.value = intel / 100f;

        if (severitySlider != null)
            severitySlider.value = severity / 100f;
    }

    public void ShowPreview(int controlDelta, int intelDelta, int severityDelta)
    {
        if (controlPreviewText != null)
            controlPreviewText.text = FormatDelta(controlDelta);

        if (intelPreviewText != null)
            intelPreviewText.text = FormatDelta(intelDelta);

        if (severityPreviewText != null)
            severityPreviewText.text = FormatDelta(severityDelta);
    }

    public void ClearPreview()
    {
        if (controlPreviewText != null)
            controlPreviewText.text = "";

        if (intelPreviewText != null)
            intelPreviewText.text = "";

        if (severityPreviewText != null)
            severityPreviewText.text = "";
    }

    string FormatDelta(int value)
    {
        if (value > 0)
            return "+" + value;

        if (value < 0)
            return value.ToString();

        return "";
    }
}
