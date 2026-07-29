using UnityEngine;

namespace Wakamole.Lyeon.Manager
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Current { get; private set; }

        [Header("Components")]
        [Tooltip("게임 전체의 오디오를 관리하는 AudioManager입니다.")]
        [SerializeField] private AudioManager audioManager;

        public AudioManager Audio => audioManager;

        private int coin = 0;

        public int Coin { get => coin; set => coin = value; }

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