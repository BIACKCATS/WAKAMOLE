using TMPro;
using UnityEngine;
using Wakamole.Lyeon.Manager.Game;
using Wakamole.Lyeon.Manager.Shop;

namespace Wakamole.Lyeon.UI.Shop
{
    public class Reroll : MonoBehaviour
    {
        [SerializeField] private TMP_Text rerollText;
        [SerializeField] private int rerollCost;
        [SerializeField] private Booth booth;

        private void Awake()
        {
            rerollText.text = $"리롤: 코인 {rerollCost}개";
        }

        public void Click()
        {
            if (GameManager.Current.Coin < rerollCost) return;

            GameManager.Current.Coin -= rerollCost;
            booth.Reroll();
        }
    }
}