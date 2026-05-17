using UnityEngine;

public enum StrategyType
{
    General,
    Hack,
    Communicate
}

[System.Serializable]
public class StrategyData
{
    public string strategyName;

    [TextArea]
    public string description;

    [Header("Stat Effect")]
    public int controlDelta;
    public int intelDelta;
    public int severityDelta;

    [Header("Duration")]
    public float durationDays = 5f;

    [Header("Strategy Type")]
    public StrategyType strategyType = StrategyType.General;

    [Header("Traffic Effect")]
    public int trafficDelta = 0;
    public int trafficSendAmount = 0;
}