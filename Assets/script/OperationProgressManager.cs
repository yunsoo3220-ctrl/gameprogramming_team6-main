using System.Collections.Generic;
using UnityEngine;

public class OperationProgressManager : MonoBehaviour
{
    public static OperationProgressManager Instance;

    [Header("Panel")]
    public GameObject progressPanel;

    [Header("UI")]
    public Transform rowContainer;
    public GameObject operationRowPrefab;

    private readonly List<RunningOperation> runningOperations = new List<RunningOperation>();

    void Awake()
    {
        Instance = this;

        if (progressPanel != null)
            progressPanel.SetActive(false);
    }

    void Update()
    {
        if (TimeManager.instance == null)
            return;

        float speed = TimeManager.instance.GetTimeSpeed();

        if (speed <= 0f)
            return;

        float delta = Time.deltaTime * speed;

        for (int i = runningOperations.Count - 1; i >= 0; i--)
        {
            RunningOperation op = runningOperations[i];

            op.elapsedDays += delta;

            float progress = op.elapsedDays / op.durationDays;

            if (op.rowUI != null)
                op.rowUI.UpdateProgress(progress);

            if (progress >= 1f)
            {
                CompleteOperation(op);
                runningOperations.RemoveAt(i);
            }
        }
    }

    public void TogglePanel()
    {
        if (progressPanel == null)
            return;

        progressPanel.SetActive(!progressPanel.activeSelf);
    }

    public void OpenPanel()
    {
        if (progressPanel != null)
            progressPanel.SetActive(true);
    }

    public void ClosePanel()
    {
        if (progressPanel != null)
            progressPanel.SetActive(false);
    }

    public void StartOperation(District targetDistrict, StrategyData data)
    {
        if (targetDistrict == null || data == null)
            return;

        GameObject rowObj = null;
        OperationRowUI rowUI = null;

        if (operationRowPrefab != null && rowContainer != null)
        {
            rowObj = Instantiate(operationRowPrefab, rowContainer);
            rowUI = rowObj.GetComponent<OperationRowUI>();

            if (rowUI != null)
            {
                rowUI.SetInfo(
                    data.strategyName,
                    targetDistrict.gameObject.name
                );
            }
        }
        else
        {
            Debug.LogWarning("OperationProgressManager: RowContainer 또는 OperationRowPrefab이 연결되지 않았습니다.");
        }

        RunningOperation op = new RunningOperation
        {
            targetDistrict = targetDistrict,
            data = data,
            durationDays = Mathf.Max(0.1f, data.durationDays),
            elapsedDays = 0f,
            rowObject = rowObj,
            rowUI = rowUI
        };

        runningOperations.Add(op);
    }

    void CompleteOperation(RunningOperation op)
    {
        if (op.targetDistrict == null || op.data == null)
            return;

        op.targetDistrict.ApplyStrategyEffect(op.data);

        RefreshSelectedDistrictUI(op.targetDistrict);

        Debug.Log(
            "[완료] " +
            op.data.strategyName +
            " / 지역: " +
            op.targetDistrict.gameObject.name
        );

        if (RandomEventManager.Instance != null)
            RandomEventManager.Instance.CheckRandomEvents(op.targetDistrict);

        if (op.rowObject != null)
            Destroy(op.rowObject);
    }
    void RefreshSelectedDistrictUI(District targetDistrict)
    {
        if (targetDistrict == null)
            return;

        if (District.currentSelected != targetDistrict)
            return;

        if (BottomStatusHUD.Instance != null)
        {
            BottomStatusHUD.Instance.ForceRefresh(targetDistrict);
        }
        else
        {
            Debug.LogWarning("BottomStatusHUD.Instance를 찾지 못했습니다.");
        }
    }

    class RunningOperation
    {
        public District targetDistrict;
        public StrategyData data;
        public float durationDays;
        public float elapsedDays;
        public GameObject rowObject;
        public OperationRowUI rowUI;
    }
}