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
        {
            button.onClick.RemoveListener(OnClickTile);
            button.onClick.AddListener(OnClickTile);
        }
    }

    void Start()
    {
        RefreshVisual();
    }

    public void OnClickTile()
    {
        if (data == null)
        {
            Debug.LogWarning(gameObject.name + "에 StrategyData가 없습니다.");
            return;
        }

        if (StrategyDetailManager.Instance != null)
        {
            StrategyDetailManager.Instance.SelectStrategy(this);
        }
        else
        {
            Debug.LogWarning("StrategyDetailManager.Instance가 없습니다.");
        }
    }

    public void RefreshVisual()
    {
        if (data == null)
            return;

        if (titleText != null)
            titleText.text = data.strategyName;

        if (tileImage != null)
            tileImage.color = Color.white;
    }
}