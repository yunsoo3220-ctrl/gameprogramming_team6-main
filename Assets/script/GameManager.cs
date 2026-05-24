using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Start UI")]
    public GameObject strategyButton;
    public GameObject statusButton;
    public GameObject progressButton;
    public GameObject startPanel;
    public GameObject bottomStatusHUD;

    public TMP_InputField nameInput;

    [Header("Player Data")]
    public string playerName = "Player";

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        if (startPanel != null)
            startPanel.SetActive(true);

        if (strategyButton != null)
            strategyButton.SetActive(false);

        if (statusButton != null)
            statusButton.SetActive(false);

        if (progressButton != null)
            progressButton.SetActive(false);

        if (bottomStatusHUD != null)
            bottomStatusHUD.SetActive(false);

        if (TimeManager.instance != null)
            TimeManager.instance.Pause();
    }

    public void OnClickStart()
    {
        if (nameInput != null)
            playerName = nameInput.text;

        if (string.IsNullOrWhiteSpace(playerName))
            playerName = "Player";

        if (startPanel != null)
            startPanel.SetActive(false);

        if (strategyButton != null)
            strategyButton.SetActive(true);

        if (statusButton != null)
            statusButton.SetActive(true);

        if (progressButton != null)
            progressButton.SetActive(true);

        if (bottomStatusHUD != null)
            bottomStatusHUD.SetActive(true);

        if (TimeManager.instance != null)
            TimeManager.instance.Speed1x();

        if (RandomEventManager.Instance != null)
            RandomEventManager.Instance.TriggerIntroEvent();
    }
}