using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Start UI")]
    public GameObject strategyButton;
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
        // 시작 화면 표시
        if (startPanel != null)
            startPanel.SetActive(true);

        // 게임 시작 전 전략 버튼 숨김
        if (strategyButton != null)
            strategyButton.SetActive(false);

        // 게임 시작 전 가운데 아래 HUD 숨김
        if (bottomStatusHUD != null)
            bottomStatusHUD.SetActive(false);

        // 시간 정지
        if (TimeManager.instance != null)
            TimeManager.instance.Pause();
    }

    public void OnClickStart()
    {
        // 플레이어 이름 입력 저장
        if (nameInput != null)
            playerName = nameInput.text;

        // 이름 미입력 시 기본값
        if (string.IsNullOrWhiteSpace(playerName))
            playerName = "Player";

        // 시작 패널 닫기
        if (startPanel != null)
            startPanel.SetActive(false);

        // 전략 버튼 표시
        if (strategyButton != null)
            strategyButton.SetActive(true);

        // 가운데 아래 HUD 표시
        if (bottomStatusHUD != null)
            bottomStatusHUD.SetActive(true);

        // 시간 재시작
        if (TimeManager.instance != null)
            TimeManager.instance.Speed1x();
    }
}