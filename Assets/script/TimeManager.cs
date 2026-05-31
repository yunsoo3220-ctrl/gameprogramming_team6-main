using UnityEngine;
using TMPro;
using System;

public class TimeManager : MonoBehaviour
{
    public static TimeManager instance;

    [Header("UI")]
    public TMP_Text dateText;

    [Header("Time Settings")]
    public float secondsPerDay = 1f;

    private DateTime currentDate;
    private float timer = 0f;
    private float timeSpeed = 1f;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        currentDate = new DateTime(2098, 1, 1);
        UpdateDateUI();
    }

    void Update()
    {
        if (timeSpeed <= 0f)
            return;

        timer += Time.deltaTime * timeSpeed;

        if (timer >= secondsPerDay)
        {
            int daysToAdd = Mathf.FloorToInt(timer / secondsPerDay);
            currentDate = currentDate.AddDays(daysToAdd);
            timer %= secondsPerDay;

            UpdateDateUI();
        }
    }

    public void UpdateDateUI()
    {
        if (dateText != null)
            dateText.text = currentDate.ToString("yyyy - MM - dd");
    }

    public void Pause()
    {
        timeSpeed = 0f;
    }

    public void Speed1x()
    {
        timeSpeed = 1f;
    }

    public void Speed3x()
    {
        timeSpeed = 3f;
    }

    public string GetCurrentDate()
    {
        return currentDate.ToString("yyyy - MM - dd");
    }

    public bool IsPaused()
    {
        return timeSpeed <= 0f;
    }

    public float GetTimeSpeed()
    {
        return timeSpeed;
    }
}