using UnityEngine;

public class InternetStatusUpdater : MonoBehaviour
{
    [Header("Update")]
    public float updateInterval = 1f;

    private float timer;

    void Update()
    {
        if (TimeManager.instance == null)
            return;

        float currentSpeed = TimeManager.instance.GetTimeSpeed();

        // 정지 상태면 인터넷 변화도 멈춤
        if (currentSpeed <= 0f)
            return;

        // 배속에 따라 시간 증가
        timer += Time.deltaTime * currentSpeed;

        if (timer < updateInterval)
            return;

        timer = 0f;

        District[] districts =
            FindObjectsByType<District>(FindObjectsSortMode.None);

        foreach (District d in districts)
        {
            // 배속 반영
            d.UpdateInternetStatus(updateInterval * currentSpeed);
        }

        if (StatusPanelManager.Instance != null)
            StatusPanelManager.Instance.Refresh();
    }
}