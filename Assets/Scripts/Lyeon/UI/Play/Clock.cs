using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Wakamole.Lyeon.UI.Play
{
    public class Clock : MonoBehaviour
    {
        [Header("Components")]
        [Tooltip("시계를 표시할 이미지입니다. Filled로 설정되어야 합니다.")]
        [SerializeField] private Image timerImage;
        [Tooltip("시계를 표시할 텍스트입니다.")]
        [SerializeField] private TMP_Text timerText;

        private bool active = false;
        private float duration = 1.0f;
        private float current = 1.0f;

        private int minute = 0, second = 0;

        /// <summary>
        /// 시계의 활성 상태입니다.
        /// </summary>
        public bool Active { get => active; set => active = value; }

        /// <summary>
        /// 시계의 설정 시간입니다.
        /// </summary>
        public float Duration
        {
            get => duration;
            set
            {
                duration = value;
                DisplayTime();
            }
        }

        /// <summary>
        /// 타이머의 현재 시간입니다.
        /// </summary>
        public float Current
        {
            get => current;
            set
            {
                current = value;
                DisplayTime();
            }
        }

        private void Update()
        {
            timerImage.fillAmount = Mathf.Lerp(timerImage.fillAmount, current / duration, 15.0f * Time.deltaTime);
        }

        private void DisplayTime()
        {
            minute = (int)(current / 60.0f);
            second = (int)(current - (minute * 60.0f));
            if (timerText != null) timerText.text = string.Format("{0:D2}:{1:D2}", minute, second);
        }
    }
}