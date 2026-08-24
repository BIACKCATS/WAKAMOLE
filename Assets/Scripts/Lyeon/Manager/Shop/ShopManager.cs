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

        private void OnEnable()
        {
            Current = this;
            coinText.Coin = GameManager.Current.Coin;
        }

        private void OnDisable()
        {
            Current = null;
        }
    }
}