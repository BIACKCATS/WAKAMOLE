using UnityEngine;

namespace Wakamole.Lyeon.Player
{
    public class PlayerManager : MonoBehaviour
    {
        public static PlayerManager Current { get; private set; }

        [SerializeField] private int atk = 1;

        public int Atk { get => atk; set => atk = value; }

        private void Awake()
        {
            if (Current != null)
            {
                Destroy(gameObject);
                return;
            }

            Current = this;
            DontDestroyOnLoad(gameObject);
        }
    }
}