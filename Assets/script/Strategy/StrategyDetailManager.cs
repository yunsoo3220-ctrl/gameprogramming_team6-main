using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StrategyDetailManager : MonoBehaviour
{
    public static StrategyDetailManager Instance;

    [Header("Detail Texts")]
    public TextMeshProUGUI strategyNameText;
    public TextMeshProUGUI strategyDescText;
    public TextMeshProUGUI strategyEffectText;

    [Header("Execute Button")]
    public Button executeButton;
    public TextMeshProUGUI executeButtonText;

    private StrategyTileButton selectedTile;

    void Awake()
    {
        Instance = this;

        if (executeButton != null)
            executeButton.onClick.AddListener(ExecuteSelected);
    }

    void Start()
    {
        ClearDetail();
    }

    public void SelectStrategy(StrategyTileButton tile)
    {
        if (tile == null || tile.data == null)
            return;

        selectedTile = tile;
        StrategyData data = tile.data;

        if (strategyNameText != null)
            strategyNameText.text = data.strategyName;

        if (strategyDescText != null)
            strategyDescText.text = data.description;

        if (strategyEffectText != null)
        {
            strategyEffectText.text =
                "CONTROL " + FormatDelta(data.controlDelta) + "\n" +
                "INTEL " + FormatDelta(data.intelDelta) + "\n" +
                "SEVERITY " + FormatDelta(data.severityDelta) + "\n" +
                "TRAFFIC " + FormatDelta(data.trafficDelta) + "\n\n" +
                "DURATION: " + data.durationDays.ToString("F0") + " DAYS\n" +
                "TYPE: " + data.strategyType;
        }

        if (executeButtonText != null)
            executeButtonText.text = "EXECUTE";

        if (executeButton != null)
            executeButton.interactable = true;

        if (BottomStatusHUD.Instance != null)
        {
            BottomStatusHUD.Instance.ShowPreview(
                data.controlDelta,
                data.intelDelta,
                data.severityDelta
            );
        }
    }

    void ExecuteSelected()
    {
        if (selectedTile == null || selectedTile.data == null)
            return;

        District targetDistrict = null;

        if (BottomStatusHUD.Instance != null)
            targetDistrict = BottomStatusHUD.Instance.GetTargetDistrict();

        if (targetDistrict == null)
            targetDistrict = District.currentSelected;

        if (targetDistrict == null)
        {
            Debug.LogWarning("전략을 실행할 지역이 없습니다.");
            return;
        }

        if (!targetDistrict.CanRunStrategy())
        {
            Debug.LogWarning(targetDistrict.gameObject.name + " 지역은 트래픽이 0이라 전략을 실행할 수 없습니다.");
            return;
        }

        StrategyData data = selectedTile.data;

        float successRate = targetDistrict.GetStrategySuccessRate();
        float roll = Random.value;

        if (roll > successRate)
        {
            Debug.LogWarning(
                "전략 실패: " +
                data.strategyName +
                " / 지역: " +
                targetDistrict.gameObject.name +
                " / 성공률: " +
                Mathf.RoundToInt(successRate * 100f) + "%"
            );

            if (BottomStatusHUD.Instance != null)
                BottomStatusHUD.Instance.ClearPreview();

            if (StrategyManager.Instance != null)
                StrategyManager.Instance.CloseStrategy();

            return;
        }

        if (OperationProgressManager.Instance != null)
        {
            OperationProgressManager.Instance.AddOperation(targetDistrict, data);
        }
        else
        {
            Debug.LogWarning("OperationProgressManager가 없습니다.");
            return;
        }

        if (BottomStatusHUD.Instance != null)
            BottomStatusHUD.Instance.ClearPreview();

        if (StrategyManager.Instance != null)
            StrategyManager.Instance.CloseStrategy();
    }

    public void ClearDetail()
    {
        selectedTile = null;

        if (strategyNameText != null)
            strategyNameText.text = "SELECT STRATEGY";

        if (strategyDescText != null)
            strategyDescText.text = "Choose a strategy tile to inspect its effect.";

        if (strategyEffectText != null)
            strategyEffectText.text = "";

        if (executeButtonText != null)
            executeButtonText.text = "EXECUTE";

        if (executeButton != null)
            executeButton.interactable = false;

        if (BottomStatusHUD.Instance != null)
            BottomStatusHUD.Instance.ClearPreview();
    }

    string FormatDelta(int value)
    {
        if (value > 0)
            return "+" + value;

        if (value < 0)
            return value.ToString();

        return "0";
    }
}