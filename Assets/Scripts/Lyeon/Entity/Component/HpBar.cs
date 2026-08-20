using UnityEngine;

namespace Wakamole.Lyeon.Entity.Component
{
    public class HpBar : MonoBehaviour
    {
        private Vector3 scale = Vector3.zero;

        /// <summary>
        /// HpBar의 값입니다. 0 ~ 1 사이의 float값을 가집니다.
        /// </summary>
        public float Value
        {
            get => scale.x;
            set
            {
                if (value < 0) scale.x = 0;
                else scale.x = value;
            }
        }

        private void Awake()
        {
            scale = transform.localScale;
        }

        private void OnEnable()
        {
            scale = Vector3.one;
            transform.localScale = scale;
        }

        private void Update()
        {
            transform.localScale = Vector3.Lerp(transform.localScale, scale, 15.0f * Time.deltaTime);
        }
    }
}