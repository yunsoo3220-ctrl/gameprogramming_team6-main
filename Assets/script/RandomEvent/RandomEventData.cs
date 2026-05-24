using UnityEngine;

public enum RandomEventType
{
    Intro,
    Government,
    Social
}

[System.Serializable]
public class RandomEventData
{
    [Header("Basic Info")]
    public string title;

    [TextArea]
    public string description;

    [TextArea(3, 10)]
    public string effectText;

    [Header("Event Type")]
    public RandomEventType eventType = RandomEventType.Government;

    [Header("Chance")]
    [Range(0, 100)]
    public int chance = 30;

    [Header("Condition")]
    public bool onlySpecificRegion;
    public string targetRegionName;

    public int minControl = 0;
    public int minInformation = 0;
    public int minSeverity = 0;

    [Header("Effect")]
    public int controlDelta = 0;
    public int informationDelta = 0;
    public int severityDelta = 0;
    public int trafficDelta = 0;
    public float infectedPcDelta = 0f;
}