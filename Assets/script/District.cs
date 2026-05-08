using UnityEngine;
using UnityEngine.EventSystems;

public class District : MonoBehaviour
{
    private Vector3 originalPosition;
    private int originalSortingOrder;
    private SpriteRenderer spriteRenderer;

    public static District currentSelected;

    private bool isSelected = false;

    [Header("Selection")]
    public float liftAmount = 0.05f;
    public Color normalColor = new Color(0.08f, 0.35f, 0.16f, 0.9f);
    public Color selectedColor = new Color(0.55f, 1f, 0.65f, 0.95f);

    [Header("Stats")]
    [Range(0, 100)] public int control = 50;
    [Range(0, 100)] public int intel = 50;
    [Range(0, 100)] public int severity = 50;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        originalPosition = transform.position;
        originalSortingOrder = spriteRenderer.sortingOrder;

        spriteRenderer.color = normalColor;
    }

    void OnMouseDown()
    {
        if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
            return;

        if (GameManager.Instance != null &&
            GameManager.Instance.startPanel != null &&
            GameManager.Instance.startPanel.activeSelf)
            return;

        if (isSelected)
        {
            Deselect();
            currentSelected = null;

            MapUIManager.Instance.ShowDefaultMessage();
            return;
        }

        if (currentSelected != null)
            currentSelected.Deselect();

        Select();
        currentSelected = this;

        MapUIManager.Instance.ShowInfo(this);
    }

    void Select()
    {
        isSelected = true;

        transform.position = originalPosition + new Vector3(0, liftAmount, 0);
        spriteRenderer.sortingOrder = 100;
        spriteRenderer.color = selectedColor;
    }

    public void Deselect()
    {
        isSelected = false;

        transform.position = originalPosition;
        spriteRenderer.sortingOrder = originalSortingOrder;
        spriteRenderer.color = normalColor;
    }

    public void ModifyStats(int c, int i, int s)
    {
        control = Mathf.Clamp(control + c, 0, 100);
        intel = Mathf.Clamp(intel + i, 0, 100);
        severity = Mathf.Clamp(severity + s, 0, 100);

        MapUIManager.Instance.UpdateBars(this);
    }
}