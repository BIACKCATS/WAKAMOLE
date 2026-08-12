using UnityEngine;
using UnityEngine.Rendering;

public class SyncShadowSprite : MonoBehaviour
{
    private SpriteRenderer parentRenderer;
    private SpriteRenderer shadowCasterRenderer;

    void Awake()
    {
        parentRenderer = GetComponent<SpriteRenderer>();

        // 자식 그림자 생성기 찾기 또는 자동 추가
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

        // 카메라에는 안 보이고 그림자만 쏘도록 설정
        shadowCasterRenderer.shadowCastingMode = ShadowCastingMode.ShadowsOnly;
        shadowCasterRenderer.receiveShadows = false;

        // Lit 머티리얼 할당 (Sprite-Lit)
        shadowCasterRenderer.material = new Material(Shader.Find("Universal Render Pipeline/2D/Sprite-Lit-Default"));
    }

    void LateUpdate()
    {
        if (parentRenderer == null || shadowCasterRenderer == null) return;

        // 원본 스프라이트 프레임 및 반전 상태 실시간 동기화
        shadowCasterRenderer.sprite = parentRenderer.sprite;
        shadowCasterRenderer.flipX = parentRenderer.flipX;
        shadowCasterRenderer.flipY = parentRenderer.flipY;
    }
}