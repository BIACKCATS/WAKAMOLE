using UnityEngine;
using UnityEngine.Rendering.Universal;

public class AutoDecalShadow : MonoBehaviour
{
    public enum ShadowType
    {
        DynamicSprite, // 두더지 애니메이션 프레임 모양대로 그림자 생성
        FixedOval      // 타원형 고정 종이 그림자 생성
    }

    [Header("1. 머티리얼 및 쉐이더 설정")]
    [Tooltip("Shader Graph -> Decal 머티리얼")]
    public Material baseDecalMaterial;

    [Tooltip("현재 데칼 쉐이더의 텍스처 변수 이름 (기본값: _BaseMap 또는 _MainTex)")]
    public string texturePropertyName = "_BaseMap";

    [Header("2. 그림자 유형")]
    public ShadowType shadowType = ShadowType.DynamicSprite;

    [Header("3. 바닥 높이 및 투사 범위")]
    [Tooltip("3D Plane 바닥의 실제 World Y 좌표 (예: 0)")]
    public float groundWorldY = 0f;

    [Tooltip("그림자 투사 가로/세로 크기")]
    public Vector2 shadowScale = new Vector2(1f, 1f);

    [Tooltip("투사 깊이 (작을수록 바닥 표면에만 딱 닿고 땅속 깊숙이 안 들어갑니다)")]
    public float projectionDepth = 0.5f;

    [Header("4. 그림자 투명도")]
    [Range(0f, 1f)]
    public float shadowOpacity = 0.5f;

    private DecalProjector decalProjector;
    private Material runtimeMaterial;
    private SpriteRenderer parentSprite;
    private int propertyID;

    void Awake()
    {
        parentSprite = GetComponent<SpriteRenderer>();

        // 1. 자식 Decal 오브젝트 자동 생성
        GameObject decalGO = new GameObject("Auto_DecalShadow");
        decalGO.transform.SetParent(transform, false);

        // 2. Decal Projector 컴포넌트 추가
        decalProjector = decalGO.AddComponent<DecalProjector>();

        // 3. 런타임 인스턴스 머티리얼 생성
        if (baseDecalMaterial != null)
        {
            runtimeMaterial = new Material(baseDecalMaterial);
            decalProjector.material = runtimeMaterial;
        }

        propertyID = Shader.PropertyToID(texturePropertyName);
    }

    void LateUpdate()
    {
        if (decalProjector == null) return;

        // 바닥 위치(groundWorldY) 표면을 향해 수직 아래로(90도) 빔 투사
        float pivotY = groundWorldY + (projectionDepth * 0.5f);
        decalProjector.transform.position = new Vector3(transform.position.x, pivotY, transform.position.z);
        decalProjector.transform.rotation = Quaternion.Euler(90f, 0f, 0f);

        decalProjector.size = new Vector3(shadowScale.x, shadowScale.y, projectionDepth);
        decalProjector.fadeFactor = shadowOpacity;

        // 실시간 스프라이트 텍스처 전달
        if (shadowType == ShadowType.DynamicSprite && parentSprite != null && parentSprite.sprite != null && runtimeMaterial != null)
        {
            Sprite sprite = parentSprite.sprite;
            Texture2D tex = sprite.texture;
            Rect rect = sprite.textureRect;

            // 해당 프로퍼티가 존재하는지 안전하게 검사 후 할당
            if (runtimeMaterial.HasProperty(propertyID))
            {
                runtimeMaterial.SetTexture(propertyID, tex);

                // 스프라이트 아틀라스(Atlas) 오프셋 반영
                Vector4 st = new Vector4(
                    rect.width / tex.width,
                    rect.height / tex.height,
                    rect.x / tex.width,
                    rect.y / tex.height
                );

                string stName = texturePropertyName + "_ST";
                if (runtimeMaterial.HasProperty(stName))
                {
                    runtimeMaterial.SetVector(stName, st);
                }
            }
        }
    }
}