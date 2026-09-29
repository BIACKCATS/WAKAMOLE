using UnityEngine;
using DG.Tweening;

public class CutsceneImage : MonoBehaviour
{
    [SerializeField] private float appearDuration = 1f;

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

        return spriteRenderer
            .DOFade(1f, appearDuration)
            .SetEase(Ease.Linear);
    }
}