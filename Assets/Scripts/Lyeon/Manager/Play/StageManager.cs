using UnityEngine;
using Wakamole.Core.Utils;
using Wakamole.Lyeon.UI;

namespace Wakamole.Lyeon.Manager.Play
{
    public class StageManager : MonoBehaviour
    {
        public static StageManager Current { get; private set; }

        [Header("Components")]
        [Tooltip("클리어 시 게임 결과를 표시할 ClearBoard 스크립트를 포함한 GameObject입니다.")]
        [SerializeField] private ClearBoard clearBoard;
        [Tooltip("현재 점수를 표시할 ScoreBoard 스크립트를 포함한 GameObject입니다.")]
        [SerializeField] private ScoreBoard scoreBoard;
        [Tooltip("게임 제한 시간을 표시할 Clock 스크립트를 포함한 GameObject입니다.")]
        [SerializeField] private Clock clock;
        [Tooltip("현재 콤보 수를 표시할 Combo 스크립트를 포함한 GameObject입니다.")]
        [SerializeField] private Combo combo;

        [Header("Informations")]
        [Tooltip("목표 점수입니다.")]
        [SerializeField] private int goalScore = 0;
        [Tooltip("스테이지의 제한 시간(초)입니다.")]
        [SerializeField] private float timeLimit = 0;

        private bool active = false;
        private int moleCount = 0;
        private int currentCoin = 0;
        private int currentScore = 0;
        private int maxCombo = 0;
        private int currentCombo = 0;

        private Timer timer = new();

        public bool Active { get => active; set => active = value; }

        /// <summary>
        /// 스테이지의 목표 점수입니다.
        /// </summary>
        public int Goal
        {
            get => goalScore;
            set
            {
                goalScore = value;
                scoreBoard.Goal = goalScore;
            }
        }

        /// <summary>
        /// 스테이지의 현재 점수입니다.
        /// </summary>
        public int Score
        {
            get => currentScore;
            set 
            {
                currentScore = value;
                scoreBoard.Current = currentScore;
                if (currentScore >= goalScore)
                {
                    active = false;
                    clearBoard.gameObject.SetActive(true);

                    clearBoard.Coin = currentCoin;
                    clearBoard.Mole = moleCount;
                    clearBoard.Score = currentScore;
                }
            }
        }

        /// <summary>
        /// 스테이지의 제한 시간입니다.
        /// </summary>
        public float TimeLimit
        {
            get => timeLimit;
            set
            {
                if (timer.Active) Debug.LogWarning("게임이 진행 중인 경우 제한 시간을 변경할 수 없습니다.");
                else timeLimit = value;
            }
        }

        /// <summary>
        /// 스테이지의 남은 시간입니다.
        /// </summary>
        public float CurrentTime
        {
            get => timer.Current;
            set
            {
                if (timer.Duration > value) timer.Current = timer.Duration;
                else timer.Current = value;
            }
        }

        /// <summary>
        /// 잡은 두더지의 수입니다.
        /// </summary>
        public int Count { get => moleCount; set => moleCount = value; }

        /// <summary>
        /// 획득한 코인의 수입니다.
        /// </summary>
        public int Coin { get => currentCoin; set => currentCoin = value; }

        /// <summary>
        /// 스테이지 내 콤보 수입니다.
        /// </summary>
        public int Combo
        {
            get => currentCombo;
            set
            {
                currentCombo = value;
                combo.Count = currentCombo;
                if (currentCombo > maxCombo) maxCombo = value;
            }
        }

        private void Awake()
        {
            if (goalScore <= 0) goalScore = 100;
            if (timeLimit <= 0) timeLimit = 100;

            Current = this;

            // for test
            StartStage();
        }

        private void Update()
        {
            if (!active) return;
            if (timer.Active) timer.Tick(Time.deltaTime);
            else
            {
                Debug.Log("타임 오버");
                active = false;
                return;
            }

            Current = this;
            clock.Current = timer.Current;
        }

        public void StartStage()
        {
            timer = new(timeLimit);
            scoreBoard.Goal = goalScore;
            clock.Duration = timeLimit;
            active = true;
        }

        private void OnDisable()
        {
            Current = null;
        }
    }
}