using UnityEngine;

[RequireComponent(typeof(Animator))]
public class ItemAnim : MonoBehaviour
{
    [Header("속도 설정")]
    [Tooltip("기본값 1.0. 1.5로 지정하면 캐릭터 및 아이템 속도가 1.5배 빨라집니다.")]
    public float speedMultiplier = 1.0f;

    [Header("단독 통합 애니메이션 설정")]
    [Tooltip("체크 시 이 아이템은 캐릭터와 합쳐진 단독 애니메이션을 사용합니다.")]
    public bool isExclusiveCombined = false;

    [Tooltip("통합 애니메이션 오버라이드 컨트롤러")]
    public AnimatorOverrideController combinedOverrideController;

    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();

        // [핵심] 유니티 개별 연산을 정지시켜 프레임 어긋남(오차)을 완전 차단
        if (animator != null)
        {
            animator.enabled = false;
        }
    }

    /// <summary>
    /// 캐릭터(마스터)의 현재 State와 시간(normalizedTime)을 전달받아 강제 동기화
    /// </summary>
    public void SyncWithMaster(int stateHash, float normalizedTime, float speed)
    {
        if (animator == null) animator = GetComponent<Animator>();
        if (animator == null || !animator.gameObject.activeInHierarchy) return;

        animator.speed = speed;

        if (animator.HasState(0, stateHash))
        {
            animator.Play(stateHash, 0, normalizedTime);
            animator.Update(0f);
        }
    }
}