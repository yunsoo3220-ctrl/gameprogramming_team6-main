using UnityEngine;

public class UIManager : MonoBehaviour
{
    public GameObject settingsPanel;

    //  설정창 상태 확인용
    public static bool isSettingsOpen = false;

    public void OpenSettings()
    {
        settingsPanel.SetActive(true);
        Time.timeScale = 0f;

        isSettingsOpen = true; 
    }

    public void CloseSettings()
    {
        settingsPanel.SetActive(false);
        Time.timeScale = 1f;

        isSettingsOpen = false; 
    }
}
