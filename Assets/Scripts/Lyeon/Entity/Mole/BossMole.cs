using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Wakamole.Core.LocalData;
using Wakamole.Lyeon.Manager.Game;
using Wakamole.Lyeon.UI.Play.Boss;

namespace Wakamole.Lyeon.Entity
{
    /// <summary>
    /// 보스 두더지의 특성을 정의합니다.
    /// </summary>
    public enum BossKeyword
    {
        DEFAULT, // 기본값 (오류 방지용)
        FAST, STRONG, REVIVE, POPULAR, // 두더지 키워드 부여
        CLOCK, FIRE, PHANTOM, LABOR, RIBBON,
        OLDER, JOKER, SEC, FARMER, PET,
        SWORD, DOCTOR
    }

    /// <summary>
    /// 보스 두더지의 장식을 정의하기 위한 구조체입니다.
    /// </summary>
    [System.Serializable]
    public struct BossMoleDeco
    {
        public BossKeyword keyword;
        public GameObject decorate;
    }

    public class BossMole : Mole
    {
        [Header("Boss Mole")]
        [Tooltip("두더지의 정보를 표시할 BossMoleData Scriptable Object입니다.")]
        [SerializeField] private BossMoleData bossMoleData;
        [Tooltip("두더지의 체력을 표시할 BossHpBar 스크립트를 포함한 GameObject입니다.")]
        [SerializeField] private BossHpBar bossHpBar;
        [Tooltip("두더지에 추가될 장식 입니다. 만일 보스 두더지인 경우 해당 장식으로 덮어씌워집니다.")]
        [SerializeField] protected List<BossMoleDeco> bossDecorations = new();

        private BossKeyword bossKeyword;
        public BossKeyword BossKeyword => bossKeyword;

        public void SetBossKeyword(BossKeyword keyword)
        {
            bossKeyword = keyword;
            bossHpBar.SetBossName(bossMoleData.BossMoleName[bossKeyword]);
            ApplyKeyword();
        }

        private void Initialize()
        {
            List<BossKeyword> keywords = bossMoleData.BossMoleName.Keys.ToList();
            SetBossKeyword(keywords[Random.Range(0, keywords.Count)]);
        }

        private void ApplyKeyword()
        {
            if (GameManager.Current == null) return;

            // 보스 두더지의 키워드가 일반 두더지들의 키워드를 추가하는 키워드인 경우 확인
            BossKeyword[] keywords = new BossKeyword[4];
            keywords[0] = BossKeyword.FAST;
            keywords[1] = BossKeyword.STRONG;
            keywords[2] = BossKeyword.REVIVE;
            keywords[3] = BossKeyword.POPULAR;

            if (keywords.Contains(bossKeyword))
            {
                if (bossKeyword.Equals(BossKeyword.FAST)) GameManager.Current.SetBossKeyword(MoleKeyword.FAST);
                else if (bossKeyword.Equals(BossKeyword.STRONG)) GameManager.Current.SetBossKeyword(MoleKeyword.STRONG);
                else if (bossKeyword.Equals(BossKeyword.REVIVE)) GameManager.Current.SetBossKeyword(MoleKeyword.REVIVE);
                else if (bossKeyword.Equals(BossKeyword.POPULAR)) GameManager.Current.SetBossKeyword(MoleKeyword.POPULAR);
                else GameManager.Current.SetBossKeyword(MoleKeyword.DEFAULT);
            }
            else GameManager.Current.SetBossKeyword(MoleKeyword.DEFAULT);

            // 보스 두더지의 장식 활성화
            foreach (BossMoleDeco deco in bossDecorations)
            {
                if (deco.keyword.Equals(bossKeyword) && deco.decorate != null)
                {
                    deco.decorate.SetActive(true);
                    break;
                }
            }
        }

        protected override void OnEnable()
        {
            Active = true;
            base.OnEnable();
            bossHpBar.Active = true;
            bossHpBar.Value = 1;
            Initialize();
        }

        protected override void OnDisable()
        {
            base.OnDisable();
        }

        protected override void Update()
        {
            // 보스 두더지가 시간이 지나면 사라지는 것을 방지하기 위한 함수 오버라이딩
            // 지우면 보스 두더지 사라짐
        }

        public override void SetHp(int value)
        {
            base.SetHp(value);
            if (bossHpBar != null) bossHpBar.Value = (float)currentHp / maxHp;
        }
    }
}
