using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
using Wakamole.Lyeon.Audio;
using Wakamole.Core.LocalData;

public class PopUpBoard : MonoBehaviour
{
    [Header("UI Objects")]
    [SerializeField] private GameObject clearBoard;

    [Header("Text UI List")]
    [SerializeField] private List<TMP_Text> textList;

    [Header("Audio Settings")]
    [SerializeField] private AudioManager audioManager;
    [SerializeField] private string typeSfxName = "Typing";
    [SerializeField] private SoundParam[] typeSfxParams; // 추가적인 개별 파라미터 덮어쓰기용

    [Header("Animation Settings")]
    [SerializeField] private float popUpDuration = 0.6f;
    [SerializeField] private float charPerSecond = 15f;
    [SerializeField] private float delayBetweenTexts = 0.15f;

    private Sequence clearSequence;

    private void Awake()
    {
        if (audioManager == null)
        {
            audioManager = FindFirstObjectByType<AudioManager>();
        }
    }

    private void OnEnable()
    {
        PlayClearAnimation();
    }

    public void PlayClearAnimation()
    {
        if (clearBoard == null)
        {
            Debug.LogError($"[{gameObject.name}] clearBoard가 인스펙터에 할당되지 않았습니다!");
            return;
        }

        clearSequence?.Kill();

        // 1. 초기 상태 설정
        clearBoard.transform.localScale = Vector3.zero;

        foreach (var text in textList)
        {
            if (text != null) text.maxVisibleCharacters = 0;
        }

        // 2. 연출 시퀀스 구성
        clearSequence = DOTween.Sequence();
        clearSequence.SetLink(gameObject);

        clearSequence.Append(clearBoard.transform.DOScale(Vector3.one, popUpDuration).SetEase(Ease.OutBack));

        foreach (var text in textList)
        {
            if (text != null)
            {
                Tween typingTween = CreateTypingTween(text);
                if (typingTween != null)
                {
                    clearSequence.Append(typingTween)
                                 .AppendInterval(delayBetweenTexts);
                }
            }
        }
    }

    private Tween CreateTypingTween(TMP_Text targetText)
    {
        if (targetText == null || string.IsNullOrEmpty(targetText.text)) return null;

        targetText.ForceMeshUpdate();

        int totalChars = targetText.textInfo.characterCount;
        if (totalChars == 0) totalChars = targetText.text.Length;

        targetText.maxVisibleCharacters = 0;

        float duration = totalChars / Mathf.Max(1f, charPerSecond);
        int lastVisibleCount = 0;

        return DOTween.To(
            () => 0,
            x => {
                if (targetText == null) return;

                targetText.maxVisibleCharacters = x;

                if (x > lastVisibleCount)
                {
                    PlayTypeSound();
                    lastVisibleCount = x;
                }
            },
            totalChars,
            duration
        )
        .SetEase(Ease.Linear)
        .SetLink(targetText.gameObject);
    }

    private void PlayTypeSound()
    {
        if (audioManager == null)
        {
            Debug.LogWarning($"[{gameObject.name}] AudioManager를 찾을 수 없어 사운드를 재생하지 못했습니다.");
            return;
        }

        if (string.IsNullOrEmpty(typeSfxName)) return;

        try
        {
            // AudioManager 내부에서 paramData의 "Typing" 기본 파라미터가 자동으로 적용되며,
            // typeSfxParams에 추가 설정이 있을 경우 덮어씌워서 재생합니다.
            if (typeSfxParams != null && typeSfxParams.Length > 0)
            {
                audioManager.PlaySfx(typeSfxName, typeSfxParams);
            }
            else
            {
                audioManager.PlaySfx(typeSfxName);
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError($"[{gameObject.name}] 사운드 재생 중 에러 발생 (키 이름: {typeSfxName}): {e.Message}");
        }
    }

    private void OnDisable() => clearSequence?.Kill();
    private void OnDestroy() => clearSequence?.Kill();
}