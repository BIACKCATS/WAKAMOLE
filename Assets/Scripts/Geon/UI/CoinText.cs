using TMPro;
using UnityEngine;
using DG.Tweening;

namespace Wakamole.Lyeon.UI.Play
{
    public class CoinText : MonoBehaviour
    {
        [Header("Number")]
        [SerializeField] private TMP_Text coinText;
        [SerializeField] private TMP_Text nextCoinText;

        [Header("Coin Icon")]
        [SerializeField] private RectTransform coinIcon;

        [Header("Animation")]
        [SerializeField] private float moveDistance = 50f;
        [SerializeField] private float moveDuration = 0.25f;

        private int displayedCoin = 0;
        private int targetCoin = 0;
        private bool isAnimating = false;

        public int Coin
        {
            set
            {
                targetCoin = value;

                if (value == displayedCoin)
                    return;

                // 이미 애니메이션 중이라면
                // 현재 애니메이션은 그대로 끝내고 최신 targetCoin으로 이어서 진행
                if (isAnimating)
                    return;

                PlayCoinAnimation();
                PlayCoinIconAnimation();
            }
        }

        private void PlayCoinAnimation()
        {
            if (isAnimating)
                return;

            isAnimating = true;

            // 애니메이션 시작 시점의 목표값을 따로 저장
            int animationTarget = targetCoin;

            // 증가인지 감소인지 결정
            float direction = animationTarget > displayedCoin ? 1f : -1f;

            RectTransform currentRect = coinText.rectTransform;
            RectTransform nextRect = nextCoinText.rectTransform;

            // 기존 Tween 제거
            currentRect.DOKill();
            nextRect.DOKill();

            // 다음 숫자 설정
            nextCoinText.text = animationTarget.ToString();

            // 초기 위치
            currentRect.anchoredPosition = Vector2.zero;

            // 증감에 따른 방향
            nextRect.anchoredPosition =
                new Vector2(0f, -moveDistance * direction);

            Sequence sequence = DOTween.Sequence();

            // 현재 숫자가 빠져나감
            sequence.Join(
                currentRect
                    .DOAnchorPosY(moveDistance * direction, moveDuration)
                    .SetEase(Ease.InCubic)
            );

            // 다음 숫자가 들어옴
            sequence.Join(
                nextRect
                    .DOAnchorPosY(0f, moveDuration)
                    .SetEase(Ease.OutCubic)
            );

            sequence.OnComplete(() =>
            {
                // 이번 애니메이션의 목표값을 실제 표시값으로 확정
                displayedCoin = animationTarget;
                coinText.text = displayedCoin.ToString();

                // 위치 초기화
                currentRect.anchoredPosition = Vector2.zero;
                nextRect.anchoredPosition =
                    new Vector2(0f, -moveDistance);

                isAnimating = false;

                // 애니메이션 도중 targetCoin이 변경됐다면
                // 최신 값까지 이어서 애니메이션
                if (displayedCoin != targetCoin)
                {
                    PlayCoinAnimation();
                    PlayCoinIconAnimation();
                }
            });
        }

        private void PlayCoinIconAnimation()
        {
            if (coinIcon == null)
                return;

            coinIcon.DOKill();
            coinIcon.localRotation = Quaternion.identity;

            Sequence sequence = DOTween.Sequence();

            sequence.Append(
                coinIcon.DORotate(
                    new Vector3(0f, 0f, -8f),
                    0.06f
                )
            );

            sequence.Append(
                coinIcon.DORotate(
                    new Vector3(0f, 0f, 8f),
                    0.12f
                )
            );

            sequence.Append(
                coinIcon.DORotate(
                    Vector3.zero,
                    0.06f
                )
            );
        }
    }
}