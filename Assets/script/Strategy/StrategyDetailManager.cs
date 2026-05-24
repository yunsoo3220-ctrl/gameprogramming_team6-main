using TMPro;
using UnityEngine;

public class StrategyDetailManager : MonoBehaviour
{
    public static StrategyDetailManager Instance;

    [Header("UI")]
    public TMP_Text strategyNameText;
    public TMP_Text strategyDescText;
    public TMP_Text strategyEffectText;

    [Header("Panels")]
    public GameObject strategyPanel;
    public GameObject strategyInfoPanel;

    private StrategyData selectedStrategy;

    void Awake()
    {
        Instance = this;
    }


    public void ShowDefaultInfo()
    {
        selectedStrategy = null;

        if (strategyInfoPanel != null)
            strategyInfoPanel.SetActive(true);

        if (strategyNameText != null)
            strategyNameText.text = "   전략을 선택하십시오";

        if (strategyDescText != null)
            strategyDescText.text = "전략 타일을 클릭하여 전략을 수행.";

        if (strategyEffectText != null)
            strategyEffectText.text =
                "CONTROL -\n" +
                "INTEL -\n" +
                "SEVERITY -\n" +
                "TRAFFIC -\n\n" +
                "REQUIRED INFO: -\n" +
                "DURATION: -\n" +
                "TYPE: -";
    }

    public void ShowStrategyDetail(StrategyData data)
    {
        if (data == null)
        {
            Debug.LogWarning("StrategyData가 없습니다.");
            return;
        }

        selectedStrategy = data;

        if (strategyInfoPanel != null)
            strategyInfoPanel.SetActive(true);

        if (strategyNameText != null)
            strategyNameText.text = data.strategyName;

        if (strategyDescText != null)
            strategyDescText.text = data.description;

        if (strategyEffectText != null)
        {
            strategyEffectText.text =
                "CONTROL +" + data.controlEffect + "\n" +
                "INTEL +" + data.intelEffect + "\n" +
                "SEVERITY +" + data.severityEffect + "\n" +
                "TRAFFIC " + data.trafficDelta + "\n\n" +
                "REQUIRED INFO: " + data.requiredInformation + "\n" +
                "DURATION: " + data.durationDays + " DAYS\n" +
                "TYPE: " + data.strategyType;
        }

        
    }

    public void SelectStrategy(StrategyTileButton tile)
    {
        if (tile == null)
        {
            Debug.LogWarning("선택된 타일이 없습니다.");
            return;
        }

        ShowStrategyDetail(tile.data);
    }

    public void ExecuteSelectedStrategy()
    {
        if (selectedStrategy == null)
        {
            Debug.LogWarning("선택된 전략이 없습니다.");
            return;
        }

        District selectedDistrict = District.currentSelected;

        if (selectedDistrict == null)
        {
            Debug.LogWarning("지역이 선택되지 않았습니다.");
            return;
        }

        if (selectedDistrict.intel < selectedStrategy.requiredInformation)
        {
            Debug.LogWarning(
                "정보력이 부족합니다. 현재 정보력: " +
                selectedDistrict.intel +
                " / 필요 정보력: " +
                selectedStrategy.requiredInformation
            );
            return;
        }

        if (OperationProgressManager.Instance == null)
        {
            Debug.LogError("OperationProgressManager.Instance가 없습니다.");
            return;
        }

        OperationProgressManager.Instance.StartOperation(
            selectedDistrict,
            selectedStrategy
        );

        CloseStrategyPanelOnly();

        selectedStrategy = null;

        Debug.Log("전략 실행 시작 / 전략 탭 닫힘");
    }

    private void CloseStrategyPanelOnly()
    {
        // 중요:
        // strategyInfoPanel은 끄지 않는다.
        // 꺼버리면 다음에 StrategyPanel을 다시 열었을 때 오른쪽 설명 패널이 사라진 상태로 남는다.

        if (strategyPanel != null)
            strategyPanel.SetActive(false);
        else
            Debug.LogWarning("StrategyPanel이 연결되지 않았습니다.");
    }
}