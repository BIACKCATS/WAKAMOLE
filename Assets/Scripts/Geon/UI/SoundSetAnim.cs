using UnityEngine;
using DG.Tweening;

namespace Wakamole.Lyeon.UI
{
    [RequireComponent(typeof(CanvasGroup))]
    public class SoundSetAnim : MonoBehaviour
    {
        [SerializeField] private CanvasGroup canvasGroup;
        [SerializeField] private RectTransform windowTransform;
        [SerializeField] private float duration = 0.35f;

        private bool isOpen = false;

        private void Awake()
        {
            if (canvasGroup == null) canvasGroup = GetComponent<CanvasGroup>();
            if (windowTransform == null) windowTransform = GetComponent<RectTransform>();

            gameObject.SetActive(false);
        }

        public void ToggleWindow()
        {
            if (isOpen) Close();
            else Open();
        }

        public void Open()
        {
            isOpen = true;
            gameObject.SetActive(true);

            windowTransform.DOKill();
            canvasGroup.DOKill();

            Time.timeScale = 0f; // 게임 일시정지

            windowTransform.localScale = Vector3.one * 0.3f;
            canvasGroup.alpha = 0f;

            windowTransform.DOScale(Vector3.one, duration).SetEase(Ease.OutBack).SetUpdate(true);
            canvasGroup.DOFade(1f, duration).SetUpdate(true);
        }

        public void Close()
        {
            isOpen = false;

            windowTransform.DOKill();
            canvasGroup.DOKill();

            Time.timeScale = 1f; // 게임 재개

            windowTransform.DOScale(Vector3.one * 0.3f, duration * 0.8f).SetEase(Ease.InBack).SetUpdate(true);
            canvasGroup.DOFade(0f, duration * 0.8f).SetUpdate(true).OnComplete(() =>
            {
                gameObject.SetActive(false);
            });
        }
    }
}