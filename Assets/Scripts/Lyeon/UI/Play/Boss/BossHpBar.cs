using UnityEngine;
using UnityEngine.UI;

namespace Wakamole.Lyeon.UI.Play.Boss
{
    public class BossHpBar : MonoBehaviour
    {
        [SerializeField] private Image hpBar;

        private float amount = 0;

        /// <summary>
        /// HpBar의 값입니다. 0 ~ 1 사이의 float값을 가집니다.
        /// </summary>
        public float Value
        {
            get => hpBar.fillAmount;
            set
            {
                if (value <= 0) amount = 0;
                else if (value >= 1) amount = 1;
                else amount = value;
            }
        }

        private void Awake()
        {
            hpBar.fillAmount = 0;
        }

        private void OnEnable()
        {
            amount = 1;   
        }

        private void Update()
        {
            hpBar.fillAmount = Mathf.Lerp(hpBar.fillAmount, amount, 15.0f * Time.deltaTime);
        }
    }
}