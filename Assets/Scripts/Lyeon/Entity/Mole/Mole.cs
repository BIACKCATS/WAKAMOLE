using UnityEngine;
using Wakamole.Core.Utils;
using Wakamole.Lyeon.Entity.Component;

namespace Wakamole.Lyeon.Entity
{
    public class Mole : MonoBehaviour
    {
        [Header("Components")]
        [Tooltip("두더지의 체력을 표시할 HpBar 스크립트를 포함한 GameObject입니다.")]
        [SerializeField] private HpBar hpBar;

        [Header("Information")]
        [Tooltip("두더지의 최대 체력입니다.")]
        [SerializeField] private int maxHp = 10;
        [Tooltip("두더지의 현재 체력입니다.")]
        [SerializeField] private int currentHp = 10;

        private ObjectPool pool;
        /// <summary>
        /// 두더지 오브젝트 반환을 위한 ObjectPool입니다.
        /// </summary>
        public ObjectPool Pool { set => pool = value; }

        /// <summary>
        /// 두더지의 현재 체력입니다. 만일 Hp Bar가 Inspector에 지정되지 않은 경우 오류가 발생할 수 있습니다.
        /// </summary>
        public int Hp
        {
            get => currentHp;
            set {
                currentHp = value;
                hpBar.Value = (float)currentHp / maxHp;
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

        private void OnDisable()
        {
            pool?.Return(gameObject);
        }

        private void OnEnable()
        {
            Hp = maxHp;
        }
    }
}
