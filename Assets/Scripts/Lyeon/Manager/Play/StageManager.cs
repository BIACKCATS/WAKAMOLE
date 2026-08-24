using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Wakamole.Core.LocalData;
using Wakamole.Core.Utils;
using Wakamole.Lyeon.Entity;
using Wakamole.Lyeon.GameCamera;
using Wakamole.Lyeon.Manager.Component;
using Wakamole.Lyeon.Manager.Game;
using Wakamole.Lyeon.UI.Play;

namespace Wakamole.Lyeon.Manager.Play
{
    public class StageManager : MonoBehaviour
    {
        private static WaitForSeconds _waitForSeconds10_0 = new WaitForSeconds(10.0f);

        public static StageManager Current { get; private set; }

        [Header("Components")]
        [Tooltip("두더지를 관리하는 MoleManager 스크립트를 포함한 GameObject입니다.")]
        [SerializeField] private MoleManager moleManager;
        [Tooltip("클리어 시 게임 결과를 표시할 ClearBoard 스크립트를 포함한 GameObject입니다.")]
        [SerializeField] private ClearBoard clearBoard;
        [Tooltip("현재 점수를 표시할 ScoreBoard 스크립트를 포함한 GameObject입니다.")]
        [SerializeField] private ScoreBoard scoreBoard;
        [Tooltip("게임 제한 시간을 표시할 Clock 스크립트를 포함한 GameObject입니다.")]
        [SerializeField] private Clock clock;
        [Tooltip("현재 콤보 수를 표시할 Combo 스크립트를 포함한 GameObject입니다.")]
        [SerializeField] private Combo combo;
        [Tooltip("현재 획득한 코인을 표시할 CoinText 스크립트를 포함한 GameObject입니다.")]
        [SerializeField] private CoinText coinText;
        [Tooltip("아이템 효과로 표시할 Alert 스크립트를 포함한 GameObject입니다.")]
        [SerializeField] private Alert alert;
        [Tooltip("아이템 효과로 표시할 Mosquito 스크립트를 포함한 GameObject입니다.")]
        [SerializeField] private Mosquito mosquito;
        [Tooltip("게임 클리어 효과를 실행할 StageFinish 스크립트입니다.")]
        [SerializeField] private StageFinish stageFinish;
        [Tooltip("아이템 생성 규칙을 지정할 BackdropPreset SO입니다.")]
        [SerializeField] private BackdropPreset backdropPreset;
        [Tooltip("아이템 목록을 표시할 ItemIcon 스크립트를 포함한 GameObject의 목록입니다.")]
        [SerializeField] private List<ItemIcon> itemSlots;

        [Header("Informations")]
        [Tooltip("목표 점수입니다.")]
        [SerializeField] private int goalScore = 0;
        [Tooltip("스테이지의 제한 시간(초)입니다.")]
        [SerializeField] private float timeLimit = 0;
        [Tooltip("알림창이 뜨는 간격입니다.")]
        [SerializeField] private float alertTime = 5.0f;
        [Tooltip("모기가 나타나는 간격입니다.")]
        [SerializeField] private float mosquitoTime = 8.0f;

        private bool active = false;
        private int moleCount = 0;
        private int currentCoin = 0;
        private int currentScore = 0;
        private int maxCombo = 0;
        private int currentCombo = 0;

        private Timer timer = new();
        private Mole attackedMole = null;

        private MosquitoPool mosquitoPool;

        public bool Active { get => active; set => active = value; }
        public MoleManager MoleManager => moleManager;

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
        /// 공격당한 두더지입니다.
        /// </summary>
        public Mole AttackedMole { set => attackedMole = value; }

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
                    //stageFinish.FinishEffect(attackedMole, CameraController.Current, Finish);
                    Finish();
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
                if (value < timeLimit) CurrentTime = value;
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
                if (timer.Duration >= value) timer.Current = timer.Duration;
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
        public int Coin
        {
            get => currentCoin;
            set {
                currentCoin = value;
                coinText.Coin = currentCoin;
            }
        }

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

        private bool activeDoubleScore = false;
        private Coroutine doubleScore = null;
        /// <summary>
        /// 9번 아이템에 의한 점수 2배 적용
        /// </summary>
        public bool ActiveDoubleScore
        {
            get => activeDoubleScore;
            set
            {
                if (value)
                {
                    if (doubleScore != null) StopCoroutine(doubleScore);
                    doubleScore = StartCoroutine(DoubleTime());
                }
                else if (doubleScore != null)
                {
                    StopCoroutine(DoubleTime());
                    doubleScore = null;
                }
                activeDoubleScore = value;
            }
        }

        private IEnumerator DoubleTime()
        {
            yield return _waitForSeconds10_0;
            doubleScore = null;
            ActiveDoubleScore = false;
        }

        /// <summary>
        /// 5번 아이템에 의한 알림창 생성
        /// </summary>
        public void StartAlert()
        {
            StartCoroutine(Alert());
        }

        private IEnumerator Alert()
        {
            WaitForSeconds wait = new(alertTime);
            while (GameManager.Current.Preference.ActiveAlert && active)
            {
                alert.Show();
                yield return wait;
            }
        }

        /// <summary>
        /// 9번 아이템에 의한 모기 생성
        /// </summary>
        public void StartMosquito()
        {
            mosquitoPool = new(mosquito.gameObject, 10);
            StartCoroutine(Mosquito());
        }

        private IEnumerator Mosquito()
        {
            WaitForSeconds wait = new(mosquitoTime);
            while (GameManager.Current.Preference.ActiveMosquito && active)
            {
                mosquitoPool.Get().Active = true;
                yield return wait;
            }
        }

        private IEnumerator CreateBackdrops()
        {
            WaitForFixedUpdate wait = new();
            for (int i = 0; i < backdropPreset.backdropCount; i++)
            {
                yield return wait;
                GameObject obj = Instantiate(backdropPreset.backdropPrefabs[Random.Range(0, backdropPreset.backdropPrefabs.Count)]);
                obj.SetActive(false);
                Vector3 position = new(Random.Range(backdropPreset.minPoint.x, backdropPreset.maxPoint.x), obj.transform.position.y, Random.Range(backdropPreset.minPoint.z, backdropPreset.maxPoint.z));
                if (obj.TryGetComponent(out Backdrop component))
                {
                    component.InitPosition = position;
                    obj.transform.position = position;
                    obj.SetActive(true);
                }
            }
        }

        private void Awake()
        {
            if (goalScore <= 0) goalScore = 100;
            if (timeLimit <= 0) timeLimit = 100;

            Current = this;
            for (int i = 0; i < 5; i++)
            {
                if (!GameManager.Current.Inventory.ContainsKey(i)) continue;
                itemSlots[i].Item = GameManager.Current.Inventory[i];
            }

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
                Finish();
                return;
            }

            Current = this;
            clock.Current = timer.Current;
        }

        public void StartStage()
        {
            timer = new(timeLimit);
            Coin = 0;
            scoreBoard.Goal = goalScore;
            clock.Duration = timeLimit;
            active = true;
            StartCoroutine(CreateBackdrops());
        }

        private void Finish()
        {
            clearBoard.gameObject.SetActive(true);
            CameraController.Current.ExpandMove = false;
            CameraController.Current.TargetPosition = CameraController.Current.InitPosition;

            currentCoin += (int)(timer.Current / 10);
            clearBoard.Coin = currentCoin;
            clearBoard.Mole = moleCount;
            clearBoard.Score = currentScore;
            GameManager.Current.Coin += currentCoin;
        }

        private void OnDisable()
        {
            Current = null;
        }
    }
}