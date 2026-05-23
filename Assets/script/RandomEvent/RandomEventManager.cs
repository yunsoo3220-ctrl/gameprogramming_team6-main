using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RandomEventManager : MonoBehaviour
{
    public static RandomEventManager Instance;

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

    public void CheckExampleEvent(District district)
    {
        if (district == null)
            return;

        // 예시:
        // 중구에서 작전 수행 시 40% 확률 이벤트

        if (district.gameObject.name.Contains("Jung") ||
            district.gameObject.name.Contains("중구"))
        {
            int roll = Random.Range(0, 100);

            if (roll < 40)
            {
                TriggerEvent(
                    "GOVERNMENT DETECTED",
                    "KOREAN GOVERNMENT DETECTED UNUSUALL TRAFFIC CHANGES.",
                    "Severity +8 / Control -3",
                    district.gameObject.name
                );
            }
        }
    }

    public void TriggerEvent(
        string title,
        string description,
        string effect,
        string region)
    {
        string message =
            "[SYSTEM EVENT] " +
            region +
            ": " +
            title +
            " | " +
            effect;

        // 상단 ticker 변경
        if (tickerUI != null)
            tickerUI.SetMessage(message);

        // 로그 추가
        CreateEventRow(
            TimeManager.instance.GetCurrentDate(),
            region,
            title,
            effect
        );

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

        GameObject row =
            Instantiate(eventRowPrefab, rowContainer);

        EventLogRowUI rowUI =
            row.GetComponent<EventLogRowUI>();

        if (rowUI != null)
        {
            rowUI.SetRow(
                date,
                region,
                title,
                effect
            );
        }
    }
}