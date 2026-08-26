using UnityEngine;
using TMPro;
using DG.Tweening;
using System.Collections.Generic;

public class PopUpBoard : MonoBehaviour
{
    [Header("UI Objects")]
    [SerializeField] private GameObject clearBoard;

    [Header("Text UI List")]
    // 인스펙터에서 연출하고 싶은 TMP 오브젝트들을 순서대로 넣어두면 됩니다.
    [SerializeField] private List<TMP_Text> textList;

    [Header("Animation Settings")]
    [SerializeField] private float popUpDuration = 0.6f;
    [SerializeField] private float charPerSecond = 15f;
    [SerializeField] private float delayBetweenTexts = 0.15f;

    private Sequence clearSequence;

    // 오브젝트가 SetActive(true)로 켜질 때마다 자동으로 실행됩니다.
    private void OnEnable()
    {
        PlayClearAnimation();
    }

    public void PlayClearAnimation()
    {
        // 1. 기존 연출 정리
        clearSequence?.Kill();

        // 2. 초기 상태 설정 (창 크기 0, 글자 숨김)
        clearBoard.transform.localScale = Vector3.zero;

        foreach (var text in textList)
        {
            if (text != null)
            {
                text.maxVisibleCharacters = 0;
            }
        }

        // 3. 연출 시퀀스 연결 및 실행
        clearSequence = DOTween.Sequence();

        // Step 1: 창 팝업 연출
        clearSequence.Append(clearBoard.transform.DOScale(Vector3.one, popUpDuration).SetEase(Ease.OutBack));

        // Step 2: 리스트에 등록된 텍스트 수만큼 순차 타이핑 연출
        foreach (var text in textList)
        {
            if (text != null)
            {
                clearSequence.Append(CreateTypingTween(text))
                             .AppendInterval(delayBetweenTexts);
            }
        }
    }

    private Tween CreateTypingTween(TMP_Text targetText)
    {
        // 켜지는 순간 TMP의 글자 수 정보를 즉시 강제 갱신 (0으로 계산되는 버그 방지)
        targetText.ForceMeshUpdate();

        int totalChars = targetText.textInfo.characterCount;
        if (totalChars == 0) totalChars = targetText.text.Length;

        float duration = totalChars / charPerSecond;

        return DOTween.To(
            () => targetText.maxVisibleCharacters,
            x => targetText.maxVisibleCharacters = x,
            totalChars,
            duration
        ).SetEase(Ease.Linear);
    }

    private void OnDisable()
    {
        // 창이 꺼질 때 연출 깔끔히 정지
        clearSequence?.Kill();
    }

    private void OnDestroy()
    {
        clearSequence?.Kill();
    }

}