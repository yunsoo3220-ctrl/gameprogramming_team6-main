using UnityEngine;

public class DistrictDotInstaller : MonoBehaviour
{
    [Header("Target")]
    public Transform seoulMap;
    public Sprite dotPatternSprite;

    [Header("Pattern Visual")]
    public Color patternColor = new Color(0f, 1f, 0.45f, 0.45f);
    public float patternScale = 1.0f;

    [Header("Sorting")]
    public int patternSortingOrderOffset = 1;

    void Start()
    {
        ApplyPattern();
    }

    public void ApplyPattern()
    {
        if (seoulMap == null || dotPatternSprite == null)
        {
            Debug.LogWarning("Seoul_Map 또는 Dot Pattern Sprite가 연결되지 않았습니다.");
            return;
        }

        foreach (Transform district in seoulMap)
        {
            SpriteRenderer districtRenderer = district.GetComponent<SpriteRenderer>();
            if (districtRenderer == null) continue;

            SpriteMask mask = district.GetComponent<SpriteMask>();
            if (mask == null)
                mask = district.gameObject.AddComponent<SpriteMask>();

            mask.sprite = districtRenderer.sprite;
            mask.alphaCutoff = 0.5f;

            Transform oldPattern = district.Find("DotPattern");
            if (oldPattern != null)
                Destroy(oldPattern.gameObject);

            GameObject patternObj = new GameObject("DotPattern");
            patternObj.transform.SetParent(district);
            patternObj.transform.localPosition = new Vector3(0f, 0f, -0.02f);
            patternObj.transform.localRotation = Quaternion.identity;
            patternObj.transform.localScale = Vector3.one * patternScale;

            SpriteRenderer patternRenderer = patternObj.AddComponent<SpriteRenderer>();
            patternRenderer.sprite = dotPatternSprite;
            patternRenderer.color = patternColor;
            patternRenderer.drawMode = SpriteDrawMode.Tiled;

            Vector2 districtSize = districtRenderer.sprite.bounds.size;
            patternRenderer.size = districtSize * 2.0f;

            patternRenderer.maskInteraction = SpriteMaskInteraction.VisibleInsideMask;

            patternRenderer.sortingLayerName = districtRenderer.sortingLayerName;
            patternRenderer.sortingOrder = districtRenderer.sortingOrder + patternSortingOrderOffset;
        }
    }
}