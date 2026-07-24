using UnityEngine;
using UnityEngine.InputSystem;
using Wakamole.Lyeon.UI;

namespace Wakamole.Lyeon.Player
{
    public class PlayerManager : MonoBehaviour
    {
        public static PlayerManager Current { get; private set; }

        [Header("Components")]
        [Tooltip("차지 공격 상태를 표시할 ProgressBar 스크립트를 포함한 GameObject입니다.")]
        [SerializeField] private ProgressBar progressBar;

        [Header("Information")]
        [Tooltip("플레이어의 공격력입니다.")]
        [SerializeField] private int atk = 1;
        [Tooltip("차지 공격에 필요한 준비 시간입니다.")]
        [SerializeField] private float chargeTime = 2.0f;
        [Tooltip("차지 공격 시 데미지 배율입니다.")]
        [SerializeField] private float chargeRatio = 1.5f;

        private bool activeCharge = false;
        private float chargedCount = 0;

        /// <summary>
        /// 플레이어의 공격력입니다.
        /// </summary>
        public int Atk { get => atk; set => atk = value; }
        /// <summary>
        /// 차지 공격 활성화 여부입니다.
        /// </summary>
        public bool Charged
        {
            get => chargedCount == chargeTime;
            set
            {
                if (value) chargedCount = chargeTime;
                else chargedCount = 0;
            }
        }
        public float ChargeRatio { get => chargeRatio; set => chargeRatio = value; }

        private void Awake()
        {
            if (Current != null)
            {
                Destroy(gameObject);
                return;
            }

            Current = this;
            DontDestroyOnLoad(gameObject);
        }

        private void Update()
        {
            if (Mouse.current.rightButton.wasPressedThisFrame) activeCharge = true;
            else if (Mouse.current.rightButton.wasReleasedThisFrame) activeCharge = false;

            if (activeCharge && chargedCount < chargeTime) chargedCount += Time.deltaTime;
            if (chargedCount > chargeTime) chargedCount = chargeTime;

            progressBar.Value = chargedCount / chargeTime;
        }
    }
}