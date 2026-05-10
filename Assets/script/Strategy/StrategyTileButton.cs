using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StrategyTileButton : MonoBehaviour
{
    [Header("Strategy Data")]
    public StrategyData data;

    [Header("Tile UI")]
    public TextMeshProUGUI titleText;
    public Image tileImage;

    private Button button;

    void Awake()
    {
        button = GetComponent<Button>();

        if (button != null)
            button.onClick.AddListener(OnClickTile);
    }

    void Start()
    {
        RefreshVisual();
    }

    public void OnClickTile()
    {
        if (StrategyDetailManager.Instance != null)
        {
            StrategyDetailManager.Instance.SelectStrategy(this);
        }
    }

    public void Execute()
    {
        if (data == null)
            return;

        if (data.executed)
            return;

        if (District.currentSelected == null)
        {
            Debug.Log("선택된 지역이 없습니다.");
            return;
        }

        District d = District.currentSelected;

        d.control = Mathf.Clamp(d.control + data.controlDelta, 0, 100);
        d.intel = Mathf.Clamp(d.intel + data.intelDelta, 0, 100);
        d.severity = Mathf.Clamp(d.severity + data.severityDelta, 0, 100);

        data.executed = true;

        RefreshVisual();

        if (BottomStatusHUD.Instance != null)
        {
            BottomStatusHUD.Instance.RefreshSelectedRegion();
            BottomStatusHUD.Instance.ClearPreview();
        }
    }

    public void RefreshVisual()
    {
        if (data == null)
            return;

        if (titleText != null)
            titleText.text = data.strategyName;

        if (tileImage != null)
        {
            if (data.executed)
                tileImage.color = new Color(0f, 1f, 0.5f, 0.35f);
            else
                tileImage.color = Color.white;
        }
    }
}
