using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EventTickerUI : MonoBehaviour
{
    [Header("Bar")]
    public GameObject eventTickerBar;

    [Header("Panel")]
    public GameObject eventLogPanel;

    [Header("Panels That Hide Ticker")]
    public GameObject[] panelsThatHideTicker;

    [Header("UI")]
    public Button tickerButton;
    public TMP_Text tickerTextA;
    public TMP_Text tickerTextB;
    public RectTransform textRectA;
    public RectTransform textRectB;

    [Header("Scroll Text")]
    public float scrollSpeed = 80f;
    public float gap = 80f;

    private float textWidth;
    private string currentMessage = "[SYSTEM EVENT]: NONE";

    void Awake()
    {
        if (eventLogPanel != null)
            eventLogPanel.SetActive(false);

        if (eventTickerBar != null)
            eventTickerBar.SetActive(false);
    }

    void Start()
    {
        if (tickerButton != null)
            tickerButton.onClick.AddListener(OpenPanel);

        SetMessage(currentMessage);
        UpdateTickerVisibility();
    }

    void Update()
    {
        UpdateTickerVisibility();

        if (eventTickerBar == null || !eventTickerBar.activeSelf)
            return;

        ScrollText();
    }

    void UpdateTickerVisibility()
    {
        if (eventTickerBar == null)
            return;

        foreach (GameObject panel in panelsThatHideTicker)
        {
            if (panel != null && panel.activeSelf)
            {
                eventTickerBar.SetActive(false);
                return;
            }
        }

        eventTickerBar.SetActive(true);
    }

    void ScrollText()
    {
        if (textRectA == null || textRectB == null)
            return;

        float move = scrollSpeed * Time.unscaledDeltaTime;

        textRectA.anchoredPosition += Vector2.left * move;
        textRectB.anchoredPosition += Vector2.left * move;

        if (textRectA.anchoredPosition.x <= -textWidth - gap)
        {
            textRectA.anchoredPosition = new Vector2(
                textRectB.anchoredPosition.x + textWidth + gap,
                textRectA.anchoredPosition.y
            );
        }

        if (textRectB.anchoredPosition.x <= -textWidth - gap)
        {
            textRectB.anchoredPosition = new Vector2(
                textRectA.anchoredPosition.x + textWidth + gap,
                textRectB.anchoredPosition.y
            );
        }
    }

    public void SetMessage(string message)
    {
        currentMessage = message;

        string repeatedMessage =
            currentMessage + "     " +
            currentMessage + "     " +
            currentMessage + "     ";

        if (tickerTextA != null)
            tickerTextA.text = repeatedMessage;

        if (tickerTextB != null)
            tickerTextB.text = repeatedMessage;

        Canvas.ForceUpdateCanvases();

        if (tickerTextA != null)
        {
            tickerTextA.ForceMeshUpdate();
            textWidth = tickerTextA.preferredWidth;
        }

        if (textRectA != null)
            textRectA.anchoredPosition = new Vector2(0f, textRectA.anchoredPosition.y);

        if (textRectB != null)
            textRectB.anchoredPosition = new Vector2(textWidth + gap, textRectB.anchoredPosition.y);
    }

    public void OpenPanel()
    {
        if (eventLogPanel != null)
            eventLogPanel.SetActive(true);

        UpdateTickerVisibility();
    }

    public void ClosePanel()
    {
        if (eventLogPanel != null)
            eventLogPanel.SetActive(false);

        UpdateTickerVisibility();
    }
}