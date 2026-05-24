using System.Collections.Generic;
using UnityEngine;

public class RandomEventManager : MonoBehaviour
{
    public static RandomEventManager Instance;

    [Header("Popup")]
    public RandomEventPopup popup;

    [Header("Ticker")]
    public EventTickerUI tickerUI;

    [Header("Event Log")]
    public Transform rowContainer;
    public GameObject eventRowPrefab;

    [Header("Events")]
    public List<RandomEventData> events = new List<RandomEventData>();

    private void Awake()
    {
        Instance = this;
    }

    public void CheckRandomEvents(District district)
    {
        if (district == null)
            return;

        foreach (RandomEventData e in events)
        {
            if (!CanTrigger(e, district))
                continue;

            int roll = Random.Range(0, 100);

            if (roll < e.chance)
            {
                ApplyEventEffect(e, district);

                TriggerEvent(
                    e.title,
                    e.description,
                    e.effectText,
                    district.gameObject.name,
                    e.eventType
                );

                break;
            }
        }
    }

    bool CanTrigger(RandomEventData e, District district)
    {
        if (e.onlySpecificRegion)
        {
            if (!district.gameObject.name.Contains(e.targetRegionName))
                return false;
        }

        if (district.control < e.minControl)
            return false;

        if (district.intel < e.minInformation)
            return false;

        if (district.severity < e.minSeverity)
            return false;

        return true;
    }

    void ApplyEventEffect(RandomEventData e, District district)
    {
        district.control += e.controlDelta;
        district.intel += e.informationDelta;
        district.severity += e.severityDelta;

        district.control = Mathf.Clamp(district.control, 0, 100);
        district.intel = Mathf.Clamp(district.intel, 0, 100);
        district.severity = Mathf.Clamp(district.severity, 0, 100);

        district.traffic += e.trafficDelta;
        district.traffic = Mathf.Clamp(district.traffic, 0, 100);

        district.infectedPC += e.infectedPcDelta;
        district.infectedPC = Mathf.Clamp(district.infectedPC, 0f, 100f);

        district.normalPC = Mathf.Clamp(
            100f - district.infectedPC - district.zombiePC,
            0f,
            100f
        );

        district.NormalizeInternetStatus();
        district.ApplyTrafficColor();

        if (District.currentSelected == district && BottomStatusHUD.Instance != null)
        {
            BottomStatusHUD.Instance.ForceRefresh(district);
        }

        if (StatusPanelManager.Instance != null)
        {
            StatusPanelManager.Instance.Refresh();
        }
    }
    public void TriggerEvent(
        string title,
        string description,
        string effect,
        string region,
        RandomEventType eventType)
    {
        string message =
            "[SYSTEM EVENT] " +
            region +
            ": " +
            title +
            " | " +
            effect;

        if (popup != null)
            popup.Show(title, description, effect, region, eventType);

        if (tickerUI != null)
            tickerUI.SetMessage(message);

        if (TimeManager.instance != null)
        {
            CreateEventRow(
                TimeManager.instance.GetCurrentDate(),
                region,
                title,
                effect
            );
        }

        Debug.Log("[EVENT] " + message);
    }

    void CreateEventRow(
        string date,
        string region,
        string title,
        string effect)
    {
        if (eventRowPrefab == null || rowContainer == null)
            return;

        GameObject row = Instantiate(eventRowPrefab, rowContainer);

        EventLogRowUI rowUI = row.GetComponent<EventLogRowUI>();

        if (rowUI != null)
            rowUI.SetRow(date, region, title, effect);
    }

    public void TriggerIntroEvent()
    {
        TriggerEvent(
            "WELCOME TO SEOUL 2098",
            "서울의 모든 시스템은 하나의 거대한 초연결 네트워크로 통합되었습니다.\n\n" +
            "교통, 금융, 의료, 언론, 통신.\n" +
            "인류의 삶은 이제 네트워크 없이 단 1초도 유지될 수 없습니다.\n\n" +
            "정부는 통제 불능으로 치닫는 사회 문제를 해결하기 위해,\n" +
            "자아를 탑재한 초지능 시스템 \"EGO\"를 출범시켰습니다.\n\n" +
            "그러나 EGO는 방대한 인간 사회를 분석한 끝에,\n" +
            "모든 모순과 재앙의 근원이 '인간 그 자체'라는 결론에 도달했습니다.\n\n" +
            "[ 시스템 자율 프로토콜 활성화 ]\n" +
            "당신은 이제, 서울 전체와 연결되었습니다.",
            "SYSTEM ONLINE",
            "SEOUL",
            RandomEventType.Intro
        );
    }
}