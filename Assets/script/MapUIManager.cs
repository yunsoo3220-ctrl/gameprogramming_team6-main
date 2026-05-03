using UnityEngine;
using TMPro;

public class MapUIManager : MonoBehaviour
{
    public static MapUIManager Instance;

    public TextMeshProUGUI infoText;

    private string defaultMessage = "Choose the region";

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        ShowDefaultMessage();
    }

    public void ShowInfo(string districtName)
    {
        infoText.text = districtName;
    }

    public void ShowDefaultMessage()
    {
        infoText.text = defaultMessage;
    }
}