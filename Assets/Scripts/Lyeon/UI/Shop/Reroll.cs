using TMPro;
using UnityEngine;
using Wakamole.Lyeon.Manager.Game;
using Wakamole.Lyeon.Manager.Shop;
using Wakamole.Lyeon.UI.Play;

namespace Wakamole.Lyeon.UI.Shop
{
    public class Reroll : MonoBehaviour
    {
        [SerializeField] private TMP_Text rerollText;
        [SerializeField] private int rerollCost;
        [SerializeField] private Booth booth;

        private void Awake()
        {
            rerollText.text = $"{rerollCost}코인";
        }

        private void OnEnable()
        {
            rerollCost = 1;
        }

        public void Click()
        {
            if (GameManager.Current.Coin < rerollCost) return;

            GameManager.Current.Coin -= rerollCost;
            rerollCost *= 2;
            rerollText.text = $"{rerollCost}코인";
            booth.Reroll();
        }
    }
}