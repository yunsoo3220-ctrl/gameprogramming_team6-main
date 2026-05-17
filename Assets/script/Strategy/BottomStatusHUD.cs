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

    private District cachedDistrict;

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
        // 현재 선택된 지역이 있으면 HUD가 기억
        if (District.currentSelected != null)
        {
            cachedDistrict = District.currentSelected;
        }

        // 기억된 지역도 없으면 초기화
        if (cachedDistrict == null)
        {
            if (regionNameText != null)
                regionNameText.text = "NONE";

            SetValues(0, 0, 0);
            ClearPreview();
            return;
        }

        if (regionNameText != null)
            regionNameText.text = cachedDistrict.gameObject.name;

        SetValues(
            cachedDistrict.control,
            cachedDistrict.intel,
            cachedDistrict.severity
        );
    }

    public void SetTargetDistrict(District district)
    {
        cachedDistrict = district;
        RefreshSelectedRegion();
    }

    public District GetTargetDistrict()
    {
        return cachedDistrict;
    }

    public void SetValues(int control, int intel, int severity)
    {
        if (controlSlider != null)
            controlSlider.value = control;

        if (intelSlider != null)
            intelSlider.value = intel;

        if (severitySlider != null)
            severitySlider.value = severity;
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