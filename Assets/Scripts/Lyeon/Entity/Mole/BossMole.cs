using UnityEngine;
using Wakamole.Lyeon.UI.Play.Boss;

namespace Wakamole.Lyeon.Entity
{
    public class BossMole : Mole
    {
        [Header("Boss Mole")]
        [Tooltip("두더지의 체력을 표시할 BossHpBar 스크립트를 포함한 GameObject입니다.")]
        [SerializeField] private BossHpBar bossHpBar;

        protected override void OnEnable()
        {
            base.OnEnable();
            bossHpBar.Active = true;
            bossHpBar.Value = 1;
        }

        protected override void OnDisable()
        {
            base.OnDisable();
        }

        public override void SetHp(int value)
        {
            bossHpBar.Value = value;
        }
    }
}
