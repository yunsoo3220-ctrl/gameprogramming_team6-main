using UnityEngine;
using TMPro;
using System;

public class TimeManager : MonoBehaviour
{
    public static TimeManager instance;

    public TMP_Text dateText;

    private DateTime currentDate;

    private float timer = 0f;
    private float timeSpeed = 1f; // 1배속

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
        timer += Time.deltaTime * timeSpeed;

        // 1초 = 하루로 설정 (원하면 바꿔도 됨)
        if (timer >= 1f)
        {
            currentDate = currentDate.AddDays(1);
            timer = 0f;

            UpdateDateUI();
        }
    }

    public void UpdateDateUI()
    {
        dateText.text = currentDate.ToString("yyyy - MM - dd");
    }

    // 일시정지
    public void Pause()
    {
        timeSpeed = 0f;
    }

    //  1배속
    public void Speed1x()
    {
        timeSpeed = 1f;
    }

    //  3배속
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
