using UnityEngine;
using DG.Tweening;

public class CutsceneImage : MonoBehaviour
{
    [SerializeField] private float appearDuration = 1f;
    [SerializeField] private SpriteRenderer shadow;
    [SerializeField] private float shadowDelay = 0.3f;
    [SerializeField] private float shadowAlpha = 0.4f;

    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public Tween PlayAppear()
    {
        Color color = spriteRenderer.color;
        color.a = 0f;
        spriteRenderer.color = color;

        Sequence sequence = DOTween.Sequence();

        sequence.Join(
            spriteRenderer
                .DOFade(1f, appearDuration)
                .SetEase(Ease.Linear)
        );

        if (shadow != null)
        {
            Color shadowColor = shadow.color;
            shadowColor.a = 0f;
            shadow.color = shadowColor;

            sequence.Insert(
                shadowDelay,
                shadow
                    .DOFade(shadowAlpha, appearDuration)
                    .SetEase(Ease.Linear)
            );
        }

        return sequence;
    }
}