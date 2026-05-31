using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [Header("Settings")]
    public GameObject settingsPanel;
    public GameObject settingsButton;

    public static bool isSettingsOpen = false;

    private float previousTimeSpeed = 1f;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        if (settingsPanel != null)
            settingsPanel.SetActive(false);

        // MainScene 시작 패널에서 톱니바퀴가 보이지 않게 기본 OFF
        if (settingsButton != null)
            settingsButton.SetActive(false);

        isSettingsOpen = false;
    }

    public void OpenSettings()
    {
        if (settingsPanel != null)
            settingsPanel.SetActive(true);

        // 설정창이 활성화된 뒤 슬라이더 다시 연결
        if (SoundManager.instance != null)
            SoundManager.instance.ReconnectSliders();

        if (settingsButton != null)
            settingsButton.SetActive(false);

        isSettingsOpen = true;

        if (LockdownManager.Instance != null)
            LockdownManager.Instance.HideLockdownUI();

        if (TimeManager.instance != null)
        {
            previousTimeSpeed = TimeManager.instance.GetTimeSpeed();
            TimeManager.instance.Pause();
        }
        else
        {
            Time.timeScale = 0f;
        }
    }

    public void CloseSettings()
    {
        if (settingsPanel != null)
            settingsPanel.SetActive(false);

        isSettingsOpen = false;

        if (settingsButton != null)
            settingsButton.SetActive(true);

        if (LockdownManager.Instance != null)
            LockdownManager.Instance.ShowLockdownUI();

        if (TimeManager.instance != null)
        {
            if (previousTimeSpeed >= 3f)
                TimeManager.instance.Speed3x();
            else
                TimeManager.instance.Speed1x();
        }
        else
        {
            Time.timeScale = 1f;
        }
    }

    public void ToggleSettings()
    {
        if (isSettingsOpen)
            CloseSettings();
        else
            OpenSettings();
    }

    public void HideSettingsButton()
    {
        if (settingsButton != null)
            settingsButton.SetActive(false);
    }

    public void ShowSettingsButton()
    {
        if (!isSettingsOpen && settingsButton != null)
            settingsButton.SetActive(true);
    }

    public void GoToTitleScene()
    {
        isSettingsOpen = false;

        Time.timeScale = 1f;

        if (TimeManager.instance != null)
            TimeManager.instance.Speed1x();

        SceneManager.LoadScene("TitleScene");
    }
}