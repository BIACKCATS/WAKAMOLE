using TMPro;
using UnityEngine;

namespace Wakamole.Lyeon.UI.Play
{
    public class Combo : MonoBehaviour
    {
        [Header("Components")]
        [Tooltip("콤보 수를 표시할 텍스트입니다.")]
        [SerializeField] private TMP_Text comboText;

        public int Count { set => comboText.text = value.ToString(); }
    }
}