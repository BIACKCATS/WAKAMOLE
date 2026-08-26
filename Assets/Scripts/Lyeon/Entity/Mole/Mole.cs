using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Wakamole.Core.LocalData;
using Wakamole.Lyeon.Entity.Component;
using Wakamole.Lyeon.Manager.Game;
using Wakamole.Lyeon.Manager.Play;

namespace Wakamole.Lyeon.Entity
{
    /// <summary>
    /// 두더지의 특성을 정의합니다. 여러 개의 특성을 가질 수 있습니다.
    /// </summary>
    [Flags]
    public enum MoleKeyword
    {
        DEFAULT = 0,
        FAST = 1,
        STRONG = 1 << 2,
        SPLIT = 1 << 3,
        REVIVE = 1 << 4,
        RICH = 1 << 5,
        POPULAR = 1 << 6,
        SHIELD = 1 << 7,

        // 시스템 구분용
        REVIVED = 1 << 31
    }

    /// <summary>
    /// 두더지의 정보를 전달하기 위한 구조체입니다.
    /// </summary>
    [Serializable]
    public struct MoleProfile
    {
        public float showTime;
        public int score, hp;
    }

    /// <summary>
    /// 두더지의 장식을 정의하기 위한 구조체입니다.
    /// </summary>
    [Serializable]
    public struct MoleDeco
    {
        public MoleKeyword keyword;
        public GameObject decorate;
    }

    public class Mole : MonoBehaviour
    {
        [Header("Components")]
        [Tooltip("두더지의 체력을 표시할 HpBar 스크립트를 포함한 GameObject입니다.")]
        [SerializeField] private HpBar hpBar;
        [SerializeField] private MoleAnim anim;

        [Header("Preferences")]
        [Tooltip("두더지가 올라오는 시간입니다.")]
        [SerializeField] protected float showTime = 3.0f;

        [Header("Information")]
        [Tooltip("두더지의 키워드(특성) 입니다.")]
        [SerializeField] protected MoleKeyword keyword;
        [Tooltip("두더지의 최대 체력입니다.")]
        [SerializeField] protected int maxHp = 10;
        [Tooltip("두더지의 현재 체력입니다.")]
        [SerializeField] protected int currentHp = 10;
        [Tooltip("두더지를 잡을 경우 획득 가능한 점수입니다.")]
        [SerializeField] protected int score = 5;

        [Header("Shield Mole")]
        [Tooltip("방패 특성을 가진 두더지가 가지는 방패의 개수입니다.")]
        [SerializeField] protected int shieldCount = 3;

        [Header("Popular Mole")]
        [Tooltip("인싸 특성을 가진 두더지가 동시에 등장하는 두더지의 최소 수입니다.")]
        [SerializeField] protected int popluarMin = 1;
        [Tooltip("인싸 특성을 가진 두더지가 동시에 등장하는 두더지의 최대 수입니다.")]
        [SerializeField] protected int popluarMax = 2;

        protected bool fixedHp = false, fixedTime = false, fixedScore = false;

        [Header("Decorations")]
        [Tooltip("두더지에 추가될 장식 입니다.")]
        [SerializeField] protected List<MoleDeco> decorations = new();

        private bool initialize = false, active = false;
        private float showedTime = 0;
        private Coroutine finishing = null;

        private MoleManager manager = null;

        public bool Active
        {
            get => active;
            set
            {
                active = value;
                if (!active)
                {
                    anim.PlayExternalState("Dead", true);
                    finishing = StartCoroutine(FinishTime());
                }
            }
        }

        /// <summary>
        /// 두더지 오브젝트를 관리하는 MoleManager입니다.
        /// </summary>
        public MoleManager Manager { set => manager = value; }

        /// <summary>
        /// 두더지의 현재 체력입니다. 만일 Hp Bar가 Inspector에 지정되지 않은 경우 오류가 발생할 수 있습니다.
        /// </summary>
            public int Hp
            {
                get => currentHp;
                set
                {
                    // [핵심 추가] 두더지가 이미 죽는 중이거나 비활성화 상태(active가 false)라면
                    // 더 이상 피격 연산이나 체력 감소, 분열 소환을 하지 않고 즉시 코드를 종료합니다.
                    if (!active) return;

                    if (currentHp > value)
                    {
                        anim.PlayExternalState("Hit");
                        if ((keyword & MoleKeyword.SHIELD) != 0 && shieldCount > 0)
                        {
                            shieldCount--;
                            return;
                        }
                    }
                    
                    currentHp = value;
                    if (currentHp < 0) currentHp = 0;

                    hpBar.Value = (float)currentHp / maxHp;
                    
                    if (currentHp <= 0)
                    {
                        // 💡 중요: 체력이 0이 되는 '최초의 순간'에 active를 false로 만들어 
                        // 연타를 하더라도 이 if문 안으로 두 번 다시 들어오지 못하게 잠가버립니다.
                        active = false;

                        if ((keyword & MoleKeyword.SPLIT) != 0)
                        {
                            manager.ShowMole(MoleKeyword.DEFAULT);
                            manager.ShowMole(MoleKeyword.DEFAULT);
                        }
                        if ((keyword & MoleKeyword.REVIVE) != 0)
                            manager.ShowMole(MoleKeyword.REVIVED);
                        if ((keyword & MoleKeyword.RICH) != 0)
                            StageManager.Current.Coin++;

                        Animator targetAnimator = anim.GetComponent<Animator>();
                        if (targetAnimator != null)
                        {
                            targetAnimator.SetTrigger("Die");
                        }

                        StartCoroutine(Co_DisableAfterAnimation());
                    }
                }
            }

