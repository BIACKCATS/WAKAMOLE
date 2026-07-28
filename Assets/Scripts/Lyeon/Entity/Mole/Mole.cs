using System;
using UnityEngine;
using Wakamole.Lyeon.Entity.Component;
using Wakamole.Lyeon.Manager;

namespace Wakamole.Lyeon.Entity
{
    /// <summary>
    /// 두더지의 특성을 정의합니다. 여러 개의 특성을 가질 수 있습니다.
    /// </summary>
    [Flags]
    public enum MoleKeyword
    {
        DEFAULT = 0, FAST = 1, REVIVE = 1 << 1, STRONG = 1 << 2, SPLIT = 1 << 3
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

    public class Mole : MonoBehaviour
    {
        [Header("Components")]
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

        private MoleManager manager = null;

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
                currentHp = value;
                if (currentHp < 0) currentHp = 0;

                hpBar.Value = (float)currentHp / maxHp;
                if (currentHp <= 0 && (keyword & MoleKeyword.SPLIT) != 0)
                {
                    manager.ShowMole(MoleKeyword.DEFAULT);
                    manager.ShowMole(MoleKeyword.DEFAULT);
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
        /// 두더지의 정보를 초기화합니다.
        /// </summary>
        /// <param name="keyword">두더지의 특성입니다.</param>
        /// <param name="moleProfile">두더지의 정보입니다.</param>
        public void SetProfile(MoleKeyword keyword, MoleProfile moleProfile)
        {
            this.keyword = keyword;
            showTime = moleProfile.showTime;
            score = moleProfile.score;
            maxHp = moleProfile.hp;
        }

        private void OnDisable()
        {
            if (manager != null) manager.ObjectPool.Return(gameObject);
        }

        private void OnEnable()
        {
            Hp = maxHp;
        }
    }
}
