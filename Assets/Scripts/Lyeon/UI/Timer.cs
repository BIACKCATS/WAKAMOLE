using TMPro;
using UnityEngine;
using Wakamole.Lyeon.Player;

namespace Wakamole.Lyeon.UI
{
    public class Timer : MonoBehaviour
    {
        [Header("Components")]
        [Tooltip("타이머를 표시할 텍스트입니다.")]
        [SerializeField] private TMP_Text timerText;

        private bool active = false;
        private float duration = 1.0f;
        private float current = 1.0f;

        private int minute = 0, second = 0;

        /// <summary>
        /// 타이머의 활성 상태입니다.
        /// </summary>
        public bool Active { get => active; set => active = value; }
        /// <summary>
        /// 타이머의 설정 시간입니다.
        /// </summary>
        public float Duration
        {
            get => duration;
            set
            {
                if (!active)
                {
                    duration = value;
                    current = value;
                }
                else Debug.LogWarning("타이머 시간은 진행 중 변경할 수 없습니다.");
            }
        }
        /// <summary>
        /// 타이머의 현재 시간입니다.
        /// </summary>
        public float Current { get => current; set => current = value; }

        private void Update()
        {
            if (!active) return;
            if (current <= 0)
            {
                active = false;
                PlayerManager.Current.Active = false;
                return;
            }
            
            current -= Time.deltaTime;
            DisplayTime();
        }

        /// <summary>
        /// 타이머를 일시정지합니다.
        /// </summary>
        public void Pause() => active = false;

        /// <summary>
        /// 타이머를 초기화합니다.
        /// </summary>
        public void Reset()
        {
            active = false;
            current = duration;
            DisplayTime();
        }

        private void DisplayTime()
        {
            minute = (int)(current / 60.0f);
            second = (int)(current - (minute * 60.0f));
            timerText.text = string.Format("{0:D2}:{1:D2}", minute, second);
        }
    }
}