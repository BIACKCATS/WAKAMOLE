using TMPro;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Wakamole.Lyeon.Manager.Game;
using Wakamole.Lyeon.Manager.Shop;

public class RerollController : MonoBehaviour
{
    [SerializeField] private Button mainButton;    // 기존 버튼
    [SerializeField] private Button imageButton;   // Image에 추가한 버튼
    [SerializeField] private TMP_Text rerollText;
    [SerializeField] private int rerollCost;
    [SerializeField] private Booth booth;

    [SerializeField] private Image targetImage;      // 변경할 UI Image 컴포넌트
    [SerializeField] private Sprite originalSprite; // 원본 이미지
    [SerializeField] private Sprite changedSprite;  // 잠시 변경될 이미지

    private Coroutine changeCoroutine;

    private void Awake()
    {
        rerollText.text = $"{rerollCost}코인";
    }

    private void OnEnable()
    {
        rerollCost = 1;
    }

    private void Start()
    {
        // 둘 중 무엇을 눌러도 OnRerollClicked 함수가 실행되도록 연결
        if (mainButton != null)
            mainButton.onClick.AddListener(OnRerollClicked);

        if (imageButton != null)
            imageButton.onClick.AddListener(OnRerollClicked);

        UpdateUI();
    }

    public void OnRerollClicked()
    {
        if (GameManager.Current.Coin < rerollCost) return;

        GameManager.Current.Coin -= rerollCost;

        // 리롤 시 비용 증가
        rerollCost *= 2;

        // UI 갱신 
        UpdateUI();

        OnButtonClick();
        booth.Reroll();
    }

    private void OnButtonClick()
    {
        // 빠르게 연타할 경우 이전 타이머 초기화
        if (changeCoroutine != null)
        {
            StopCoroutine(changeCoroutine);
        }
        changeCoroutine = StartCoroutine(ChangeImageRoutine());
    }

    private void UpdateUI()
    {
        if (rerollText != null)
        {
            rerollText.text = $"{rerollCost}코인";
        }
    }

    private IEnumerator ChangeImageRoutine()
    {
        targetImage.sprite = changedSprite;
        yield return new WaitForSeconds(0.5f);
        targetImage.sprite = originalSprite;
    }
}