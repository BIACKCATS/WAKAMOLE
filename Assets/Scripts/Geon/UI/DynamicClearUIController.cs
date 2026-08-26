using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class DynamicClearUIController : MonoBehaviour
{
    [Header("애니메이션 설정")]
    [SerializeField] private float durationPerText = 1.0f;  // 각 텍스트 애니메이션 시간
    [SerializeField] private float delayBetweenTexts = 0.2f; // 텍스트 간 출력 간격

    [Header("대상 텍스트 (비워두면 자식 텍스트 자동 수집)")]
    [SerializeField] private List<TextMeshProUGUI> targetTexts = new List<TextMeshProUGUI>();

    // 씬에 적혀있는 원래 텍스트 내용 저장용
    private Dictionary<TextMeshProUGUI, string> originalTextCache = new Dictionary<TextMeshProUGUI, string>();

    private void Awake()
    {
        // 1. 인스펙터에 등록된 게 없으면 자식 오브젝트의 텍스트들을 전부 가져옴
        if (targetTexts == null || targetTexts.Count == 0)
        {
            targetTexts = new List<TextMeshProUGUI>(GetComponentsInChildren<TextMeshProUGUI>(true));
        }

        // 2. 인스펙터에 미리 써둔 텍스트 원본을 기억하고 화면에서는 비워둠
        foreach (var textUI in targetTexts)
        {
            if (textUI != null)
            {
                originalTextCache[textUI] = textUI.text;
                textUI.text = "";
            }
        }
    }

    // 클리어창이 열릴 때 호출
    public void PlayClearAnimation()
    {
        Sequence clearSequence = DOTween.Sequence();

        foreach (var textUI in targetTexts)
        {
            if (textUI == null || !originalTextCache.ContainsKey(textUI)) continue;

            string targetString = originalTextCache[textUI];
            textUI.text = ""; // 애니메이션 시작 직전 초기화

            // Append: 순차적으로 하나씩 연출 (하나 끝나면 다음 글자 등장)
            clearSequence.Append(textUI.DOText(targetString, durationPerText).SetEase(Ease.Linear));

            if (delayBetweenTexts > 0)
            {
                clearSequence.AppendInterval(delayBetweenTexts);
            }
        }
    }
}