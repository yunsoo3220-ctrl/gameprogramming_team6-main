using UnityEngine;

public class District : MonoBehaviour
{
    private Vector3 originalPosition;
    private Vector3 originalScale;
    private int originalSortingOrder;
    private SpriteRenderer spriteRenderer;

    public static District currentSelected; // 현재 선택된 지역

    private bool isSelected = false;

    [Header("Selection Settings")]
    public float liftAmount = 0.2f;
    public float scaleMultiplier = 1.1f;
    public Color selectedColor = new Color(0.8f, 1f, 0.8f);

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalPosition = transform.position;
        originalScale = transform.localScale;
        originalSortingOrder = spriteRenderer.sortingOrder;
    }

    void OnMouseDown()
    {
        // 이미 선택된 걸 다시 클릭 → 선택 해제
        if (isSelected)
        {
            Deselect();
            currentSelected = null;
            MapUIManager.Instance.ShowDefaultMessage();
            return;
        }

        // 다른 지역 선택되어 있으면 해제
        if (currentSelected != null)
        {
            currentSelected.Deselect();
        }

        Select();
        currentSelected = this;

        MapUIManager.Instance.ShowInfo(gameObject.name);
    }

    void Select()
    {
        isSelected = true;

        transform.position = originalPosition + new Vector3(0, liftAmount, 0);
        transform.localScale = originalScale * scaleMultiplier;
        spriteRenderer.sortingOrder = 100;
        spriteRenderer.color = selectedColor;
    }

    public void Deselect()
    {
        isSelected = false;

        transform.position = originalPosition;
        transform.localScale = originalScale;
        spriteRenderer.sortingOrder = originalSortingOrder;
        spriteRenderer.color = Color.white;
    }
}