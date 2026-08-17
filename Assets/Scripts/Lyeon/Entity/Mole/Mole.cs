using System;
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
        [Tooltip("두더지를 표시하는 스크립트를 포함한 GameObject입니다.")]
        [SerializeField] private MoleCharactor charactor;
        [Tooltip("두더지의 체력을 표시할 HpBar 스크립트를 포함한 GameObject입니다.")]
        [SerializeField] private HpBar hpBar;

        [Header("Preferences")]
        [Tooltip("두더지가 올라오는 시간입니다.")]
        [SerializeField] protected float showTime = 3.0f;
        [Tooltip("두더지가 올라오고 내려가는 속도입니다.")]
        [SerializeField] protected float moveSpeed = 8.0f;

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

        private MoleManager manager = null;
        public bool Active { get => charactor.Active; set => charactor.Active = false; }
        public bool Moving => charactor.Moving;
        public bool Interactable => charactor.Interactable;

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
            set {
                if (currentHp > value && (keyword & MoleKeyword.SHIELD) != 0 && shieldCount > 0)
                {
                    shieldCount--;
                    return;
                }
                        
                currentHp = value;
                if (currentHp < 0) currentHp = 0;

                hpBar.Value = (float)currentHp / maxHp;
                if (currentHp <= 0)
                {
                    if ((keyword & MoleKeyword.SPLIT) != 0)
                    {
                        manager.ShowMole(MoleKeyword.DEFAULT);
                        manager.ShowMole(MoleKeyword.DEFAULT);
                    }
                    if ((keyword & MoleKeyword.REVIVE) != 0)
                        manager.ShowMole(MoleKeyword.REVIVED);
                    if ((keyword & MoleKeyword.RICH) != 0)
                        StageManager.Current.Coin++;
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
        /// 두더지가 등장하는 시간입니다.
        /// </summary>
        public float ShowTime => showTime;

        /// <summary>
        /// 두더지가 움직이는 속도입니다.
        /// </summary>
        public float MoveSpeed => moveSpeed;

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

            charactor.ShowTime = showTime;
            charactor.MoveSpeed = moveSpeed;
            charactor.Active = true;
        }

        private void OnEnable()
        {
            if ((keyword & MoleKeyword.SHIELD) != 0) shieldCount = 3;
            if ((keyword & MoleKeyword.POPULAR) != 0)
            {
                int rand = UnityEngine.Random.Range(popluarMin, popluarMax + 1);
                for (int i = 0; i < rand; i++) manager.ShowMole(MoleKeyword.DEFAULT);
            }
        }

        private void OnDisable()
        {
            if (manager != null) manager.HideMole(this);
            foreach (MoleDeco deco in decorations)
                deco.decorate.SetActive(false);
            keyword = MoleKeyword.DEFAULT;

            if (currentHp > 0 && StageManager.Current.Active && GameManager.Current.Preference.ActiveFailScore)
                StageManager.Current.Score += (int)(score * GameManager.Current.Preference.FailScore);
        }
    }
}
