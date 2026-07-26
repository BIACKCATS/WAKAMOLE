using UnityEngine;
using Wakamole.Lyeon.Player;

namespace Wakamole.Lyeon.Entity.Component
{
    public class MoleCharactor : MonoBehaviour
    {
        [Header("Components")]
        [Tooltip("두더지를 관리하는 스크립트를 포함한 GameObject입니다.")]
        [SerializeField] private Mole mole;

        [Header("Information")]
        [Tooltip("두더지가 올라오는 시간입니다.")]
        [SerializeField] private float showTime = 3.0f;
        [Tooltip("두더지가 올라오고 내려가는 속도입니다.")]
        [SerializeField] private float moveSpeed = 8.0f;

        private bool active = false;
        private Vector3 initPosition = Vector3.zero;
        private Vector3 movePosition = new(0, 0.1f, 0);
        private Vector3 targetPosition = Vector3.zero;

        private float showedTime = 0;

        /// <summary>
        /// 두더지가 올라오는 시간입니다.
        /// </summary>
        public float ShowTime { get => showTime; set => showTime = value; }

        /// <summary>
        /// 두더지가 활성화된 상태입니다.
        /// </summary>
        public bool Active
        {
            get => active;
            set
            {
                active = value;
                if (active) targetPosition = movePosition;
                else
                {
                    targetPosition = initPosition;
                    showedTime = 0;
                }
            }
        }

        private void Awake()
        {
            initPosition = transform.localPosition;
        }

        private void OnEnable()
        {
            Active = true;
        }

        private void Update()
        {
            if (!active) return;

            showedTime += Time.deltaTime;
            if (showedTime >= showTime) Active = false;
        }

        private void FixedUpdate()
        {
            if (targetPosition == initPosition && Vector3.Distance(transform.localPosition, targetPosition) < 0.01f)
            {
                mole.gameObject.SetActive(false);
                return;
            }
                        
            transform.localPosition = Vector3.Lerp(transform.localPosition, targetPosition, Time.fixedDeltaTime * moveSpeed);
        }

        private void OnMouseDown()
        {
            if (!active) return;
            if (PlayerManager.Current.Charged)
            {
                mole.Hp -= PlayerManager.Current.Atk * (int)PlayerManager.Current.ChargeRatio;
                PlayerManager.Current.Charged = false;
            }
            else mole.Hp -= PlayerManager.Current.Atk;
            
            if (mole.Hp <= 0)
            {
                PlayerManager.Current.Score++;
                targetPosition = initPosition;
                active = false;
            }
        }
    }
}