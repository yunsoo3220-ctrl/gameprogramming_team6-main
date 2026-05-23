using UnityEngine;

[System.Serializable]
public class RandomEventData
{
    public string title;
    public string description;
    public string effectText;

    [Range(0, 100)]
    public int chance = 30;
}