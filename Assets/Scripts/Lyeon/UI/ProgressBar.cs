using UnityEngine;
using UnityEngine.UI;

namespace Wakamole.Lyeon.UI
{
    public class ProgressBar : MonoBehaviour
    {
        [Header("Components")]
        [Tooltip("진행도를 표시할 Image입니다. Filled로 설정되어야 합니다.")]
        [SerializeField] protected Image image;
        protected float value = 0;

        /// <summary>
        /// ProgressBar의 값입니다. 0 ~ 1 사이의 float값을 가집니다.
        /// </summary>
        public float Value
        {
            get => value;
            set
            {
                if (value < 0) this.value = 0;
                else this.value = value;
            }
        }

        protected virtual void Update()
        {
            image.fillAmount = Mathf.Lerp(image.fillAmount, value, 15.0f * Time.deltaTime);
        }
    }
}