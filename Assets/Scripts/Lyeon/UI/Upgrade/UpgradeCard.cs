using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using Wakamole.Core.LocalData;
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

        [Header("Variables")]
        [SerializeField] private Color cardColor;
        [SerializeField] private MoleKeyword keyword;

        private bool init = false; // 화면 표시 상태
        private bool active = false; // 선택 가능 상태
        private bool animate = false; // 애니메이션 진행 중
        private int maxCount = 0; // 해당 키워드를 가질 수 있는 두더지의 수
        private int bonusCoin = 0; // 해당 강화창 선택 시 획득 가능한 코인 수

        private float targetAlpha = 0, targetScale = 1.0f;
        private Vector3 initPosition = Vector3.zero, targetPosition = Vector3.zero;

        public bool Active => gameObject.activeSelf && init;

        /// <summary>
        /// Card의 상태를 초기화합니다.
        /// </summary>
        public void Init(MoleData data)
        {
            // Initialize Booleans
            init = false;
            active = false;
            animate = false;

            // Initialize variables
            maxCount = Random.Range(1, GameManager.Current.MaxKeywordCount);
            bonusCoin = (data.score != 0) ? maxCount * data.score : maxCount;

            targetAlpha = 1;
            targetScale = 1.0f;
            targetPosition = initPosition;
            rect.position = new Vector3(initPosition.x, -640.0f, initPosition.z);

            // Initialize Keyword
            keyword = data.keyword;
            title.text = data.moleName;
            desc.text = string.Format("랜덤한 {0}마리의 두더지에 <color=#{1}>{2}</color> 키워드가 추가로 붙습니다.\n해당 키워드의 두더지는 {3}\n\n추가로 코인이 {4}개 지급됩니다.", maxCount.ToString(), cardColor.ToHexString(), data.moleName, data.moleDesc, 
                bonusCoin.ToString());
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
            GameManager.Current.Coin += bonusCoin;
            GameManager.Current.SetIncludeKeyword(keyword);
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