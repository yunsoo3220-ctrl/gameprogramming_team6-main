using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RandomEventPopup : MonoBehaviour
{
    [Header("Text")]
    public TMP_Text titleText;
    public TMP_Text regionText;
    public TMP_Text descriptionText;
    public TMP_Text effectText;

    [Header("Button")]
    public Button confirmButton;

    [Header("Event Image")]
    public Image eventImage;
    public Sprite introSprite;
    public Sprite governmentSprite;
    public Sprite socialSprite;

    void Start()
    {
        if (confirmButton != null)
            confirmButton.onClick.AddListener(Close);

        gameObject.SetActive(false);
    }

    public void Show(
        string title,
        string description,
        string effect,
        string region,
        RandomEventType eventType)
    {
        gameObject.SetActive(true);

        if (titleText != null)
            titleText.text = title;

        if (regionText != null)
            regionText.text = region;

        if (descriptionText != null)
            descriptionText.text = description;

        if (effectText != null)
        {
            bool isIntro = eventType == RandomEventType.Intro;
            effectText.gameObject.SetActive(!isIntro);
            effectText.text = isIntro ? "" : effect;
        }

        SetEventImage(eventType);

        if (TimeManager.instance != null)
            TimeManager.instance.Pause();
    }

    void SetEventImage(RandomEventType eventType)
    {
        if (eventImage == null)
            return;

        switch (eventType)
        {
            case RandomEventType.Intro:
                eventImage.sprite = introSprite;
                break;

            case RandomEventType.Government:
                eventImage.sprite = governmentSprite;
                break;

            case RandomEventType.Social:
                eventImage.sprite = socialSprite;
                break;
        }

        eventImage.enabled = eventImage.sprite != null;
    }

    public void Close()
    {
        gameObject.SetActive(false);

        if (TimeManager.instance != null)
            TimeManager.instance.Speed1x();
    }
}