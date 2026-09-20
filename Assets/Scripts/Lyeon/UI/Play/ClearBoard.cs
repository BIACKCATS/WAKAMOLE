using TMPro;
using UnityEngine;

namespace Wakamole.Lyeon.UI.Play
{
    public class ClearBoard : MonoBehaviour
    {
        [Header("Components")]
        [Tooltip("Canvas의 CanvasGroup 컴포넌트입니다.")]
        [SerializeField] private CanvasGroup canvasGroup;
        [Tooltip("게임 성공/실패 여부를 표시할 텍스트입니다.")]
        [SerializeField] private TMP_Text titleText;
        [Tooltip("획득한 코인을 표시할 텍스트입니다.")]
        [SerializeField] private TMP_Text coinText;
        [Tooltip("잡은 두더지의 수를 표시할 텍스트입니다.")]
        [SerializeField] private TMP_Text moleText;
        [Tooltip("최종 점수를 표시할 텍스트입니다.")]
        [SerializeField] private TMP_Text scoreText;
        [Tooltip("게임 클리어 시 표시할 메뉴입니다.")]
        [SerializeField] private GameObject winMenu;
        [Tooltip("게임 오버 시 표시할 메뉴입니다.")]
        [SerializeField] private GameObject loseMenu;

        private bool isWin = true;

        public int Coin { set => coinText.text = value.ToString(); }
        public int Mole { set => moleText.text = value.ToString(); }
        public int Score { set => scoreText.text = value.ToString(); }
        public bool GameClear { set => isWin = value; }

        private bool showEvent = false;

        private void Awake()
        {
            canvasGroup.alpha = 0;
        }

        private void OnEnable()
        {
            canvasGroup.alpha = 0;
            showEvent = true;

            if (isWin) titleText.text = "이겼다!";
            else titleText.text = "놓쳤다...";
            winMenu.SetActive(isWin);
            loseMenu.SetActive(!isWin);
        }

        private void Update()
        {
            if (showEvent)
            {
                canvasGroup.alpha = Mathf.Lerp(canvasGroup.alpha, 1, 15.0f * Time.deltaTime);
                if (canvasGroup.alpha <= 0.99)
                {
                    canvasGroup.alpha = 1;
                    showEvent = false;
                }
            }
        }
    }
}