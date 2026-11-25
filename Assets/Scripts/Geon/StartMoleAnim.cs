using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(RectTransform))]
public class StartMoleAnim : MonoBehaviour
{
    public enum MotionType
    {
        None,   // 평상시 움직임 없음 (클릭 시 작아졌다 커짐)
        Bouncy, // 통통 튀는 느낌
        Sway,   // Z축 까딱까딱
        Shiver  // 바들바들 떠는 느낌
    }

    [Header("모션 타입 선택")]
    [SerializeField] private MotionType motionType;

    private RectTransform rectTransform;
    private Sequence currentSequence;
    private Vector2 originalPosition;
    private bool isInteracting = false;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();

        if (motionType == MotionType.Sway)
        {
            Vector2 pivotOffset = new Vector2(0.5f, 0f) - rectTransform.pivot;
            rectTransform.pivot = new Vector2(0.5f, 0f);
            rectTransform.anchoredPosition += new Vector2(pivotOffset.x * rectTransform.rect.width, pivotOffset.y * rectTransform.rect.height);
        }

        originalPosition = rectTransform.anchoredPosition;
    }

    private void Start()
    {
        StartIdleAnimation();
    }

    private void StartIdleAnimation()
    {
        switch (motionType)
        {
            case MotionType.None:
                PlayNoneMotion();
                break;
            case MotionType.Bouncy:
                PlayBouncyMotion();
                break;
            case MotionType.Sway:
                PlaySwayMotion();
                break;
            case MotionType.Shiver:
                PlayShiverMotion();
                break;
        }
    }

    // [버튼 연결용 함수] 생성한 투명 버튼(Hitbox)의 On Click() 에 이 함수를 등록해 주세요.
    public void OnClickCharacter()
    {
        if (isInteracting) return;
        isInteracting = true;

        currentSequence?.Kill();
        rectTransform.DOKill();

        if (motionType == MotionType.Bouncy)
        {
            PlayHighJumpOnClick();
        }
        else
        {
            PlaySquashOnClick();
        }
    }

    // 0. 평상시 정지 상태
    private void PlayNoneMotion()
    {
        rectTransform.DOKill();
        rectTransform.anchoredPosition = originalPosition;
        rectTransform.localRotation = Quaternion.identity;
        rectTransform.localScale = Vector3.one;
    }

    // [클릭 1] 높고 빠르게 점프
    private void PlayHighJumpOnClick()
    {
        float highJumpHeight = 35f;
        float fastJumpDuration = 0.15f;

        rectTransform.localScale = Vector3.one;

        Sequence jumpSequence = DOTween.Sequence();
        jumpSequence.Append(rectTransform.DOAnchorPosY(originalPosition.y + highJumpHeight, fastJumpDuration).SetEase(Ease.OutQuad));
        jumpSequence.Append(rectTransform.DOAnchorPosY(originalPosition.y, fastJumpDuration).SetEase(Ease.InQuad));

        jumpSequence.OnComplete(() =>
        {
            isInteracting = false;
            StartIdleAnimation();
        });
    }

    // [클릭 2] 제자리에서 빠르게 작아졌다가 뿅 하고 펴짐
    private void PlaySquashOnClick()
    {
        Vector3 smallScale = new Vector3(0.8f, 0.8f, 1f); // 전체적으로 20% 작아짐
        float shrinkDuration = 0.06f;                    // 아주 빠르게 쑥 작아짐
        float restoreDuration = 0.14f;                   // 뿅 하고 탱탱하게 원복

        Sequence scaleSequence = DOTween.Sequence();
        scaleSequence.Append(rectTransform.DOScale(smallScale, shrinkDuration).SetEase(Ease.OutQuad));
        scaleSequence.Append(rectTransform.DOScale(Vector3.one, restoreDuration).SetEase(Ease.OutBack));

        scaleSequence.OnComplete(() =>
        {
            isInteracting = false;
            StartIdleAnimation();
        });
    }

    private void PlayBouncyMotion()
    {
        float jumpHeight = 8f;
        float jumpDuration = 0.3f;
        float restDuration = 0.15f;

        rectTransform.DOKill();
        rectTransform.localScale = Vector3.one;

        currentSequence = DOTween.Sequence();
        currentSequence.Append(rectTransform.DOAnchorPosY(originalPosition.y + jumpHeight, jumpDuration).SetEase(Ease.OutQuad));
        currentSequence.Append(rectTransform.DOAnchorPosY(originalPosition.y, jumpDuration).SetEase(Ease.InQuad));
        currentSequence.AppendInterval(restDuration);
        currentSequence.SetLoops(-1);
    }

    private void PlaySwayMotion()
    {
        float tiltAngle = 4f;
        float duration = 3.0f;

        rectTransform.DOKill();
        rectTransform.localRotation = Quaternion.Euler(0f, 0f, -tiltAngle);

        rectTransform.DOLocalRotate(new Vector3(0f, 0f, tiltAngle), duration)
            .SetEase(Ease.InOutSine)
            .SetLoops(-1, LoopType.Yoyo);
    }

    private void PlayShiverMotion()
    {
        float shakeStrength = 3f;
        float duration = 0.1f;

        rectTransform.DOKill();
        rectTransform.localRotation = Quaternion.identity;
        rectTransform.localScale = Vector3.one;

        rectTransform.DOAnchorPosX(originalPosition.x + shakeStrength, duration)
            .SetEase(Ease.Linear)
            .SetLoops(-1, LoopType.Yoyo);
    }

    private void OnDestroy()
    {
        currentSequence?.Kill();
        rectTransform?.DOKill();
    }
}