using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StatusPanelManager : MonoBehaviour
{
    public static StatusPanelManager Instance;

    [Header("Panel")]
    public GameObject statusPanel;

    [Header("Texts")]
    public TextMeshProUGUI regionNameText;
    public TextMeshProUGUI normalText;
    public TextMeshProUGUI infectedText;
    public TextMeshProUGUI zombieText;
    public TextMeshProUGUI trafficText;

    [Header("Pie Chart")]
    public Image normalSlice;
    public Image infectedSlice;
    public Image zombieSlice;

    private District cachedDistrict;

    void Awake()
    {
        Instance = this;

        if (statusPanel != null)
            statusPanel.SetActive(false);
    }

    public void OpenStatus()
    {
        if (District.currentSelected != null)
            cachedDistrict = District.currentSelected;

        if (BottomStatusHUD.Instance != null &&
            BottomStatusHUD.Instance.GetTargetDistrict() != null)
        {
            cachedDistrict = BottomStatusHUD.Instance.GetTargetDistrict();
        }

        if (statusPanel != null)
            statusPanel.SetActive(true);

        Refresh();
    }

    public void CloseStatus()
    {
        if (statusPanel != null)
            statusPanel.SetActive(false);
    }

    public void Refresh()
    {
        if (District.currentSelected != null)
            cachedDistrict = District.currentSelected;

        if (cachedDistrict == null)
        {
            if (regionNameText != null)
                regionNameText.text = "SELECTED NODE: NONE";

            if (normalText != null)
                normalText.text = "NORMAL PC: 100.0%";

            if (infectedText != null)
                infectedText.text = "INFECTED PC: 0.0%";

            if (zombieText != null)
                zombieText.text = "ZOMBIE PC: 0.0%";

            if (trafficText != null)
                trafficText.text = "TRAFFIC: -";

            SetPie(100f, 0f, 0f);
            return;
        }

        District d = cachedDistrict;

        if (regionNameText != null)
            regionNameText.text = "SELECTED NODE: " + d.gameObject.name;

        if (normalText != null)
            normalText.text = "NORMAL PC: " + d.normalPC.ToString("F1") + "%";

        if (infectedText != null)
            infectedText.text = "INFECTED PC: " + d.infectedPC.ToString("F1") + "%";

        if (zombieText != null)
            zombieText.text = "ZOMBIE PC: " + d.zombiePC.ToString("F1") + "%";

        if (trafficText != null)
            trafficText.text = "TRAFFIC: " + d.traffic;

        SetPie(d.normalPC, d.infectedPC, d.zombiePC);
    }

    void SetPie(float normal, float infected, float zombie)
    {
        float n = normal / 100f;
        float i = infected / 100f;
        float z = zombie / 100f;

        if (normalSlice != null)
        {
            normalSlice.fillAmount = n;
            normalSlice.transform.localRotation = Quaternion.Euler(0, 0, 0);
        }

        if (infectedSlice != null)
        {
            infectedSlice.fillAmount = i;
            infectedSlice.transform.localRotation = Quaternion.Euler(0, 0, -360f * n);
        }

        if (zombieSlice != null)
        {
            zombieSlice.fillAmount = z;
            zombieSlice.transform.localRotation = Quaternion.Euler(0, 0, -360f * (n + i));
        }
    }
}