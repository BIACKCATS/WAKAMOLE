using UnityEngine;
using UnityEngine.InputSystem;
using Wakamole.Lyeon.UI;

namespace Wakamole.Lyeon.Player
{
    public class PlayerManager : MonoBehaviour
    {
        public static PlayerManager Current { get; private set; }

        [Header("Components")]
        [Tooltip("클리어 시 게임 결과를 표시할 ClearBoard 스크립트를 포함한 GameObject입니다.")]
        [SerializeField] private ClearBoard clearBoard;
        [Tooltip("현재 점수를 표시할 ScoreBoard 스크립트를 포함한 GameObject입니다.")]
        [SerializeField] private ScoreBoard scoreBoard;
        [Tooltip("게임 제한 시간을 표시할 Timer 스크립트를 포함한 GameObject입니다.")]
        [SerializeField] private Timer timer;
        [Tooltip("차지 공격 상태를 표시할 ProgressBar 스크립트를 포함한 GameObject입니다.")]
        [SerializeField] private ProgressBar progressBar;

        [Header("Information")]
        [Tooltip("플레이어의 공격력입니다.")]
        [SerializeField] private int atk = 1;
        [Tooltip("차지 공격에 필요한 준비 시간입니다.")]
        [SerializeField] private float chargeTime = 2.0f;
        [Tooltip("차지 공격 시 데미지 배율입니다.")]
        [SerializeField] private float chargeRatio = 1.5f;

        [Header("Preferences")]
        [Tooltip("게임 목표 점수입니다.")]
        [SerializeField] private int goalScore = 10;
        [Tooltip("게임 제한 시간(초)입니다.")]
        [SerializeField] private float playDuration = 60.0f;

        private bool activeGame = false;
        private int coin = 0;
        private int score = 0;
        private int moleCount = 0;

        private bool activeCharge = false;
        private float chargedCount = 0;

        /// <summary>
        /// 게임의 활성 상태입니다.
        /// </summary>
        public bool Active { get => activeGame; set => activeGame = value; }
        /// <summary>
        /// 플레이어가 획득한 점수입니다.
        /// </summary>
        public int Score
        {
            get => score;
            set
            {
                score = value;
                scoreBoard.Current = score;
                if (score >= goalScore)
                {
                    activeGame = false;
                    clearBoard.gameObject.SetActive(true);
                    clearBoard.Coin = coin;
                    clearBoard.Mole = moleCount;
                    clearBoard.Score = score;
                }
            }
        }
        public int Count
        {
            get => moleCount;
            set => moleCount = value;
        }
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
        /// <summary>
        /// 차지 공격으로 입히는 데미지의 배율입니다.
        /// </summary>
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

            // 개발용
            activeGame = true;

            timer.Duration = playDuration;
            timer.Active = true;

            scoreBoard.Goal = goalScore;
        }

        private void Update()
        {
            if (Mouse.current.rightButton.wasPressedThisFrame) activeCharge = true;
            else if (Mouse.current.rightButton.wasReleasedThisFrame) activeCharge = false;

            if (activeCharge && chargedCount < chargeTime) chargedCount += Time.deltaTime;
            else if (chargedCount < chargeTime && chargedCount > 0) chargedCount -= Time.deltaTime / 2;

            if (chargedCount > chargeTime) chargedCount = chargeTime;
            else if (chargedCount < 0) chargedCount = 0;

            progressBar.Value = chargedCount / chargeTime;
        }
    }
}