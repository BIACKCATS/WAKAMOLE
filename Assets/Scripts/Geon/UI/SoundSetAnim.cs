using UnityEngine;
using DG.Tweening;

namespace Wakamole.Lyeon.UI
{
    [RequireComponent(typeof(CanvasGroup))]
    public class SoundSetAnim : MonoBehaviour
    {
        [Header("UI Components")]
        [SerializeField] private CanvasGroup canvasGroup;
        [SerializeField] private RectTransform windowTransform;
        [SerializeField] private SoundSetting soundUIController;

        [Header("Animation Settings")]
        [SerializeField] private float duration = 0.35f;

        private bool isAnimating = false;

        public bool IsOpen => gameObject.activeSelf;

        private void Awake()
        {
            if (canvasGroup == null) canvasGroup = GetComponent<CanvasGroup>();
            if (windowTransform == null) windowTransform = GetComponent<RectTransform>();

            // 주의: Awake에서 SetActive(false)를 넣지 않습니다. (첫 클릭 씹힘 원인 방지)
        }

        public void Open()
        {
            if (isAnimating || gameObject.activeSelf) return;

            // 창이 켜지기 직전, 현재 씬의 AudioManager 수치와 슬라이더 위치 동기화
            if (soundUIController != null)
            {
                soundUIController.RefreshSliders();
            }

            gameObject.SetActive(true);
            isAnimating = true;

            windowTransform.DOKill();
            canvasGroup.DOKill();

            Time.timeScale = 0f; // 게임 일시정지

            windowTransform.localScale = Vector3.one * 0.3f;
            canvasGroup.alpha = 0f;

            windowTransform.DOScale(Vector3.one, duration).SetEase(Ease.OutBack).SetUpdate(true);
            canvasGroup.DOFade(1f, duration).SetUpdate(true).OnComplete(() =>
            {
                isAnimating = false;
            });
        }

        public void Close()
        {
            if (isAnimating || !gameObject.activeSelf) return;
            isAnimating = true;

            windowTransform.DOKill();
            canvasGroup.DOKill();

            Time.timeScale = 1f; // 게임 재개

            windowTransform.DOScale(Vector3.one * 0.3f, duration * 0.8f).SetEase(Ease.InBack).SetUpdate(true);
            canvasGroup.DOFade(0f, duration * 0.8f).SetUpdate(true).OnComplete(() =>
            {
                isAnimating = false;
                gameObject.SetActive(false); // 애니메이션 완료 후 비활성화
            });
        }
    }
}