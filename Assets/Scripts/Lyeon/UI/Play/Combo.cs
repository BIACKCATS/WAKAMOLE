using TMPro;
using UnityEngine;

namespace Wakamole.Lyeon.UI.Play
{
    public class Combo : MonoBehaviour
    {
        [Header("Components")]
        [Tooltip("콤보 수를 표시할 텍스트입니다.")]
        [SerializeField] private TMP_Text comboText;
        [SerializeField] private RectTransform rect;
        [SerializeField] private RectTransform titleRect, comboRect;

        private Vector2 targetScale;
        private int comboCount = 0;

        public int Count
        {
            set
            {
                comboText.text = value.ToString();
                if (value != 0 && comboCount < value)
                {
                    titleRect.localScale = Vector2.one * 2.5f;
                    comboRect.localScale = Vector2.one * 2.5f;
                }
                comboCount = value;
            }
        }

        private void Awake()
        {
            targetScale = titleRect.localScale;
        }

        private void Update()
        {
            titleRect.localScale = Vector2.Lerp(titleRect.localScale, targetScale, 15.0f * Time.deltaTime);
            comboRect.localScale = Vector2.Lerp(comboRect.localScale, targetScale, 15.0f * Time.deltaTime);
        }
    }
}