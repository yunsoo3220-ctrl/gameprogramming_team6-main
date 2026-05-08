using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class StrategyManager : MonoBehaviour
{
    public static StrategyManager Instance;

    [Header("Main UI")]
    public GameObject strategyPanel;
    public TextMeshProUGUI regionNameText;

    [Header("Panels")]
    public GameObject osPanel;
    public GameObject generalPanel;
    public GameObject hackPanel;
    public GameObject communicatePanel;

    [Header("Tab Button Images")]
    public Image osButtonImage;
    public Image generalButtonImage;
    public Image hackButtonImage;
    public Image communicateButtonImage;

    [Header("Tab Colors")]
    public Color normalTabColor = new Color(0f, 0.6f, 0.35f, 0.55f);
    public Color activeTabColor = new Color(0f, 1f, 0.65f, 1f);

    [Header("OS Texts")]
    public TextMeshProUGUI playerNameText;
    public TextMeshProUGUI startDateText;
    public TextMeshProUGUI currentDateText;
    public TextMeshProUGUI selectedRegionText;
    public TextMeshProUGUI systemStateText;

    void Awake()
    {
        Instance = this;

        if (strategyPanel != null)
            strategyPanel.SetActive(false);

        HideAllPanels();
    }

    void HideAllPanels()
    {
        if (osPanel != null) osPanel.SetActive(false);
        if (generalPanel != null) generalPanel.SetActive(false);
        if (hackPanel != null) hackPanel.SetActive(false);
        if (communicatePanel != null) communicatePanel.SetActive(false);
    }

    public void OpenStrategy()
    {
        if (District.currentSelected == null)
        {
            Debug.Log("선택된 지역 없음");
            return;
        }

        strategyPanel.SetActive(true);
        regionNameText.text = District.currentSelected.gameObject.name;

        if (TimeManager.instance != null)
            TimeManager.instance.Pause();

        ShowOS();
    }

    public void CloseStrategy()
    {
        strategyPanel.SetActive(false);

        if (TimeManager.instance != null)
            TimeManager.instance.Speed1x();
    }

    public void ShowOS()
    {
        HideAllPanels();

        if (osPanel != null)
            osPanel.SetActive(true);

        SetActiveTab(osButtonImage);
        UpdateOSPanel();
    }

    public void ShowGeneral()
    {
        HideAllPanels();

        if (generalPanel != null)
            generalPanel.SetActive(true);

        SetActiveTab(generalButtonImage);
    }

    public void ShowHack()
    {
        HideAllPanels();

        if (hackPanel != null)
            hackPanel.SetActive(true);

        SetActiveTab(hackButtonImage);
    }

    public void ShowCommunicate()
    {
        HideAllPanels();

        if (communicatePanel != null)
            communicatePanel.SetActive(true);

        SetActiveTab(communicateButtonImage);
    }

    void SetActiveTab(Image activeButton)
    {
        if (osButtonImage != null)
            osButtonImage.color = normalTabColor;

        if (generalButtonImage != null)
            generalButtonImage.color = normalTabColor;

        if (hackButtonImage != null)
            hackButtonImage.color = normalTabColor;

        if (communicateButtonImage != null)
            communicateButtonImage.color = normalTabColor;

        if (activeButton != null)
            activeButton.color = activeTabColor;
    }

    public void UpdateOSPanel()
    {
        if (playerNameText != null)
        {
            if (GameManager.Instance != null)
                playerNameText.text = "USER ID: " + GameManager.Instance.playerName;
            else
                playerNameText.text = "USER ID: NO_GAME_MANAGER";
        }

        if (startDateText != null)
        {
            startDateText.text = "ACTIVATION DATE: 2098 - 01 - 01";
        }

        if (currentDateText != null)
        {
            if (TimeManager.instance != null)
                currentDateText.text = "CURRENT DATE: " + TimeManager.instance.GetCurrentDate();
            else
                currentDateText.text = "CURRENT DATE: NO_TIME_MANAGER";
        }

        if (selectedRegionText != null)
        {
            if (District.currentSelected != null)
                selectedRegionText.text = "SELECTED NODE: " + District.currentSelected.gameObject.name;
            else
                selectedRegionText.text = "SELECTED NODE: NONE";
        }

        if (systemStateText != null)
        {
            systemStateText.text = "SYSTEM STATE: DORMANT";
        }
    }
}