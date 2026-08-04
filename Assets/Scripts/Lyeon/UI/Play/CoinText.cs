using TMPro;
using UnityEngine;

namespace Wakamole.Lyeon.UI.Play
{
    public class CoinText : MonoBehaviour
    {
        [SerializeField] private TMP_Text coinText;

        public int Coin { set => coinText.text = value.ToString(); }
    }
}