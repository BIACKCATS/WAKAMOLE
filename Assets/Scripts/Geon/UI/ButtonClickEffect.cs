using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ButtonClickEffect : MonoBehaviour
{
    [SerializeField] private float pressedScale = 0.92f;
    [SerializeField] private float pressDuration = 0.06f;
    [SerializeField] private float releaseDuration = 0.1f;

    private Vector3 originalScale;

    private void Awake()
    {
        originalScale = transform.localScale;
    }

    public void PlayEffect()
    {
        transform.DOKill();

        transform.localScale = originalScale;

        transform.DOScale(originalScale * pressedScale, pressDuration)
            .SetEase(Ease.OutQuad)
            .OnComplete(() =>
            {
                transform.DOScale(originalScale, releaseDuration)
                    .SetEase(Ease.OutBack);
            });
    }
}