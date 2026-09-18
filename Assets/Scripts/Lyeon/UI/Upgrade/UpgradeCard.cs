using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using Wakamole.Lyeon.Entity;
using Wakamole.Lyeon.Manager.Game;
using Wakamole.Lyeon.Manager.Upgrade;

namespace Wakamole.Lyeon.UI.Upgrade
{
    public class UpgradeCard : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [Header("Components")]
        [SerializeField] private RectTransform rect;
        [SerializeField] private CanvasGroup group;
        [SerializeField] private TMP_Text title, desc;
        
        [Header("Managers")]
        [SerializeField] private UpgradeManager upgrade;
        [SerializeField] private GameManager game;

        private bool init = false; // 화면 표시 상태
        private bool active = false; // 선택 가능 상태
        private bool animate = false; // 애니메이션 진행 중

        private float targetAlpha = 0, targetScale = 1.0f;
        private Vector3 targetPosition = Vector3.zero;

        private MoleKeyword keyword;

        public bool Active => gameObject.activeSelf && init;
        public void SetActive(bool active)
        {
            if (active)
            {
                // 이미 초기화가 된 상태인 경우 시작 애니메이션을 시작하지 않음 (이미 화면에 표시되어 있음)
                if (init) return;

                // Initialize variables
                targetAlpha = 1;
                targetScale = 1.0f;
                targetPosition = rect.position;

                // Initialize 
                Vector3 movePosition = rect.position;
                movePosition.y -= 640.0f;
                rect.position = movePosition;
                group.alpha = 0;
                animate = true;
            }
            else
            {
                // 이미 초기화가 되지 않은 상태인 경우 시작 애니메이션을 시작하지 않음 (이미 화면에 표시되지 않음)
                if (!init) return;
            }

            this.active = active;
        }

        /// <summary>
        /// 해당 카드가 가질 키워드를 설정합니다. 키워드는 하나만 설정해야합니다.
        /// </summary>
        /// <param name="keyword">설정할 키워드입니다.</param>
        public void SetKeyword(MoleKeyword keyword)
        {
            string upgradeKeyword = "";
            switch (keyword)
            {
                case MoleKeyword.FAST:
                    upgradeKeyword = "빠른";
                    break;
                case MoleKeyword.POPULAR:
                    upgradeKeyword = "인싸";
                    break;
                case MoleKeyword.REVIVE:
                    upgradeKeyword = "부활하는";
                    break;
                case MoleKeyword.RICH:
                    upgradeKeyword = "부자";
                    break;
                case MoleKeyword.SHIELD:
                    upgradeKeyword = "방패";
                    break;
                case MoleKeyword.SPLIT:
                    upgradeKeyword = "분열하는";
                    break;
                case MoleKeyword.STRONG:
                    upgradeKeyword = "단단한";
                    break;
            }
            title.text = upgradeKeyword;
            desc.text = string.Format(desc.text, upgradeKeyword);
            this.keyword = keyword;
        }

        public void ShowCard()
        {
            if (init) return;
        }

        public void SelectCard()
        {
            
        }

        public void HideCard()
        {
            
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (active) targetScale = 1.1f;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (active) targetScale = 1.0f;
        }

        private void Update()
        {
            // scale의 경우 hover event가 있기에 항상 작동
            rect.localScale = Vector3.Lerp(rect.localScale, Vector3.one * targetScale, 15.0f * Time.deltaTime);

            if (!animate) return;
            group.alpha = Mathf.Lerp(group.alpha, targetAlpha, 15.0f * Time.deltaTime);
            rect.position = Vector3.Lerp(rect.position, targetPosition, 15.0f * Time.deltaTime);
        }
    }
}