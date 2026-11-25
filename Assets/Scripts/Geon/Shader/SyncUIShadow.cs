using UnityEngine;
using UnityEngine.UI;

public class SyncUIShadow : MonoBehaviour
{
    public RectTransform shadowRect;      // UI_Shadow의 RectTransform
    public Transform mainLight;           // Directional Light Transform
    public LayerMask groundLayer;         // 바닥 레이어

    public float maxShadowDistance = 5f;  // 최대 높이 제한
    public float maxScaleMultiplier = 1.3f; // 높이에 따른 최대 확대 비율
    public float minAlpha = 0.2f;         // 최고 높이에서의 최소 알파값
    public float maxAlpha = 0.7f;         // 바닥 밀착 시 최대 알파값

    private Image parentImage;
    private Image shadowImage;
    private Material shadowMaterialInstance;

    void Awake()
    {
        parentImage = GetComponent<Image>();
        if (shadowRect != null)
        {
            shadowImage = shadowRect.GetComponent<Image>();
            if (shadowImage != null && shadowImage.material != null)
            {
                shadowMaterialInstance = new Material(shadowImage.material);
                shadowImage.material = shadowMaterialInstance;
            }
        }
    }

    void LateUpdate()
    {
        if (shadowRect == null) return;

        // 1. UI 스프라이트 동기화
        if (parentImage != null && shadowImage != null && shadowImage.sprite != parentImage.sprite)
        {
            shadowImage.sprite = parentImage.sprite;
        }

        // 2. 바닥(Ground)을 향해 Raycast 투사
        Ray ray = new Ray(transform.position, Vector3.down);
        if (Physics.Raycast(ray, out RaycastHit hit, 100f, groundLayer))
        {
            float height = Vector3.Distance(transform.position, hit.point);
            float normalizedHeight = Mathf.Clamp01(height / maxShadowDistance);

            // 3. 라이트 방향에 따른 그림자 오프셋 계산
            Vector3 lightDir = mainLight != null ? -mainLight.forward : Vector3.down;
            Vector3 shadowWorldPos = hit.point + (new Vector3(lightDir.x, 0, lightDir.z) * height * 0.3f);

            // 그림자 위치 및 회전 적용 (바닥 평면에 밀착)
            shadowRect.position = shadowWorldPos + (Vector3.up * 0.01f); // 바닥 묻힘 방지용 미세 오프셋
            shadowRect.rotation = Quaternion.FromToRotation(Vector3.up, hit.normal);

            // 4. 높이에 따른 크기(Scale) 조절
            float currentScale = Mathf.Lerp(1f, maxScaleMultiplier, normalizedHeight);
            shadowRect.localScale = Vector3.one * currentScale;

            // 5. 높이에 따른 투명도(Alpha) 전달
            float currentAlpha = Mathf.Lerp(maxAlpha, minAlpha, normalizedHeight);
            if (shadowMaterialInstance != null)
            {
                shadowMaterialInstance.SetFloat("_ShadowAlpha", currentAlpha);
            }
        }
    }
}