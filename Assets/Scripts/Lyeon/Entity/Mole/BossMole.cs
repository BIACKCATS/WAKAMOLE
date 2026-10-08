using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Wakamole.Core.LocalData;
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

    public class BossMole : Mole
    {
        [Header("Boss Mole")]
        [Tooltip("두더지의 정보를 표시할 BossMoleData Scriptable Object입니다.")]
        [SerializeField] private BossMoleData bossMoleData;
        [Tooltip("두더지의 체력을 표시할 BossHpBar 스크립트를 포함한 GameObject입니다.")]
        [SerializeField] private BossHpBar bossHpBar;

        private BossKeyword bossKeyword;
        public BossKeyword BossKeyword => bossKeyword;

        public void SetBossKeyword(BossKeyword keyword)
        {
            bossKeyword = keyword;
            bossHpBar.SetBossName(bossMoleData.BossMoleName[bossKeyword]);
        }

        private void Initialize()
        {
            List<BossKeyword> keywords = bossMoleData.BossMoleName.Keys.ToList();
            bossKeyword = keywords[Random.Range(0, keywords.Count)];
            bossHpBar.SetBossName(bossMoleData.BossMoleName[bossKeyword]);
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
