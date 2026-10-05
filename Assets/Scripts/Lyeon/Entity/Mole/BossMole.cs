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
            Active = true;
            base.OnEnable();
            bossHpBar.Active = true;
            bossHpBar.Value = 1;
        }

        protected override void OnDisable()
        {
            base.OnDisable();
        }

        protected override void Update()
        {
            
        }

        public override void SetHp(int value)
        {
            base.SetHp(value);
            if (bossHpBar != null) bossHpBar.Value = (float)currentHp / maxHp;
        }
    }
}
