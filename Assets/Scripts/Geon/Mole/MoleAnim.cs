using UnityEngine;

public class MoleAnim : MonoBehaviour
{
    [SerializeField] 
    private string[] animationStateNames;

    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        // Animator 참조 확인
        if (animator == null)
            animator = GetComponent<Animator>();

        // 배열이 비어있지 않다면 랜덤 재생
        if (animationStateNames != null && animationStateNames.Length > 0)
        {
            int randomIndex = Random.Range(0, animationStateNames.Length);
            string selectedState = animationStateNames[randomIndex];

            // 선택된 애니메이션을 재생
            animator.Play(selectedState, 0, 0f);
        }
        else
        {
            Debug.LogWarning("애니메이션 목록 없음", gameObject);
        }
    }
}
