using UnityEngine;

[System.Serializable]
public class StrategyData
{
    [Header("Basic Info")]
    public string strategyName;

    [TextArea]
    public string description;

    [Header("Effect")]
    public int controlEffect;
    public int intelEffect;
    public int severityEffect;
    public int trafficDelta;

    [Header("Requirement")]
    public int requiredInformation = 0;

    [Header("Duration")]
    public float durationDays = 30f;

    [Header("Type")]
    public string strategyType = "General";

    [Header("Communicate Effect")]
    public bool sendTrafficToAdjacent = false;

    [Range(0, 100)]
    public int trafficSendPercent = 0;
}