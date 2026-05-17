using TMPro;
using UnityEngine;

public class EventLogRowUI : MonoBehaviour
{
    public TMP_Text dateText;
    public TMP_Text regionText;
    public TMP_Text eventText;
    public TMP_Text effectText;

    public void SetRow(
        string date,
        string region,
        string title,
        string effect)
    {
        dateText.text = date;
        regionText.text = region;
        eventText.text = title;
        effectText.text = effect;
    }
}