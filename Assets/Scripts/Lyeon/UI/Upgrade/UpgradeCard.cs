using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using Wakamole.Lyeon.Entity;
using Wakamole.Lyeon.Manager.Game;
using Wakamole.Lyeon.Manager.Upgrade;

namespace Wakamole.Lyeon.UI.Upgrade
{
    public class UpgradeCard : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
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
        private Vector3 initPosition = Vector3.zero, targetPosition = Vector3.zero;

        private MoleKeyword keyword;

        public bool Active => gameObject.activeSelf && init;

        /// <summary>
        /// Card의 상태를 초기화합니다.
        /// </summary>
        public void Init(MoleKeyword keyword, string name, string desc)
        {
            // Initialize Booleans
            init = false;
            active = false;
            animate = false;

            // Initialize variables
            targetAlpha = 1;
            targetScale = 1.0f;
            targetPosition = initPosition;
            rect.position = new Vector3(initPosition.x, -640.0f, initPosition.z);

            // Initialize Keyword
            this.keyword = keyword;
            this.title.text = name;
            this.desc.text = string.Format(this.desc.text, name, desc);
        }

        public void ShowCard()
        {
            init = false;
            group.alpha = 0;
            animate = true;
        }

        public void SelectCard()
        {
            active = false;
            targetScale = 1.2f;
            animate = true;
        }

        public void HideCard()
        {
            active = false;
            targetAlpha = 0;
            targetPosition = new Vector3(initPosition.x, -640.0f, initPosition.z);
            animate = true;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (active) targetScale = 1.1f;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (active) targetScale = 1.0f;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            upgrade.SelectCard(this);
            SelectCard();
        }

        private void Awake()
        {
            initPosition = rect.position;
        }

        private void Update()
        {
            // scale의 경우 hover event가 있기에 항상 작동
            rect.localScale = Vector3.Lerp(rect.localScale, Vector3.one * targetScale, 15.0f * Time.deltaTime);

            if (!animate) return;
            group.alpha = Mathf.Lerp(group.alpha, targetAlpha, 15.0f * Time.deltaTime);
            rect.position = Vector3.Lerp(rect.position, targetPosition, 15.0f * Time.deltaTime);

            if (group.alpha - targetAlpha < 0.01 && Vector3.Distance(rect.position, targetPosition) < 0.01f)
            {
                // 초기화
                if (!init) active = true;
                else if (!active) init = false;
                animate = false;
            }
        }
    }
}