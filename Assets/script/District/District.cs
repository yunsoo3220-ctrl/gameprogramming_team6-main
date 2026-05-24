using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class District : MonoBehaviour
{
    public static District currentSelected;

    [Header("Selection")]
    public float liftAmount = 0.2f;

    public Color normalColor = new Color(0.08f, 0.35f, 0.16f, 0.9f);
    public Color selectedColor = new Color(0.8f, 1f, 0.8f, 1f);

    [Header("Stats")]
    [Range(0, 100)] public int control;
    [Range(0, 100)] public int intel;
    [Range(0, 100)] public int severity;

    [Header("Traffic")]
    [Range(0, 100)] public int traffic = 50;

    [Header("Adjacent Districts")]
    public List<District> adjacentDistricts = new List<District>();

    [Header("Internet Status")]
    [Range(0, 100)] public float normalPC = 98f;
    [Range(0, 100)] public float infectedPC = 1.5f;
    [Range(0, 100)] public float zombiePC = 0.5f;

    private Vector3 originalPosition;

    private bool isSelected = false;

    private SpriteRenderer spriteRenderer;
    private int originalSortingOrder;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void OnEnable()
    {
        if (spriteRenderer == null)
            spriteRenderer = GetComponent<SpriteRenderer>();

        ApplyTrafficColor();
    }

    void Start()
    {
        originalPosition = transform.position;

        if (spriteRenderer == null)
            spriteRenderer = GetComponent<SpriteRenderer>();

        if (spriteRenderer != null)
        {
            originalSortingOrder = spriteRenderer.sortingOrder;
        }

        NormalizeInternetStatus();
        ApplyTrafficColor();
    }

#if UNITY_EDITOR
    void OnValidate()
    {
        if (spriteRenderer == null)
            spriteRenderer = GetComponent<SpriteRenderer>();

        ApplyTrafficColor();
    }
#endif

    void OnMouseDown()
    {
        if (EventSystem.current != null &&
            EventSystem.current.IsPointerOverGameObject())
            return;

        if (GameManager.Instance != null &&
            GameManager.Instance.startPanel != null &&
            GameManager.Instance.startPanel.activeSelf)
            return;

        // 이미 선택된 지역을 한 번 더 클릭하면 선택 해제
        if (currentSelected == this)
        {
            Deselect();
            currentSelected = null;

            if (MapUIManager.Instance != null)
                MapUIManager.Instance.ShowDefaultMessage();

            if (StatusPanelManager.Instance != null)
                StatusPanelManager.Instance.Refresh();

            return;
        }

        // 다른 지역이 선택되어 있으면 기존 지역 해제
        if (currentSelected != null)
            currentSelected.Deselect();

        // 새 지역 선택
        Select();
    }

    public void Select()
    {
        currentSelected = this;
        isSelected = true;

        transform.position = originalPosition + Vector3.up * liftAmount;

        if (spriteRenderer != null)
        {
            spriteRenderer.color = selectedColor;
            spriteRenderer.sortingOrder = 100;
        }

        if (BottomStatusHUD.Instance != null)
            BottomStatusHUD.Instance.RefreshSelectedRegion();

        if (MapUIManager.Instance != null)
            MapUIManager.Instance.ShowInfo(this);

        if (StatusPanelManager.Instance != null)
            StatusPanelManager.Instance.Refresh();
    }

    public void Deselect()
    {
        isSelected = false;

        transform.position = originalPosition;

        if (spriteRenderer != null)
        {
            spriteRenderer.sortingOrder = originalSortingOrder;
        }

        ApplyTrafficColor();
    }

    public void ApplyTrafficColor()
    {
        if (spriteRenderer == null)
            return;

        if (isSelected)
        {
            spriteRenderer.color = selectedColor;
            return;
        }

        // 트래픽 0 = 거의 죽은 지역
        if (traffic <= 0)
        {
            spriteRenderer.color =
                new Color(0.02f, 0.02f, 0.025f, 0.9f);
        }

        // 매우 위험
        else if (traffic >= 85)
        {
            spriteRenderer.color =
                new Color(0.55f, 0f, 0.18f, 0.92f);
        }

        // 위험
        else if (traffic >= 70)
        {
            spriteRenderer.color =
                new Color(0.28f, 0f, 0.12f, 0.88f);
        }

        // 보통
        else if (traffic >= 45)
        {
            spriteRenderer.color =
                new Color(0.05f, 0.42f, 0.18f, 0.9f);
        }

        // 낮음
        else
        {
            spriteRenderer.color =
                new Color(0.03f, 0.25f, 0.12f, 0.82f);
        }
    }

    public void NormalizeInternetStatus()
    {
        float total = normalPC + infectedPC + zombiePC;

        if (total <= 0f)
        {
            normalPC = 100f;
            infectedPC = 0f;
            zombiePC = 0f;
            return;
        }

        normalPC = (normalPC / total) * 100f;
        infectedPC = (infectedPC / total) * 100f;
        zombiePC = (zombiePC / total) * 100f;
    }

    public bool CanRunStrategy()
    {
        return traffic > 0;
    }

    public void ApplyStrategyEffect(StrategyData data)
    {
        if (data == null)
            return;

        control = Mathf.Clamp(control + data.controlEffect, 0, 100);
        intel = Mathf.Clamp(intel + data.intelEffect, 0, 100);
        severity = Mathf.Clamp(severity + data.severityEffect, 0, 100);

        traffic = Mathf.Clamp(traffic + data.trafficDelta, 0, 100);

        if (data.sendTrafficToAdjacent)
        {
            SendTrafficToAdjacentByPercent(data.trafficSendPercent);
        }

        infectedPC = Mathf.Clamp(infectedPC + Random.Range(0.5f, 2.5f), 0f, 100f);
        zombiePC = Mathf.Clamp(zombiePC + Random.Range(0.1f, 1f), 0f, 100f);
        normalPC = Mathf.Clamp(100f - infectedPC - zombiePC, 0f, 100f);

        NormalizeInternetStatus();
        ApplyTrafficColor();

        if (StatusPanelManager.Instance != null)
            StatusPanelManager.Instance.Refresh();

        if (BottomStatusHUD.Instance != null)
            BottomStatusHUD.Instance.RefreshSelectedRegion();
    }

    void SendTrafficToAdjacent(int percent)
    {
        if (adjacentDistricts == null || adjacentDistricts.Count == 0)
            return;

        float sendAmount = traffic * (percent / 100f);

        if (sendAmount <= 0)
            return;

        float amountPerDistrict =
            sendAmount / adjacentDistricts.Count;

        traffic -= Mathf.RoundToInt(sendAmount);

        foreach (District d in adjacentDistricts)
        {
            if (d == null)
                continue;

            d.traffic += Mathf.RoundToInt(amountPerDistrict);
            d.traffic = Mathf.Clamp(d.traffic, 0, 100);

            d.ApplyTrafficColor();
        }

        traffic = Mathf.Clamp(traffic, 0, 100);

        ApplyTrafficColor();

        Debug.Log(
            gameObject.name +
            " → 인접 지역으로 트래픽 전송: " +
            sendAmount
        );
    }
    public float GetStrategySuccessRate()
    {
        if (traffic <= 0)
            return 0f;

        float trafficPenalty = Mathf.Pow(traffic / 100f, 1.35f) * 0.5f;
        float successRate = 1f - trafficPenalty;

        return Mathf.Clamp(successRate, 0.5f, 0.98f);
    }

    public void ModifyStats(int c, int i, int s)
    {
        control = Mathf.Clamp(control + c, 0, 100);
        intel = Mathf.Clamp(intel + i, 0, 100);
        severity = Mathf.Clamp(severity + s, 0, 100);

        if (MapUIManager.Instance != null)
            MapUIManager.Instance.UpdateBars(this);

        if (BottomStatusHUD.Instance != null)
            BottomStatusHUD.Instance.RefreshSelectedRegion();

        if (StatusPanelManager.Instance != null)
            StatusPanelManager.Instance.Refresh();
    }

    public void UpdateInternetStatus(float deltaTime)
    {
        float infectionGrowth =
            (intel * 0.0025f) +
            (severity * 0.0015f) -
            (control * 0.0015f) +
            (traffic * 0.0008f);

        float zombieGrowth =
            (control * 0.0015f) +
            (infectedPC * 0.0012f) -
            ((100f - severity) * 0.0008f);

        infectedPC += infectionGrowth * deltaTime;
        zombiePC += zombieGrowth * deltaTime;

        infectedPC = Mathf.Clamp(infectedPC, 0f, 100f);
        zombiePC = Mathf.Clamp(zombiePC, 0f, 100f);

        if (infectedPC + zombiePC > 100f)
        {
            float total = infectedPC + zombiePC;
            infectedPC = infectedPC / total * 100f;
            zombiePC = zombiePC / total * 100f;
        }

        normalPC = Mathf.Clamp(100f - infectedPC - zombiePC, 0f, 100f);
    }
    public void SendTrafficToAdjacentByPercent(int percent)
    {
        if (adjacentDistricts == null || adjacentDistricts.Count == 0)
            return;

        percent = Mathf.Clamp(percent, 0, 100);

        int sendAmount = Mathf.RoundToInt(traffic * (percent / 100f));

        if (sendAmount <= 0)
            return;

        sendAmount = Mathf.Clamp(sendAmount, 0, traffic);

        int amountPerDistrict = Mathf.Max(1, sendAmount / adjacentDistricts.Count);

        traffic = Mathf.Clamp(traffic - sendAmount, 0, 100);

        foreach (District d in adjacentDistricts)
        {
            if (d == null)
                continue;

            d.traffic = Mathf.Clamp(d.traffic + amountPerDistrict, 0, 100);
            d.ApplyTrafficColor();
        }

        ApplyTrafficColor();

        if (StatusPanelManager.Instance != null)
            StatusPanelManager.Instance.Refresh();

        if (BottomStatusHUD.Instance != null)
            BottomStatusHUD.Instance.RefreshSelectedRegion();

        Debug.Log(
            gameObject.name +
            " 트래픽 " +
            sendAmount +
            " 전송 / 인접 지역당 +" +
            amountPerDistrict
        );
    }
}