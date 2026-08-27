using UnityEngine;
using TMPro;
using DG.Tweening;

public class WinUI : MonoBehaviour
{
    [Header("Text UI References")]
    [SerializeField] private TMP_Text coinText;
    [SerializeField] private TMP_Text moleText;
    [SerializeField] private TMP_Text totalText;

    [Header("Animation Settings")]
    [SerializeField] private float charPerSecond = 15f; // 초당 출력할 글자 수
    [SerializeField] private float delayBetweenTexts = 0.2f; // 각 텍스트 연출 사이 간격

    private Sequence clearSequence;

    public void ShowClearWindow(int coin, int mole, int total)
    {
        // 1. 기존 트윈 초기화
        clearSequence?.Kill();

        // 2. 텍스트 데이터 세팅 및 가시성 0으로 초기화
        coinText.text = $"Coin: {coin}";
        moleText.text = $"Mole: {mole}";
        totalText.text = $"Total: {total}";

        coinText.maxVisibleCharacters = 0;
        moleText.maxVisibleCharacters = 0;
        totalText.maxVisibleCharacters = 0;

        // 3. DOTween Sequence 연출
        clearSequence = DOTween.Sequence();

        clearSequence.Append(CreateTypingTween(coinText))
                     .AppendInterval(delayBetweenTexts)
                     .Append(CreateTypingTween(moleText))
                     .AppendInterval(delayBetweenTexts)
                     .Append(CreateTypingTween(totalText));
    }

    private Tween CreateTypingTween(TMP_Text targetText)
    {
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

    private void OnDestroy()
    {
        clearSequence?.Kill();
    }
}