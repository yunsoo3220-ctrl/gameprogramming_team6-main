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
                "SEVERITY " + FormatDelta(data.severityDelta);
        }

        if (executeButtonText != null)
            executeButtonText.text = data.executed ? "EXECUTED" : "EXECUTE";

        if (executeButton != null)
            executeButton.interactable = !data.executed;

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
        if (selectedTile == null)
            return;

        selectedTile.Execute();
        SelectStrategy(selectedTile);
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