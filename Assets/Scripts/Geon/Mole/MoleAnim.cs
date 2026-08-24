using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class MoleAnim : MonoBehaviour
{
    [Header("1. 등장 및 상태 설정")]
    [SerializeField] private string spawnStateName = "Spawn";
    [SerializeField] private string breakStateName = "Break";
    [SerializeField] private string idleStateName = "Idle";

    [Header("2. 시간마다 자동 재생할 랜덤 동작들")]
    [Tooltip("Idle, Head, Eye 등 자동 루프를 돌릴 State 목록")]
    [SerializeField] private string[] randomStateNames;

    [Header("속도 및 시간 설정")]
    [Tooltip("캐릭터 기본 재생 속도 배율")]
    [SerializeField] private float animSpeed = 1.0f;
    [SerializeField] private float minRandomInterval = 2.0f;
    [SerializeField] private float maxRandomInterval = 4.0f;

    private Animator animator;
    private RuntimeAnimatorController defaultController;
    private Coroutine randomAnimCoroutine;
    private float finalAnimSpeed = 1.0f;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        if (animator != null)
        {
            defaultController = animator.runtimeAnimatorController;
        }
    }

    private void OnEnable()
    {
        InitAnimatorSetting();

        if (randomAnimCoroutine != null)
            StopCoroutine(randomAnimCoroutine);

        randomAnimCoroutine = StartCoroutine(Co_SpawnAndRandomLoop());
    }

    private void OnDisable()
    {
        if (randomAnimCoroutine != null)
        {
            StopCoroutine(randomAnimCoroutine);
            randomAnimCoroutine = null;
        }
    }

    private void LateUpdate()
    {
        if (animator == null) return;

        // 단독 통합 아이템이 아닐 때만 자식 아이템들의 프레임을 캐릭터에 칼동기화
        if (GetExclusiveItem() == null)
        {
            AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
            ItemAnim[] equippedItems = GetComponentsInChildren<ItemAnim>(true);

            foreach (ItemAnim item in equippedItems)
            {
                item.SyncWithMaster(stateInfo.shortNameHash, stateInfo.normalizedTime, finalAnimSpeed);
            }
        }
    }

    // 초기화 및 아이템 속도/오버라이드 적용
    public void InitAnimatorSetting()
    {
        if (animator == null) animator = GetComponent<Animator>();

        // 장착된 아이템 중 가장 높은 속도 배율을 가져와 최종 속도 계산
        float itemSpeed = GetEquippedItemSpeed();
        finalAnimSpeed = animSpeed * itemSpeed;
        animator.speed = finalAnimSpeed;

        // 단독 통합 아이템 체크 및 오버라이드 처리
        ItemAnim exclusiveItem = GetExclusiveItem();
        if (exclusiveItem != null && exclusiveItem.combinedOverrideController != null)
        {
            animator.runtimeAnimatorController = exclusiveItem.combinedOverrideController;
        }
        else
        {
            animator.runtimeAnimatorController = defaultController;
        }
    }

    /// <summary>
    /// Spawn 실행 후 지정된 시간 간격으로 randomStateNames 중 무작위 재생
    /// </summary>
    private IEnumerator Co_SpawnAndRandomLoop()
    {
        // 컨트롤러 교체 및 애니메이터 초기화 1프레임 대기
        yield return null;

        // 1. 등장 모션 실행
        PlayStateInternal(spawnStateName);

        if (randomStateNames == null || randomStateNames.Length == 0)
            yield break;

        // 2. 지정된 시간 간격 무한 루프
        while (true)
        {
            float waitTime = Random.Range(minRandomInterval, maxRandomInterval);
            yield return new WaitForSeconds(waitTime);

            int randomIndex = Random.Range(0, randomStateNames.Length);
            PlayStateInternal(randomStateNames[randomIndex]);
        }
    }

    /// <summary>
    /// [외부 호출용] Hit, Dead, Break 등 외부 이벤트 발생 시 사용
    /// </summary>
    /// <param name="stateName">재생할 State 이름</param>
    /// <param name="stopRandomLoop">사망처럼 더 이상 랜덤 루프를 돌리지 않아야 할 경우 true</param>
    public void PlayExternalState(string stateName, bool stopRandomLoop = false)
    {
        if (stopRandomLoop && randomAnimCoroutine != null)
        {
            StopCoroutine(randomAnimCoroutine);
            randomAnimCoroutine = null;
        }

        // Break 상태가 입력되었고, Exclusive 아이템이 켜져 있다면 파괴 코루틴 실행
        if (stateName == breakStateName)
        {
            ItemAnim exclusiveItem = GetExclusiveItem();
            if (exclusiveItem != null && exclusiveItem.gameObject.activeSelf)
            {
                StartCoroutine(Co_ExclusiveBreakRoutine(exclusiveItem.gameObject, stateName));
                return;
            }
        }

        PlayStateInternal(stateName);
    }

    /// <summary>
    /// Exclusive 아이템 파괴 애니메이션 재생 후 Disable 및 원복 처리
    /// </summary>
    private IEnumerator Co_ExclusiveBreakRoutine(GameObject exclusiveObject, string stateName)
    {
        // 1. 깨지는 애니메이션 재생
        PlayStateInternal(stateName);

        // State 전환 반영을 위한 1프레임 대기
        yield return null;

        // 2. 해당 애니메이션의 실제 재생 시간만큼 대기
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        yield return new WaitForSeconds(stateInfo.length);

        // 3. 아이템 오브젝트 Disable
        exclusiveObject.SetActive(false);

        // 4. 기본 캐릭터 컨트롤러 및 속도로 원복
        InitAnimatorSetting();

        // 5. 기본 캐릭터의 Idle 모션으로 복귀
        PlayStateInternal(idleStateName);
    }

    private void PlayStateInternal(string stateName)
    {
        if (animator == null) return;

        animator.Play(stateName, 0, 0f);
        animator.Update(0f);
    }

    private float GetEquippedItemSpeed()
    {
        ItemAnim[] items = GetComponentsInChildren<ItemAnim>(false);
        float highestSpeed = 1.0f;

        foreach (var item in items)
        {
            if (item.speedMultiplier > highestSpeed)
            {
                highestSpeed = item.speedMultiplier;
            }
        }
        return highestSpeed;
    }

    private ItemAnim GetExclusiveItem()
    {
        ItemAnim[] items = GetComponentsInChildren<ItemAnim>(false);
        foreach (var item in items)
        {
            if (item.isExclusiveCombined) return item;
        }
        return null;
    }
}