using UnityEngine;

[System.Serializable]
public class StrategyData
{
    [Header("Basic")]
    public string strategyName;

    [TextArea(2, 5)]
    public string description;

    [Header("Effect")]
    public int controlDelta;
    public int intelDelta;
    public int severityDelta;

    [Header("State")]
    public bool executed;
}