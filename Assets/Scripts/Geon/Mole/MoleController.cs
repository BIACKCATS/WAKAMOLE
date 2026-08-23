using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Wakamole.Core.LocalData;

[RequireComponent(typeof(SpriteRenderer))]
public class MoleController : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private Dictionary<string, MoleAnimClip> clipDict = new Dictionary<string, MoleAnimClip>();

    private MoleAnimClip currentClip;
    private int currentFrameIndex;
    private float frameTimer;
    private bool isTriggerFired;

    // 프레임 트리거 발생 시 실행할 외부 이벤트 (사운드, 이펙트 등)
    public UnityEvent<string, int> OnAnimationTrigger;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void Initialize(MoleData data)
    {
        clipDict.Clear();
        foreach (var clip in data.animClips)
        {
            if (!clipDict.ContainsKey(clip.animName))
                clipDict.Add(clip.animName, clip);
        }

        PlayAnimation("Idle"); // 기본 애니메이션 실행
    }

    public void PlayAnimation(string animName)
    {
        if (!clipDict.TryGetValue(animName, out MoleAnimClip newClip)) return;

        currentClip = newClip;
        currentFrameIndex = 0;
        frameTimer = 0f;
        isTriggerFired = false;

        if (currentClip.frames != null && currentClip.frames.Length > 0)
            spriteRenderer.sprite = currentClip.frames[0];
    }

    private void Update()
    {
        if (currentClip == null || currentClip.frames == null || currentClip.frames.Length == 0) return;

        frameTimer += Time.deltaTime;
        float frameInterval = 1f / currentClip.fps;

        if (frameTimer >= frameInterval)
        {
            frameTimer -= frameInterval;
            currentFrameIndex++;

            // 1. 프레임 트리거 체크
            if (currentFrameIndex == currentClip.triggerFrame && !isTriggerFired)
            {
                isTriggerFired = true;
                OnAnimationTrigger?.Invoke(currentClip.animName, currentFrameIndex);
            }

            // 2. 애니메이션 종료 처리
            if (currentFrameIndex >= currentClip.frames.Length)
            {
                if (currentClip.isLoop)
                {
                    currentFrameIndex = 0;
                    isTriggerFired = false; // 반복 재생 시 트리거 리셋
                }
                else
                {
                    // 다음 애니메이션이 지정되어 있다면 자동 전환 (예: Hit -> Idle)
                    if (!string.IsNullOrEmpty(currentClip.nextAnimName))
                    {
                        PlayAnimation(currentClip.nextAnimName);
                        return;
                    }
                    currentFrameIndex = currentClip.frames.Length - 1; // 마지막 프레임 고정
                }
            }

            spriteRenderer.sprite = currentClip.frames[currentFrameIndex];
        }
    }
}