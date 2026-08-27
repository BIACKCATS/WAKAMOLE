using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
using FMODUnity;
using FMOD.Studio; // [수정] EventInstance 사용을 위해 추가
using Wakamole.Core.LocalData;

public class PopUpBoard : MonoBehaviour
{
    [Header("UI Objects")]
    [SerializeField] private GameObject clearBoard;

    [Header("Text UI List")]
    [SerializeField] private List<TMP_Text> textList;

    [Header("Audio Settings")]
    [SerializeField] private EventReference typeSfxEvent;
    [SerializeField] private SoundParam[] typeSfxParams; // 인스펙터에서 파라미터 설정

    [Header("Animation Settings")]
    [SerializeField] private float popUpDuration = 0.6f;
    [SerializeField] private float charPerSecond = 15f;
    [SerializeField] private float delayBetweenTexts = 0.15f;

    private Sequence clearSequence;

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

        clearBoard.transform.localScale = Vector3.zero;

        foreach (var text in textList)
        {
            if (text != null) text.maxVisibleCharacters = 0;
        }

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

    // [수정] FMOD 인스턴스를 직접 생성하여 인스펙터의 파라미터를 바인딩한 뒤 재생
    private void PlayTypeSound()
    {
        if (typeSfxEvent.IsNull) return;

        try
        {
            EventInstance instance = RuntimeManager.CreateInstance(typeSfxEvent);
            instance.set3DAttributes(RuntimeUtils.To3DAttributes(transform.position));

            // 인스펙터에 설정된 SoundParam 항목들을 FMOD 파라미터로 설정
            if (typeSfxParams != null && typeSfxParams.Length > 0)
            {
                foreach (var param in typeSfxParams)
                {
                    if (!string.IsNullOrEmpty(param.name))
                    {
                        instance.setParameterByName(param.name, param.value);
                    }
                }
            }

            instance.start();
            instance.release(); // 재생 완료 후 자동 메모리 해제
        }
        catch (System.Exception e)
        {
            Debug.LogError($"[{gameObject.name}] 사운드 재생 중 에러 발생: {e.Message}");
        }
    }

    private void OnDisable() => clearSequence?.Kill();
    private void OnDestroy() => clearSequence?.Kill();
}