using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LockdownManager : MonoBehaviour
{
    public static LockdownManager Instance;

    [Header("Districts")]
    public District[] districts;

    [Header("Lockdown Condition")]
    public float requiredAverageSeverity = 30f;
    public bool lockdownStarted = false;

    [Header("Lockdown Progress")]
    [Range(0, 100)] public float lockdownProgress = 0f;
    public float baseProgressSpeed = 0.4f;
    public float severityMultiplier = 0.03f;

    [Header("UI")]
    public GameObject lockdownPanel;
    public TMP_Text lockdownText;
    public Image lockdownGauge;

    private bool lockdownFinished = false;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        if (districts == null || districts.Length == 0)
            districts = FindObjectsByType<District>(FindObjectsSortMode.None);

        if (lockdownPanel != null)
            lockdownPanel.SetActive(false);
    }

    void Update()
    {
        if (lockdownFinished) return;

        if (!lockdownStarted)
            CheckLockdownStartCondition();
        else
            UpdateLockdownProgress();
    }

    public void CheckLockdownStartCondition()
    {
        if (lockdownStarted) return;
        if (districts == null || districts.Length == 0) return;

        float avgSeverity = GetAverageSeverity();

        if (avgSeverity >= requiredAverageSeverity)
            StartLockdown();
    }

    void StartLockdown()
    {
        lockdownStarted = true;

        if (lockdownPanel != null)
            lockdownPanel.SetActive(true);

        if (RandomEventManager.Instance != null)
        {
            RandomEventManager.Instance.TriggerEvent(
                "서울 봉쇄 정책 시행",
                "정부가 서울 네트워크 봉쇄를 시작했습니다.\n\n" +
                "봉쇄 완료 전까지 최대한 많은 PC를 좀비 PC로 전환하십시오.",
                "LOCKDOWN PROTOCOL ACTIVATED",
                "SEOUL",
                RandomEventType.Lockdown
            );
        }

        UpdateUI(GetAverageSeverity());

        Debug.Log("서울 봉쇄 정책 실행");
    }

    void UpdateLockdownProgress()
    {
        if (TimeManager.instance != null && TimeManager.instance.IsPaused())
        {
            UpdateUI(GetAverageSeverity());
            return;
        }

        float avgSeverity = GetAverageSeverity();

        float timeSpeed = 1f;

        if (TimeManager.instance != null)
            timeSpeed = TimeManager.instance.GetTimeSpeed();

        float progressSpeed =
            baseProgressSpeed + (avgSeverity * severityMultiplier);

        lockdownProgress += progressSpeed * Time.deltaTime * timeSpeed;
        lockdownProgress = Mathf.Clamp(lockdownProgress, 0f, 100f);

        UpdateUI(avgSeverity);

        if (lockdownProgress >= 100f)
            FinishLockdown();
    }

    void FinishLockdown()
    {
        lockdownFinished = true;

        float finalZombieRate = GetAverageZombiePC();

        PlayerPrefs.SetFloat("FinalZombieRate", finalZombieRate);
        PlayerPrefs.SetInt("IsSecretFound", 0);
        PlayerPrefs.Save();

        SceneManager.LoadScene("EndingScene");
    }

    float GetAverageSeverity()
    {
        float total = 0f;
        int count = 0;

        foreach (District d in districts)
        {
            if (d == null) continue;

            total += d.severity;
            count++;
        }

        if (count == 0) return 0f;
        return total / count;
    }

    float GetAverageZombiePC()
    {
        float total = 0f;
        int count = 0;

        foreach (District d in districts)
        {
            if (d == null) continue;

            total += d.zombiePC;
            count++;
        }

        if (count == 0) return 0f;
        return total / count;
    }

    void UpdateUI(float avgSeverity)
    {
        if (lockdownText != null)
        {
            lockdownText.text =
                "LOCKDOWN " +
                lockdownProgress.ToString("F1") +
                "%  |  SEV " +
                avgSeverity.ToString("F1") +
                "%";
        }

        if (lockdownGauge != null)
            lockdownGauge.fillAmount = lockdownProgress / 100f;
    }

    public void HideLockdownUI()
    {
        if (lockdownPanel != null)
            lockdownPanel.SetActive(false);
    }

    public void ShowLockdownUI()
    {
        if (lockdownStarted && !lockdownFinished && lockdownPanel != null)
            lockdownPanel.SetActive(true);
    }
}