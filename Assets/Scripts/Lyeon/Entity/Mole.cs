using UnityEngine;
using Wakamole.Core.Utils;

namespace Wakamole.Lyeon.Entity
{
    public class Mole : MonoBehaviour
    {
        [SerializeField] private int maxHp = 10;
        [SerializeField] private int currentHp = 10;

        private ObjectPool pool;
        public ObjectPool Pool { set => pool = value; }

        public int Hp
        {
            get => currentHp;
            set => currentHp = value;
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
