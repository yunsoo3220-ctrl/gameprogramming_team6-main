using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class MapUIManager : MonoBehaviour
{
    public static MapUIManager Instance;

    public TextMeshProUGUI regionNameText;

    public Slider controlSlider;
    public Slider intelSlider;
    public Slider severitySlider;

    void Awake()
    {
        Instance = this;
    }

    public void ShowInfo(District d)
    {
        regionNameText.text = d.gameObject.name;
        UpdateBars(d);
    }

    public void UpdateBars(District d)
    {
        controlSlider.value = d.control;
        intelSlider.value = d.intel;
        severitySlider.value = d.severity;
    }

    public void ShowDefaultMessage()
    {
        regionNameText.text = "choose the region";

        controlSlider.value = 0;
        intelSlider.value = 0;
        severitySlider.value = 0;
    }
}