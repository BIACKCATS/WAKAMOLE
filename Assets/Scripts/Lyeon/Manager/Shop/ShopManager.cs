using UnityEngine;
using Wakamole.Lyeon.Manager.Game;
using Wakamole.Lyeon.UI;
using Wakamole.Lyeon.UI.Play;

namespace Wakamole.Lyeon.Manager.Shop
{
    public class ShopManager : MonoBehaviour
    {
        public static ShopManager Current { get; private set; }

        [SerializeField] private ItemTooltip tooltip;
        [SerializeField] private CoinText coinText;

        public ItemTooltip Tooltip => tooltip;

        public void UpdateCoin()
        {
            coinText.Coin = GameManager.Current.Coin;
        }

        private void OnEnable()
        {
            Current = this;
            coinText.Coin = GameManager.Current.Coin;
            GameManager.Current.Audio.SetBgmParameter("ShopEnter", 100);
        }

        private void OnDisable()
        {
            GameManager.Current.Audio.SetBgmParameter("ShopEnter", 0);
            Current = null;
        }
    }
}