        /// <summary>
        /// 두더지의 최대 체력입니다. 만일 현재 체력보다 작은 경우 현재 체력이 강제로 최대 체력만큼 설정됩니다.
        /// </summary>
        public int MaxHp
        {
            get => maxHp;
            set
            {
                if (value < currentHp) Hp = value;
                maxHp = value;
            }
        }

        /// <summary>
        /// 두더지를 잡을 경우 획득 가능한 점수입니다.
        /// </summary>
        public int Score => score;

        /// <summary>
        /// 두더지의 키워드입니다.
        /// </summary>
        public MoleKeyword Keyword => keyword;

        /// <summary>
        /// 두더지의 데이터를 초기화합니다.
        /// </summary>
        public void Init(int defaultHp, int defaultScore, float defaultTime)
        {   
            fixedHp = fixedScore = fixedTime = false;
            showTime = defaultTime;
            score = defaultScore;
            maxHp = defaultHp;
            Hp = defaultHp;
        }

        /// <summary>
        /// 두더지의 정보를 초기화합니다.
        /// </summary>
        /// <param name="keyword">두더지의 특성입니다.</param>
        /// <param name="moleData">두더지의 정보입니다.</param>
        public void AddKeyword(MoleKeyword keyword, MoleData moleData)
        {
            this.keyword = keyword;

            foreach (MoleDeco deco in decorations)
            {
                if ((keyword & deco.keyword) != 0 && deco.decorate != null)
                    deco.decorate.SetActive(true);
            }

            if (!fixedTime)
            {
                fixedTime = moleData.isFixedTime;
                if (fixedTime) showTime = moleData.showTime;
                else showTime += moleData.showTime;
            }

            if (!fixedScore)
            {
                fixedScore = moleData.isFixedScore;
                if (fixedScore) score = moleData.score;
                else score += moleData.score;
            }

            if (!fixedHp)
            {
                fixedHp = moleData.isFixedHp;
                if (fixedHp)
                {
                    maxHp = moleData.hp;
                    Hp = moleData.hp;
                }
                else
                {
                    maxHp += moleData.hp;
                    Hp += moleData.hp;
                }
            }

            active = true;
        }

        private void OnEnable()
        {
            if (!initialize) initialize = true;
            else anim.ResetToSpawn();
            
            showedTime = 0;
            if ((keyword & MoleKeyword.SHIELD) != 0) shieldCount = 3;
            if ((keyword & MoleKeyword.POPULAR) != 0)
            {
                int rand = UnityEngine.Random.Range(popluarMin, popluarMax + 1);
                for (int i = 0; i < rand; i++) manager.ShowMole(MoleKeyword.DEFAULT);
            }
        }

        private void Update()
        {
            if (!active) return;

            showedTime += Time.deltaTime;
            if (showedTime >= showTime) Active = false;
        }

        private void OnDisable()
        {
            if (manager != null) manager.HideMole(this);
            foreach (MoleDeco deco in decorations)
                deco.decorate.SetActive(false);
            keyword = MoleKeyword.DEFAULT;

            if (currentHp > 0 && StageManager.Current.Active)
            {
                if (GameManager.Current.Preference.ActiveFailScore)
                    StageManager.Current.Score += (int)(score * GameManager.Current.Preference.FailScore);
                StageManager.Current.Combo = 0;
            }
        }

private IEnumerator FinishTime()
        {
            // 0.5초 동안 죽는 애니메이션이 나오기를 기다리게 할거고..
            yield return new WaitForSeconds(0.5f);
            
            // [원인 제거] 여기에 있던 anim.InitAnimatorSetting(); 을 삭제했습니다!
            // 유니티 버그 원인: 끄기 직전에 Rebind를 하면 풀링 시 상태가 꼬여서 깨어납니다.

            gameObject.SetActive(false);
            finishing = null;
        }

private IEnumerator Co_DisableAfterAnimation()
        {
            Animator targetAnimator = anim.GetComponent<Animator>();
            if (targetAnimator == null)
            {
                gameObject.SetActive(false);
                yield break;
            }

            // 1. 애니메이터가 "Hit"에서 "Dead" 상태로 확실히 꺾일 때까지 1프레임 대기. 일단. 버그 걸린다고 하더라
            yield return null;

            // 2. 현재 애니메이터가 'Dead' 애니메이션 상태로 무사히 들어왔는지 확인해야하구
            int loopCount = 0;
            while (!targetAnimator.GetCurrentAnimatorStateInfo(0).IsName("Dead") && loopCount < 10)
            {
                yield return null; // Dead 상태가 될 때까지 최대 10프레임 동안 기다립니다.
                loopCount++;
            }

            // 3. 이제 진짜 'Dead' 애니메이션의 실제 재생 시간(초)을 가져오기
            float deadAnimLength = targetAnimator.GetCurrentAnimatorStateInfo(0).length;

            // 4. 두더지가 쓰러지는 연출 시간만큼 자로 잰 듯 정확하게 대기.
            yield return new WaitForSeconds(deadAnimLength);

            // [원인 제거] 제가 이전에 추가하라고 했던 초기화 코드를 삭제했습니다.
            // 그냥 죽은 상태 그대로 편안하게 오브젝트를 꺼야 합니다.

            // 5. 연출이 완벽히 끝났으므로 안전하게 오브젝트를 비활성화.
            gameObject.SetActive(false);
        }
    }
}
