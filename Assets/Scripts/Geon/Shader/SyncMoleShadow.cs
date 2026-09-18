using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class SyncMoleShadow : MonoBehaviour
{
    [Header("Lit Material 할당")]
    public Material shadowMaterial;

    [Header("그림자를 제외할 SpriteRenderer")]
    [SerializeField]
    private List<SpriteRenderer> excludeRenderers
        = new List<SpriteRenderer>();

    private SpriteRenderer parentRenderer;

    // ShadowCaster
    private Dictionary<SpriteRenderer, SpriteRenderer> shadowCasters
        = new Dictionary<SpriteRenderer, SpriteRenderer>();

    void Awake()
    {
        parentRenderer = GetComponent<SpriteRenderer>();

        // MoleCharacter 자신 + 모든 자식 SpriteRenderer 검색
        SpriteRenderer[] renderers =
            GetComponentsInChildren<SpriteRenderer>(true);

        foreach (SpriteRenderer renderer in renderers)
        {
            // Inspector에서 제외한 SpriteRenderer는 건너뜀
            if (excludeRenderers.Contains(renderer))
                continue;

            // ShadowCaster가 또 ShadowCaster를 만드는 것 방지
            if (renderer.gameObject.name == "ShadowCaster")
                continue;

            CreateShadowCaster(renderer);
        }
    }

    void CreateShadowCaster(SpriteRenderer source)
    {
        Transform existing = source.transform.Find("ShadowCaster");

        GameObject shadowObject;

        if (existing != null)
        {
            shadowObject = existing.gameObject;
        }
        else
        {
            shadowObject = new GameObject("ShadowCaster");
            shadowObject.transform.SetParent(source.transform, false);
        }

        Transform shadowTransform = shadowObject.transform;

        // 원본과 같은 위치/회전
        shadowTransform.localPosition = Vector3.zero;
        shadowTransform.localRotation = Quaternion.identity;

        // 기존 x의 0.9배, 그게 자연스러움
        shadowTransform.localScale =
            new Vector3(0.9f, 1f, 1f);

        SpriteRenderer shadowRenderer =
            shadowObject.GetComponent<SpriteRenderer>();

        if (shadowRenderer == null)
        {
            shadowRenderer =
                shadowObject.AddComponent<SpriteRenderer>();
        }

        // 그림자만 생성
        shadowRenderer.shadowCastingMode =
            ShadowCastingMode.ShadowsOnly;

        shadowRenderer.receiveShadows = false;

        // Material 적용
        if (shadowMaterial != null)
        {
            shadowRenderer.material = shadowMaterial;
        }

        // 현재 Sprite 상태 동기화
        shadowRenderer.sprite = source.sprite;
        shadowRenderer.flipX = source.flipX;
        shadowRenderer.flipY = source.flipY;

        // 원본 활성 상태와 동기화
        shadowObject.SetActive(
            source.gameObject.activeInHierarchy
        );

        shadowCasters[source] = shadowRenderer;
    }

    void LateUpdate()
    {
        foreach (var pair in shadowCasters)
        {
            SpriteRenderer source = pair.Key;
            SpriteRenderer shadow = pair.Value;

            if (source == null || shadow == null)
                continue;

            // 애니메이션 Sprite 변경
            if (shadow.sprite != source.sprite)
            {
                shadow.sprite = source.sprite;
            }

            // Flip 동기화
            if (shadow.flipX != source.flipX)
            {
                shadow.flipX = source.flipX;
            }

            if (shadow.flipY != source.flipY)
            {
                shadow.flipY = source.flipY;
            }

            // 활성/비활성 동기화
            bool active = source.gameObject.activeInHierarchy;

            if (shadow.gameObject.activeSelf != active)
            {
                shadow.gameObject.SetActive(active);
            }
        }
    }
}