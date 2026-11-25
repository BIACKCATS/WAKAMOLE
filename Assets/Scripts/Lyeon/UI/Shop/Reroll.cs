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
            rerollText.SetText("{0}코인", rerollCost);
        }

        private void OnEnable()
        {
            rerollCost = 1;
        }

        public void Click()
        {
            if (GameManager.Current.Coin < rerollCost)
            {
                ShopManager.Current?.Alert.Show();
                return;
            }

            GameManager.Current.Coin -= rerollCost;
            ShopManager.Current?.UpdateCoin();
            rerollCost *= 2;
            rerollText.SetText("{0}코인", rerollCost);
            booth.Reroll();
        }
    }
}