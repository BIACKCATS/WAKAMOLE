using UnityEngine;
using UnityEngine.Rendering;

public class SyncShadowSprite : MonoBehaviour
{
    [Header("Lit Material 할당")]
    public Material shadowMaterial;

    private SpriteRenderer parentRenderer;
    private SpriteRenderer shadowCasterRenderer;

    void Awake()
    {
        parentRenderer = GetComponent<SpriteRenderer>();

        Transform child = transform.Find("ShadowCaster");
        if (child == null)
        {
            GameObject go = new GameObject("ShadowCaster");
            go.transform.SetParent(transform, false);
            child = go.transform;
        }

        shadowCasterRenderer = child.GetComponent<SpriteRenderer>();
        if (shadowCasterRenderer == null)
        {
            shadowCasterRenderer = child.gameObject.AddComponent<SpriteRenderer>();
        }

        // Shadows Only 설정
        shadowCasterRenderer.shadowCastingMode = ShadowCastingMode.ShadowsOnly;
        shadowCasterRenderer.receiveShadows = false;

        // 인스펙터에서 할당한 머티리얼 적용
        if (shadowMaterial != null)
        {
            shadowCasterRenderer.material = shadowMaterial;
        }
    }

    void LateUpdate()
    {
        if (parentRenderer == null || shadowCasterRenderer == null) return;

        // 애니메이션 프레임이나 반전이 바뀌었을 때만 동기화
        if (shadowCasterRenderer.sprite != parentRenderer.sprite ||
            shadowCasterRenderer.flipX != parentRenderer.flipX ||
            shadowCasterRenderer.flipY != parentRenderer.flipY)
        {
            shadowCasterRenderer.sprite = parentRenderer.sprite;
            shadowCasterRenderer.flipX = parentRenderer.flipX;
            shadowCasterRenderer.flipY = parentRenderer.flipY;
        }
    }
}